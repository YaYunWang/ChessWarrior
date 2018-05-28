using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using FairyGUI;

public partial class ChessEntity : MonoBehaviour
{
	public static float ChessInterval = 5;

	public bool Ready = false;

	public Chess chessObj = null;
	public ChessCfg chessCfg = null;

	private List<AssetBundleLoadAssetOperation> loadOps = new List<AssetBundleLoadAssetOperation>();
    private Dictionary<string, Transform> bpoints = new Dictionary<string, Transform>();
	public Transform avatarModel;
	private Renderer[] Renderers;

	protected Animator animator;

	public ChessEntity Target;

	public int ID
	{
		get
		{
			return chessObj.id;
		}
	}

	public bool IsDead
	{
		get; private set;
	}

	public int HP
	{
		get;
		set;
	}

	public bool CanAttackClick
	{
		get;
		set;
	}

	public void InitChess(Chess chess)
	{
		chessObj = chess;
		chessCfg = ConfigManager.Get<ChessCfgLoader>().GetConfig((int)chessObj.chess_id);

		if (chessCfg == null)
			return;

		this.gameObject.name = string.Format("{0} -- {1}", chess.chess_id, chess.id);

		Ready = false;

		this.transform.localPosition = new Vector3(ChessInterval * chess.chess_index_x, 0, ChessInterval * chess.chess_index_z);
		this.transform.localScale = Vector3.one * 2;

		HP = (int)chessObj.max_hp;

		StartCoroutine(InitChessInternale());

		GUIManager.GetView<ChessInfoUIPanel>("ChessInfoUIPanel").ShowChessInfo(this);
	}
	
	private void OnReady()
	{
		Ready = true;

		ChessManager.AddEntity(this);
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

		GUIManager.GetView<ChessInfoUIPanel>("ChessInfoUIPanel").RemoveChessInfo(this);

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

	protected void SetIndexPath(int index_x, int index_z)
	{
		if (index_x < 0 || index_x > 8 || index_z < 0 || index_z > 9)
			return;

		ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, index_z);
		if (entity == this)
			return;

		if (entity != null)
		{
			if(entity.chessObj.chess_owner_player != chessObj.chess_owner_player)
				entity.SetCanAttackClick();
		}
		else
		{
			ChessPathManager.SetPathIndex(index_x, index_z);
		}
	}

	public void SetCanAttackClick()
	{
		CanAttackClick = true;
		MaterialsColor(Color.red);
	}

	public void SetUnCanAttackClick()
	{
		CanAttackClick = false;
		RevertMaterialsColor();
	}

	public void BeAttack(int damage)
	{
		if (IsDead)
			return;

		PlayAction("Wound");

		HP -= damage;
		if(HP <= 0)
		{
			HP = 0;
			IsDead = true;

			StartCoroutine(OnDeath());
		}
	}

	private IEnumerator OnDeath()
	{
		PlayAction("Death");
		yield return new WaitForSeconds(5);

		ChessManager.RemoveEntity(chessObj.id);
	}

	protected bool hasEntity(int index_x, int index_z)
	{
		ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, index_z);

		return entity != null;
	}
}
