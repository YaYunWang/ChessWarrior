using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public partial class ChessEntity : MonoBehaviour
{
	public bool Ready = false;

	private Chess chessObj = null;
	private ChessCfg chessCfg = null;

	private List<AssetBundleLoadAssetOperation> loadOps = new List<AssetBundleLoadAssetOperation>();
    private Dictionary<string, Transform> bpoints = new Dictionary<string, Transform>();
	protected Transform avatarModel;
	private Renderer[] Renderers;

	protected Animator animator;

	public int ID
	{
		get
		{
			return chessObj.id;
		}
	}

	public void InitChess(Chess chess)
	{
		chessObj = chess;

		chessCfg = ConfigManager.Get<ChessCfgLoader>().GetConfig((int)chessObj.chess_id);

		this.gameObject.name = string.Format("{0} -- {1}", chess.chess_id, chess.id);

		Ready = false;

		StartCoroutine(InitChessInternale());
	}

	private void OnReady()
	{
		Ready = true;
	}

	private void Update()
	{
		if (!Ready)
			return;
	}

	private void LateUpdate()
	{
	}

	private IEnumerator InitChessInternale()
	{
		yield return StartCoroutine(OnSetupAvatar());

		foreach (var renender in Renderers)
			renender.enabled = false;

		if (avatarModel != null)
		{
			animator = avatarModel.GetComponent<Animator>();
		}

		if (animator == null)
		{
			animator = avatarModel.gameObject.AddComponent<Animator>();
		}

		animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		animator.applyRootMotion = false;
		yield return StartCoroutine(OnSetupController());

		foreach (var renender in Renderers)
			renender.enabled = true;

		OnReady();

		GameEventManager.RaiseEvent(GameEventTypes.EntityCreated, this);
	}

	private IEnumerator OnSetupController()
	{
		string controllerName = chessCfg.Controller;

		if (string.IsNullOrEmpty(controllerName))
		{
			Debug.LogErrorFormat("chess config {0} does not have a controller name", chessObj.id);
			yield break;
		}

		string resPath = string.Format("controller/controller_{0}.bundle", controllerName);

		var request = AssetLoadManager.LoadAssetAsync(resPath, controllerName, typeof(RuntimeAnimatorController));

		AutoUnloadAsset(request);

		yield return request;

		var controller = request.GetAsset<RuntimeAnimatorController>();

		SetAnimatorController(controller);
	}

	private IEnumerator OnSetupAvatar()
	{
		string modelName = chessCfg.Model;

		if (string.IsNullOrEmpty(modelName))
		{
			yield break;
		}

		string resPath = string.Format("model/{0}.bundle", modelName);

		var request = AssetLoadManager.LoadAssetAsync(resPath, modelName, typeof(GameObject));

		AutoUnloadAsset(request);

		yield return request;

		GameObject prefab = request.GetAsset<GameObject>();

		UnityEngine.Profiling.Profiler.BeginSample("EntityInstantiate");
		GameObject model = Instantiate(prefab) as GameObject;
		UnityEngine.Profiling.Profiler.EndSample();

		SetAvatar(model);
	}

	protected void SetAvatar(GameObject model)
	{
		avatarModel = model.transform;
		model.transform.SetParent(transform);
		model.transform.ResetPRS();

		Renderers = null;
		Renderers = avatarModel.GetComponentsInChildren<Renderer>(true);
	}

	private void AutoUnloadAsset(AssetBundleLoadAssetOperation loadOp)
	{
		loadOps.Add(loadOp);
	}

	private void UnloadAssets()
	{
		for (int i = 0; i < loadOps.Count; i++)
		{
			loadOps[i].Unload();
		}

		loadOps.Clear();
	}

	public void OnRemove()
	{
		StartCoroutine(OnRemoveInternal());
	}

	private IEnumerator OnRemoveInternal()
	{
		while (avatarModel == null)
		{
			yield return null;
		}

		UnloadAssets();

		Object.Destroy(gameObject);
	}

	public virtual Transform FindBindPoint(string bpoint)
	{
		// 优化时可以考虑根据模型名，缓存路径，直接按路径查找
		Transform bpTrans = null;
		if (!bpoints.TryGetValue(bpoint, out bpTrans))
		{
			if (Ready && avatarModel != null)
			{
				bpTrans = avatarModel.Search(bpoint);
				bpoints[bpoint] = bpTrans;
			}
		}

		if (bpTrans == null)
		{
			return transform;
		}

		return bpTrans;
	}
}
