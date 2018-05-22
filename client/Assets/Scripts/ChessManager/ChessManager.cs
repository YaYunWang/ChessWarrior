using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class ChessManager : ManagerTemplateBase<ChessManager>
{
	private static Dictionary<int, ChessEntity> chessMap = new Dictionary<int, ChessEntity>();
	protected override void InitManager()
	{
		KBEngine.Event.registerOut("ChessCreate", this, "ChessCreate");

		GameEventManager.RegisterEvent(GameEventTypes.ExitScene, Clear);
	}

	private void Clear(GameEventTypes eventType, object[] args)
	{
		ClearEntity();
	}

	public void ChessCreate(Chess chess)
	{
		GameObject entityGameObject = new GameObject();
		ChessEntity chessEntity = entityGameObject.AddComponent<ChessEntity>();
		chessEntity.InitChess(chess);
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
}
