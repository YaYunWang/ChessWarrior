using UnityEngine;
using UnityEngine.Profiling;
using System.Collections.Generic;

public class EffectObject
{
    #region EffectTarget
    struct EffectTarget
    {
        public bool hasTransform;

        public Transform root;
        public Transform bpTrans;

        public Vector3 position;
        public Quaternion rotation;

        public void Clear()
        {
            hasTransform = false;
            root = null;
            bpTrans = null;
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }

        public void SetTransform(Transform root, string bp)
        {
            this.hasTransform = true;
            this.root = root;

            if (!string.IsNullOrEmpty(bp))
                this.bpTrans = root.Search(bp);

            if (this.bpTrans == null)
                this.bpTrans = root;

            this.position = bpTrans.position;
            this.rotation = bpTrans.rotation;
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            this.hasTransform = false;

            this.root = null;
            this.bpTrans = null;

            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            if (hasTransform && bpTrans != null)
            {
                return bpTrans.TransformPoint(point);
            }
            else
            {
                return position + point;
            }
        }

        public Quaternion TransformRotation(Quaternion localRotation)
        {
            if (hasTransform && bpTrans != null)
            {
                return bpTrans.rotation * localRotation;
            }
            else
            {
                return this.rotation * localRotation;
            }
        }

        public Vector3 TransformScale(Vector3 localScale)
        {
            if (hasTransform && bpTrans != null)
            {
                Vector3 lossyScale = bpTrans.lossyScale;
                lossyScale.x *= localScale.x;
                lossyScale.y *= localScale.y;
                lossyScale.z *= localScale.z;
                return lossyScale;
            }
            else
            {
                return localScale;
            }
        }

        public bool IsValid()
        {
            return !hasTransform || bpTrans != null;
        }
    }
    #endregion

    enum EffectStates
    {
        Initial,
        Delay,
        Playing,
        FadeOut,
        Expired,
    }


    // -------------
    EffectCfg config;
    public GameObject gameObject;
    Transform transform;
    GameObject modelGo;
    Transform modelTrans;
    Renderer[] renderers;
    int activelayer = 0;

    AssetBundleLoadAssetOperation assetLoadOperation;
    string assetBundleName;
    ParticleSystem[] particleSystems;

    // -------------
    public EffectCfg Config { get { return this.config; } }
    public int EffectID { protected set; get; }
    public bool Expired { get { return state == EffectStates.Expired; } }
    public bool IsLoaded { get { return modelGo != null; } }
    public GameObject Model { get { return modelGo; } }
    public bool IsLoop
    {
        get
        {
            if (config == null)
            {
                return false;
            }
            return config.Lifetime <= 0;
        }
    }

    EffectTarget target;
    int stateElapsed;
    EffectStates state;
    EffectRuntimeQuality runtimeQuality;

    AudioObject audioObject;
    List<EffectObject> companionEffects;

    EffectFadeOut fadeout;
    EffectQualitySwitcher qualitySwitcher;

    System.Action<EffectObject> onLoadCallback;
    System.Func<Renderer, bool> applyOnRenderersCallback;

    string effectBundlePath = "effect/effect_{0}.bundle";

    public EffectObject(EffectCfg config, Transform root)
    {
        this.config = config;
        this.EffectID = config.ID;
        this.gameObject = new GameObject(config.ID.ToString());
        this.transform = gameObject.transform;
        this.transform.SetParent(root);
        this.assetBundleName = string.Format(effectBundlePath, config.AssetName.ToLower());
        this.activelayer = LayerManager.DefaultLayer;

        assetLoadOperation = AssetLoadManager.LoadAssetAsync(assetBundleName, config.AssetName, typeof(GameObject));
    }

    public void Start(Transform trans, System.Action<EffectObject> onLoad, EffectRuntimeQuality quality)
    {
        target.Clear();
        target.SetTransform(trans, config.BindPoint);
        StartInternal(onLoad, quality);
    }

    public void Start(Vector3 position, Quaternion rotation, System.Action<EffectObject> onLoad, EffectRuntimeQuality quality)
    {
        target.Clear();
        target.SetPosition(position, rotation);
        StartInternal(onLoad, quality);
    }

