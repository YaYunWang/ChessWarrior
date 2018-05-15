using System.Collections;
using UnityEngine;

public class SceneManager : ManagerTemplateBase<SceneManager>
{
    public static float Progress { protected set; get; }

    static SceneCfgLoader sceneCfgLoader;

    public static Camera SceneCamera { protected set; get; }

    public static SceneCfg LastSceneCfg { protected set; get; }

    public static Transform SceneCameraTrans { protected set; get; }
    
    protected override void InitManager()
    {
        sceneCfgLoader = ConfigManager.Get<SceneCfgLoader>();
    }

    public static void ChangeScene(int sceneId)
    {
        Instance.StartCoroutine(Instance.ChangeToEmptyScene(sceneId));
    }

    /// <summary>
    /// 这个切场景只能是服务器主动调用，不可以客户端主动调用
    /// </summary>
    /// <param name="sceneId"></param>
    public static void ChangeToServerScene(int sceneId)
    {
        Instance.StartCoroutine(Instance.ChangeToEmptyScene(sceneId,true));
    }

    private IEnumerator ChangeToEmptyScene(int sceneId, bool isServerScene = false)
    {
        AssetLoadManager.OnDispose();

        UnityEngine.SceneManagement.SceneManager.LoadScene("empty");

        UnLoadLastSceneAsset();

        SceneCamera = null;
        
        yield return ChangeSceneInternal(sceneId);
    }

    private IEnumerator ChangeSceneInternal(int sceneId)
    {
        var config = sceneCfgLoader.GetConfig(sceneId);

        LastSceneCfg = config;

        string sceneName = "";

        sceneName = config.AssetPath;
        string sceneBundleName = string.Format("scene/scene_{0}.bundle", sceneName.ToLower());

        var m_operation = AssetLoadManager.LoadLevelAsync(sceneBundleName, sceneName, false);

        Progress = 0f;

        while (!m_operation.IsDone())
        {
            Progress += Time.deltaTime * 0.2f;
            Progress = Mathf.Min(Progress, 1f);

            yield return null;
        }

        Progress = 1;

        SceneCamera = Camera.main;

        SceneCameraTrans = SceneCamera.transform;

        GameEventManager.EnableEventFiring = true;

        CloseMainCameraUILayer();

        if (config.BGM > 0)
            PlayBGM(config.BGM);

        GameStateManager.ChangeState(StateEnum.WORLD);
    }
    
    private static void PlayBGM(int audioID)
    {
        if (SceneCamera == null)
            return;
        var mainCamera = SceneCamera.transform;
        AudioManager.PlayAudio(audioID, mainCamera);
    }

    private static void CloseMainCameraUILayer()
    {
        if (SceneCamera == null)
            return;
        int uiLayer = LayerMask.NameToLayer("UI");
        SceneCamera.cullingMask &= ~(1 << uiLayer);
    }

    private void UnLoadLastSceneAsset()
    {
        if (LastSceneCfg != null)
        {
            string sceneBundleName = string.Format("scene/scene_{0}.bundle", LastSceneCfg.AssetPath.ToLower());
            AssetLoadManager.UnLoadAssetBundle(sceneBundleName);
        }
    }
}


