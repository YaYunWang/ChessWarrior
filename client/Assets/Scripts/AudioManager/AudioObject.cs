using UnityEngine;


public class AudioObject
{
    class AudioTarget
    {
        public bool hasTransform;

        public Transform target;
        public Vector3 targetPos;

        public void Clear()
        {
            hasTransform = false;
            target = null;
            targetPos = Vector3.zero;
        }

        public void SetTransform(Transform target)
        {
            this.hasTransform = true;
            this.target = target;
            this.targetPos = target.position;
        }

        public void SetPosition(Vector3 position)
        {
            this.hasTransform = false;
            this.target = null;
            this.targetPos = position;
        }

        public Vector3 GetPosition()
        {
            if (hasTransform && target != null)
                return target.position;
            else
                return targetPos;
        }

        public bool IsValid()
        {
            return !hasTransform || target != null;
        }
    }

    enum AudioStates
    {
        Initial,
        FadeIn,
        Playing,
        FadeOut,
        Expired,
    }

    AudioCfg audioCfg;
    GameObject gameObject;
    Transform transform;
    AudioSource audioSource;
    AudioClip audioClip;
    AudioTarget audioTarget;
    AudioStates audioStates;

    string assetBundleName;
    AssetBundleLoadAssetOperation assetLoadOperation;

    string audioBundlePath = "audio/audio_{0}.bundle";

    int stateElapsed;

    public int AudioID { protected set; get; }
    public bool Expired { get { return audioStates == AudioStates.Expired; } }
    public bool IsLoop { get { return audioCfg != null && audioCfg.Loop; } }

    public AudioObject(AudioCfg audioCfg, Transform root)
    {
        this.audioCfg = audioCfg;
        AudioID = audioCfg.ID;
        gameObject = new GameObject(audioCfg.ID.ToString());
        Disable();
        transform = gameObject.transform;
        transform.SetParent(root);

        audioTarget = new AudioTarget();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.GetMixerGroup((AudioGroupTypes)audioCfg.Priority);
        audioSource.volume = audioCfg.Volume;
        audioSource.loop = audioCfg.Loop;
        audioSource.dopplerLevel = 0;

        assetBundleName = string.Format(audioBundlePath, audioCfg.AssetName.ToLower());
        assetLoadOperation = AssetLoadManager.LoadAssetAsync(assetBundleName, audioCfg.AssetName, typeof(AudioClip));
    }

    void Enable()
    {
        gameObject.SetActive(true);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Play(Transform trans)
    {
        audioTarget.Clear();
        audioTarget.SetTransform(trans);
        PlayInternal();
    }

    public void Play(Vector3 position)
    {
        audioTarget.Clear();
        audioTarget.SetPosition(position);
        PlayInternal();
    }

    void PlayInternal()
    {
        Enable();
        ResetState();
        UpdatePosition();
    }

    public void Stop()
    {
        if (audioStates == AudioStates.Playing)
        {
            MoveToNextState();
        }
        else
        {
            StopImmediate();
        }
    }

    public void StopImmediate()
    {
        audioStates = AudioStates.Expired;
        Disable();
    }

    public void Destroy()
    {
        if (assetLoadOperation != null)
        {
            assetLoadOperation.Unload();
            assetLoadOperation = null;
        }

        if (gameObject != null)
        {
            Object.Destroy(gameObject);
            gameObject = null;
        }
    }

    void ResetState()
    {
        audioStates = AudioStates.Initial;
        stateElapsed = 0;
        audioSource.volume = audioCfg.Volume;
    }

    void MoveToNextState()
    {
        AudioStates curState = audioStates;
        stateElapsed = 0;

        switch (curState)
        {
            case AudioStates.Initial:
                if (audioCfg.FadeIn > 0)
                {
                    audioStates = AudioStates.FadeIn;
                    audioSource.volume = 0;
                }
                else
                {
                    audioStates = AudioStates.Playing;
                    audioSource.volume = audioCfg.Volume;
                }
                break;

            case AudioStates.FadeIn:
                audioStates = AudioStates.Playing;
                audioSource.volume = audioCfg.Volume;

                break;

            case AudioStates.Playing:
                if (audioCfg.FadeOut > 0)
                {
                    audioStates = AudioStates.FadeOut;
                }
                else
                {
                    audioStates = AudioStates.Expired;
                    Disable();
                }

                break;

            case AudioStates.FadeOut:
                audioStates = AudioStates.Expired;
                Disable();
                break;
        }
    }

    public void Update()
    {
        if (audioClip == null)
        {
            CheckLoading();
            return;
        }

        UpdateState();

        if (audioStates == AudioStates.Expired)
            return;

        UpdatePosition();
    }

    void UpdateState()
    {
        if (!audioTarget.IsValid())
        {
            StopImmediate();
            return;
        }

        stateElapsed += (int)(Time.deltaTime * 1000);

        switch (audioStates)
        {
            case AudioStates.Initial:
                MoveToNextState();
                break;
            case AudioStates.FadeIn:
                audioSource.volume = Mathf.Lerp(0, audioCfg.Volume, (float)stateElapsed / audioCfg.FadeIn);
                if (stateElapsed > audioCfg.FadeIn)
                    MoveToNextState();
                break;

            case AudioStates.Playing:
                if (!audioCfg.Loop)
                {
                    if (!audioSource.isPlaying || (audioClip.length - audioSource.time) < audioCfg.FadeOut * 0.001f)
                    {
                        MoveToNextState();
                    }
                }
                break;

            case AudioStates.FadeOut:
                audioSource.volume = Mathf.Lerp(audioCfg.Volume, 0, (float)stateElapsed / audioCfg.FadeOut);
                if (stateElapsed > audioCfg.FadeOut)
                    MoveToNextState();
                break;
        }
    }

    void UpdatePosition()
    {
        if (audioTarget.hasTransform)
            transform.position = audioTarget.GetPosition();
    }

    void CheckLoading()
    {
        if (assetLoadOperation != null)
        {
            if (assetLoadOperation.IsDone())
            {
                audioClip = assetLoadOperation.GetAsset<AudioClip>();
                audioSource.clip = audioClip;
                if (audioClip == null)
                {
                    assetLoadOperation.Unload();
                    assetLoadOperation = null;
                    Stop();
                }
                else
                    audioSource.Play();
            }
        }
    }
}