    private void StartInternal(System.Action<EffectObject> onLoad, EffectRuntimeQuality quality)
    {
        onLoadCallback = onLoad;
        applyOnRenderersCallback = null;
        runtimeQuality = quality;
        if (qualitySwitcher != null)
            qualitySwitcher.SetQuality(runtimeQuality);

        ResetState();

        UpdatePosition();
        UpdateScale();
        if (config.InitFollowRotation)
            UpdateRotation();
    }

    public void Stop()
    {
        Profiler.BeginSample("EffectStop");
        if (this.state == EffectStates.Playing)
            MoveToNextState();
        else if (this.state == EffectStates.FadeOut) { }
        else
            StopImmediate();
        Profiler.EndSample();


        if (companionEffects != null && companionEffects.Count > 0)
        {
            for (int i = 0; i < companionEffects.Count; i++)
            {
                Profiler.BeginSample("StopcompanionEffects");
                companionEffects[i].Stop();
                Profiler.EndSample();
            }
        }
    }

    public void StopImmediate()
    {
        state = EffectStates.Expired;
        Hide();

        if (companionEffects != null && companionEffects.Count > 0)
        {
            for (int i = 0; i < companionEffects.Count; i++)
            {
                companionEffects[i].StopImmediate();
            }
            companionEffects.Clear();
        }
    }

    public void ApplyOnRenderers(System.Func<Renderer, bool> callback)
    {
        if (IsLoaded)
        {
            ApplyOnRenderersInternal(callback);
        }
        else
        {
            applyOnRenderersCallback = callback;
        }
    }

    private void ApplyOnRenderersInternal(System.Func<Renderer, bool> callback)
    {
        if (modelGo == null)
            return;

        if (renderers == null)
        {
            renderers = modelGo.GetComponentsInChildren<Renderer>();
        }

        if (renderers.Length == 0)
            return;

        for (int i = 0; i < renderers.Length; i++)
        {
            var next = callback(renderers[i]);
            if (!next)
                break;
        }
    }

