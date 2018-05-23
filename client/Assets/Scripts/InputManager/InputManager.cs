using KBEngine;
using UnityEngine;

public class InputManager : ManagerTemplateBase<InputManager>
{
	public static bool CanMove = false;

	private static ChessEntity SelectChess = null;
	protected override void InitManager()
    {
    }

	public static void ClearSelectChess()
	{
		if (SelectChess != null)
			SelectChess.UnSelect();

		SelectChess = null;
	}

    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.B))
        {
			Account account = KBEngine.KBEngineApp.app.player() as Account;
			if (account == null)
				return;

			account.baseCall("TestChessEntity", 1);
        }
#endif

		if(Input.GetMouseButtonDown(0))
		{
			if (!CanMove)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				GameObject hitGameObject = hit.collider.gameObject;

				ChessEntity entity = ChessManager.Instance.FindChessByAvatarModel(hitGameObject);
				if(entity != null)
				{
					if(SelectChess != null && entity.chessObj.chess_owner_player == 0)
					{
						// 攻击当前选中的棋子
					}
					else
					{
						// 显示当前棋子可行走路线
						ClearSelectChess();

						SelectChess = entity;
						SelectChess.BeSelect();
					}
				}
				else if(SelectChess != null)
				{
					if(hitGameObject.name.Equals("ChessPath"))
					{
						int index_x, index_z = 0;
						if(ChessPathManager.GetChessPathIndex(hitGameObject, out index_x, out index_z))
						{
							// 走到这个格子去
						}
					}
				}
				else
				{
					ClearSelectChess();
				}
			}
		}
    }
}
