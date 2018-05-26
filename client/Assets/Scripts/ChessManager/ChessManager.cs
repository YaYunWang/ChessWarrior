using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class ChessManager : ManagerTemplateBase<ChessManager>
{
	private static Dictionary<string, System.Type> entityTypeMap = new Dictionary<string, System.Type>();

	private static Dictionary<int, ChessEntity> chessMap = new Dictionary<int, ChessEntity>();
	protected override void InitManager()
	{
		InitTypes();

		KBEngine.Event.registerOut("ChessCreate", this, "ChessCreate");
		KBEngine.Event.registerOut("OnChessMove", this, "OnChessMove");

		GameEventManager.RegisterEvent(GameEventTypes.ExitScene, Clear);
	}

	private void Clear(GameEventTypes eventType, object[] args)
	{
		ClearEntity();
	}

	public void OnChessMove(int chess_id, int index_x, int index_z)
	{
		InputManager.ClearSelectChess();

		ChessEntity chess = chessMap.ContainsKey(chess_id) ? chessMap[chess_id] : null;
		if (chess == null || !chess.Ready)
			return;

		chess.MoveTo(index_x, index_z);
	}

	public static ChessEntity FindEntityByID(int id)
	{
		return chessMap.ContainsKey(id) ? chessMap[id] : null;
	}

	public void ChessCreate(Chess chess)
	{
		GameObject entityGameObject = new GameObject();
		System.Type chessType = GetEntityTypeByConfigID((int)chess.chess_id);
		if (chessType == null)
			return;

		ChessEntity chessEntity = entityGameObject.AddComponent(chessType) as ChessEntity;
		chessEntity.InitChess(chess);
	}

	public ChessEntity FindChessByIndex(int index_x, int index_z)
	{
		foreach (ChessEntity entity in chessMap.Values)
		{
			if ((int)entity.chessObj.chess_index_x == index_x && (int)entity.chessObj.chess_index_z == index_z)
				return entity;
		}

		return null;
	}

	public ChessEntity FindChessByAvatarModel(GameObject avatar)
	{
		foreach (ChessEntity entity in chessMap.Values)
		{
			if (entity.avatarModel.gameObject == avatar)
				return entity;
		}

		return null;
	}

	public static void SetUnCanAttackClick()
	{
		foreach (ChessEntity entity in chessMap.Values)
		{
			entity.SetUnCanAttackClick();
		}
	}

	public static void AddEntity(ChessEntity entity)
	{
#if UNITY_EDITOR
		if (chessMap.ContainsKey(entity.ID))
		{
			Debug.LogErrorFormat("Add chess {0} already exists", entity.ID);
			Debug.Break();
		}
#endif

		chessMap.Add(entity.ID, entity);
	}

	public static void ClearEntity()
	{
		foreach (var entity in chessMap.Values)
		{
			entity.OnRemove();
		}
		chessMap.Clear();
	}

	public static void RemoveEntity(int objID)
	{
#if UNITY_EDITOR
		if (!chessMap.ContainsKey(objID))
		{
			Debug.LogErrorFormat("Remote entity {0} but does not exist.", objID);
			Debug.Break();
		}
#endif

		ChessEntity entity = null;
		if (chessMap.TryGetValue(objID, out entity))
		{
			entity.OnRemove();
			chessMap.Remove(entity.ID);
		}
	}

	private static void InitTypes()
	{
		var types = typeof(ChessEntity).Assembly.GetTypes();

		for (int i = 0; i < types.Length; i++)
		{
			var type = types[i];
			if (type.IsSubclassOf(typeof(ChessEntity)))
			{
				entityTypeMap.Add(type.ToString(), type);
			}
		}
	}

	public static System.Type GetEntityTypeByConfigID(int configID)
	{
		ChessCfg cfg = ConfigManager.Get<ChessCfgLoader>().GetConfig(configID);
		if (cfg == null)
			return typeof(ChessEntity);

		string script = cfg.Script;
		if (string.IsNullOrEmpty(script))
			return typeof(ChessEntity);

		if (!entityTypeMap.ContainsKey(script))
			return typeof(ChessEntity);

		return entityTypeMap[script];
	}
}
