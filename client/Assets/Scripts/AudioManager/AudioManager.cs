using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public enum AudioGroupTypes
{
    Default = 0,
    Voice = 1,
    Fight = 2,
    Environment = 3,
    Music = 4,
}

public class AudioManager : ManagerTemplateBase<AudioManager>
{
    public static int AUDIO_POOL_SIZE = 30;

    static Transform listenerTarget;
    static Transform listenerTrans;

    static LinkedList<AudioObject> pool = new LinkedList<AudioObject>();
    static LinkedList<AudioObject> activeAudio = new LinkedList<AudioObject>();

    static AudioCfgLoader audioCfgLoader;
    static AudioMixer defaultMixer;
    static Dictionary<int, AudioMixerGroup> audioMixerGroups = new Dictionary<int, AudioMixerGroup>();

    static HashSet<int> audioPlayedCurrFrame = new HashSet<int>();

    protected override void InitManager()
    {
        audioCfgLoader = ConfigManager.Get<AudioCfgLoader>();
        defaultMixer = Resources.Load<AudioMixer>("AudioMixer");
        if (defaultMixer != null)
        {
            var audioGroupVals = System.Enum.GetValues(typeof(AudioGroupTypes));
            var audioGroupNames = System.Enum.GetNames(typeof(AudioGroupTypes));
            for (int i = 0; i < audioGroupVals.Length; i++)
            {
                audioMixerGroups[(int)audioGroupVals.GetValue(i)] = defaultMixer.FindMatchingGroups((string)audioGroupNames.GetValue(i))[0];
            }
        }

        DestroyExistingListener();

        GameObject listenerGo = new GameObject("AudioListener");
        listenerGo.transform.parent = transform;
        listenerTrans = listenerGo.transform;
        listenerGo.AddComponent<AudioListener>();

        GameEventManager.RegisterEvent(GameEventTypes.ExitScene, OnExitScene);
        GameEventManager.RegisterEvent(GameEventTypes.EnterScene, OnEnterScene);
    }

    private void OnExitScene(GameEventTypes eventType, object[] args)
    {
        LinkedListNode<AudioObject> curNode = activeAudio.First;
        int count = activeAudio.Count;
        for (int i = 0; i < count; i++)
        {
            curNode.Value.Destroy();
            curNode = curNode.Next;
        }

        curNode = pool.First;
        count = pool.Count;
        for (int i = 0; i < count; i++)
        {
            curNode.Value.Destroy();
            curNode = curNode.Next;
        }

        activeAudio.Clear();
        audioPlayedCurrFrame.Clear();
        pool.Clear();
    }

    private static void OnEnterScene(GameEventTypes eventType, object[] args)
    {
        DestroyExistingListener();
        RestoreSettings();
    }

    private static void DestroyExistingListener()
    {
        var mainCamera = Camera.main;

        if (mainCamera != null)
        {
            var audioListener = mainCamera.GetComponent<AudioListener>();

            if (audioListener != null)
            {
                DestroyImmediate(audioListener);
            }
        }
    }

    public static void SetListener(Transform target)
    {
        listenerTarget = target;
    }

    public static AudioObject PlayAudio(int id, Vector3 position)
    {
        AudioObject audioObj = FetchAudioObject(id);
        if (audioObj == null)
            return null;

        activeAudio.AddLast(audioObj);

        audioObj.Play(position);

        return audioObj;
    }

    public static AudioObject PlayAudio(int id, Transform target)
    {
        AudioObject audioObj = FetchAudioObject(id);
        if (audioObj == null)
            return null;

        activeAudio.AddLast(audioObj);

        audioObj.Play(target);

        return audioObj;
    }

    private static AudioObject FetchAudioObject(int id)
    {
        // 相同音效同一帧不允许放多个
        if (audioPlayedCurrFrame.Contains(id))
            return null;

        AudioObject audioObj = FetchFromPool(id);

        if (audioObj == null)
            audioObj = CreateAudioObject(id);

        audioPlayedCurrFrame.Add(id);

        return audioObj;
    }

    private static AudioObject CreateAudioObject(int id)
    {
        AudioCfg config = audioCfgLoader.GetConfig(id);

        if (config == null)
        {
            DebugLogger.LogErrorFormat("[AudioManager]:Failed to find config of audio {0}", id);
            return null;
        }

        AudioObject audioObj = new AudioObject(config, Instance.transform);

        return audioObj;
    }

    private static AudioObject FetchFromPool(int id)
    {
        LinkedListNode<AudioObject> curNode = pool.First;
        int count = pool.Count;

        for (int i = 0; i < count; i++)
        {
            LinkedListNode<AudioObject> next = curNode.Next;

            if (curNode.Value.AudioID == id)
            {
                pool.Remove(curNode);
                return curNode.Value;
            }

            curNode = next;
        }

        return null;
    }

    void Update()
    {
        audioPlayedCurrFrame.Clear();

        if (activeAudio.Count == 0)
            return;

        if (listenerTarget != null)
        {
            listenerTrans.position = listenerTarget.position;
        }

        LinkedListNode<AudioObject> curNode = activeAudio.First;

        int count = activeAudio.Count;
        for (int i = 0; i < count; i++)
        {
            LinkedListNode<AudioObject> next = curNode.Next;

            curNode.Value.Update();

            if (curNode.Value.Expired)
            {
                ProcessExpiredEffect(curNode.Value);
                activeAudio.Remove(curNode);
            }

            curNode = next;
        }
    }

    private static void ProcessExpiredEffect(AudioObject audioObj)
    {
        if (pool.Count < AUDIO_POOL_SIZE)
        {
            pool.AddLast(audioObj);
        }
        else
        {
            pool.AddLast(audioObj);
            var first = pool.First;
            pool.RemoveFirst();
            first.Value.Destroy();
        }
    }

    public static AudioObject Find(int audioID)
    {
        LinkedListNode<AudioObject> curNode = activeAudio.First;

        int count = activeAudio.Count;
        for (int i = 0; i < count; i++)
        {
            if (curNode.Value.AudioID == audioID)
            {
                return curNode.Value;
            }

            curNode = curNode.Next;
        }

        return null;
    }

    public static AudioMixerGroup GetMixerGroup(AudioGroupTypes type)
    {
        AudioMixerGroup group;
        audioMixerGroups.TryGetValue((int)type, out group);
        return group;
    }

    public static void SetVolume(AudioGroupTypes type, float volume)
    {
        AudioMixerGroup group = GetMixerGroup(type);
        if (group == null)
            return;

        float db = Mathf.Lerp(-80, 0, volume);
        group.audioMixer.SetFloat(string.Format("{0}:Volume", Enum.GetName(typeof(AudioGroupTypes), type)), db);
    }

    public static void SetMusicVolume(float value)
    {
        AudioManager.SetVolume(AudioGroupTypes.Music, value);
    }

    public static void SetAudioVolume(float value)
    {
        SetVolume(AudioGroupTypes.Environment, value);
        SetVolume(AudioGroupTypes.Fight, value);
        SetVolume(AudioGroupTypes.Voice, value);
        SetVolume(AudioGroupTypes.Default, value);
    }

    private static void RestoreSettings()
    {
        if (PlayerPrefs.GetInt("AudioOn", 1) > 0)
        {
            SetAudioVolume(PlayerPrefs.GetFloat("AudioVolume", 1));
        }
        else
        {
            SetAudioVolume(0);
        }

        if (PlayerPrefs.GetInt("MusicOn", 1) > 0)
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
        }
        else
        {
            SetMusicVolume(0);
        }
    }
}