    public void Destroy()
    {
        ResetState();

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

    private void ResetState()
    {
        if (state == EffectStates.Initial)
            return;

        stateElapsed = 0;
        state = EffectStates.Initial;
        Hide();
        StopAudio();
    }

    private void MoveToNextState()
    {
        EffectStates curState = this.state;
        this.stateElapsed = 0;

        switch (curState)
        {
            case EffectStates.Initial:
                if (config.Delay > 0)
                {
                    this.state = EffectStates.Delay;
                }
                else
                {
                    this.state = EffectStates.Playing;
                    Show();
                    OnBeginPlay();
                    PlayAudio();
                }

                break;
            case EffectStates.Delay:
                this.state = EffectStates.Playing;
                Show();
                OnBeginPlay();
                PlayAudio();

                break;
            case EffectStates.Playing:
                if (config.FadeOutTime > 0)
                {
                    this.state = EffectStates.FadeOut;
                }
                else
                {
                    StopImmediate();
                }

                OnEndPlay();
                StopAudio();
                BeginFadeOut();

                break;
            case EffectStates.FadeOut:
                StopImmediate();

                break;
        }
    }

    private void UpdateState()
    {
        stateElapsed += (int)(Time.deltaTime * 1000);

        switch (state)
        {
            case EffectStates.Initial:
                MoveToNextState();
                break;

            case EffectStates.Delay:
                if (stateElapsed > config.Delay)
                    MoveToNextState();
                break;

            case EffectStates.Playing:
                if (config.Lifetime > 0 && stateElapsed > config.Lifetime)
                    MoveToNextState();
                break;

            case EffectStates.FadeOut:
                if (stateElapsed > config.FadeOutTime)
                    MoveToNextState();
                break;
        }

        if (!target.IsValid())
        {
            Stop();
            return;
        }
    }

    private void Show()
    {
        if (fadeout != null)
        {
            fadeout.EnableFadeOut();
        }

        LayerManager.SetLayer(renderers,activelayer);

        if (config.Priority != (int)EffectPriority.High)
            EffectManager.AddParticleSystems(particleSystems);
    }

    private void Hide()
    {
        LayerManager.SetLayer(renderers, LayerManager.InvisibleLayer);

        if (config.Priority != (int)EffectPriority.High)
            EffectManager.RemoveParticleSystem(particleSystems);
    }

    private void PlayAudio()
    {
        if (audioObject == null && config.Audio != 0)
        {
            if (target.hasTransform && target.bpTrans != null)
            {
                audioObject = AudioManager.PlayAudio(config.Audio, target.bpTrans);
            }
            else
            {
                audioObject = AudioManager.PlayAudio(config.Audio, target.position);
            }
        }
    }

    private void StopAudio()
    {
        if (audioObject != null)
        {
            if (audioObject.IsLoop)
            {
                audioObject.Stop();
            }

            audioObject = null;
        }
    }

    private void OnBeginPlay()
    {
        if (config.OnBeginPlayArray != null)
        {
            for (int i = 0; i < config.OnBeginPlayArray.Length; i++)
            {
                int effID = config.OnBeginPlayArray[i];

                PlayEffectOnSameTarget(effID);
            }
        }
    }

    private void OnEndPlay()
    {
        if (config.OnEndPlayArray != null)
        {
            for (int i = 0; i < config.OnEndPlayArray.Length; i++)
            {
                int effID = config.OnEndPlayArray[i];

                PlayEffectOnSameTarget(effID);
            }
        }
    }

    private void BeginFadeOut()
    {
        if (fadeout != null)
        {
            fadeout.BeginFadeOut();
        }
    }

    private void PlayEffectOnSameTarget(int effID)
    {
        if (companionEffects == null)
            companionEffects = new List<EffectObject>(10);

        EffectObject companionEffect = null;

        if (target.hasTransform && target.root != null)
        {
            companionEffect = EffectManager.Play(effID, target.root);
        }
        else
        {
            companionEffect = EffectManager.Play(effID, target.position, target.rotation);
        }

        if (companionEffect != null && companionEffect.IsLoop)
        {
            companionEffects.Add(companionEffect);
        }
    }

    public void Update()
    {
        if (modelGo == null)
        {
            CheckLoading();
            return;
        }

        if (onLoadCallback != null)
        {
            if (modelGo != null)
            {
                onLoadCallback(this);
                onLoadCallback = null;
            }
        }

        if (applyOnRenderersCallback != null)
        {
            ApplyOnRenderersInternal(applyOnRenderersCallback);
            applyOnRenderersCallback = null;
        }

        UpdateState();
    }

    public void LateUpdate()
    {
        if (state == EffectStates.Expired)
            return;

        if (config.FollowPosition)
            UpdatePosition();

        if (config.FollowRotation)
            UpdateRotation();

        if (config.FollowScale)
            UpdateScale();

        UpdateVisiblilty();
    }

    void CheckLoading()
    {
        if (assetLoadOperation != null)
        {
            if (assetLoadOperation.IsDone())
            {
                GameObject prefab = assetLoadOperation.GetAsset<GameObject>();
                if(prefab == null)
                {
                    assetLoadOperation.Unload();
                    assetLoadOperation = null;
                    StopImmediate();
                }else
                {
                    InstantiateModel(prefab);
                }
            }
        }
    }

    private void UpdatePosition()
    {
        if (transform == null)
        {
            DebugLogger.LogFormat("[EffectObject]:UpdatePosition() error config id: {0}", config.ID);
            return;
        }
        transform.position = target.TransformPoint(config.LocalPositionVec3);
    }

    private void UpdateRotation()
    {
        transform.rotation = target.TransformRotation(config.LocalRotationQuaternion);
    }

    private void UpdateScale()
    {
        transform.localScale = target.TransformScale(config.LocalScaleVec3);
    }

    private void UpdateVisiblilty()
    {
        if (state == EffectStates.Playing || state == EffectStates.FadeOut)
        {
            if (target.hasTransform && target.bpTrans != null && !target.bpTrans.gameObject.activeInHierarchy)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    private void InstantiateModel(GameObject prefab)
    {
        modelGo = Object.Instantiate<GameObject>(prefab);
        modelGo.SetActive(true);
        modelTrans = modelGo.transform;
        modelTrans.SetParent(transform);
        modelTrans.ResetPRS();

        if (renderers == null)
        {
            renderers = modelGo.GetComponentsInChildren<Renderer>();
        }

        fadeout = modelGo.GetComponent<EffectFadeOut>();
        qualitySwitcher = modelGo.GetComponent<EffectQualitySwitcher>();
        particleSystems = modelGo.GetComponentsInChildren<ParticleSystem>();

        if (qualitySwitcher != null)
            qualitySwitcher.SetQuality(runtimeQuality);
    }
}


