using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;

public class LoginState : IState
{
	public const string PlayerPrefs_Account = "WYY_Account";
	public const string PlayerPrefs_Password = "WYY_Password";

	public void Enter()
	{
		DebugLogger.Log("entry login state.");

		GUIManager.Open<LoginUIPanel>("Login", "LoginUIPanel");

		KBEngine.Event.registerOut("Account", this, "OnAccountCreate");
		KBEngine.Event.registerOut("AccountReName", this, "AccountReName");

		//// 测试，客户端测试显示棋子
		//Chess chess = new Chess();
		//chess.id = 1;
		//chess.chess_id = 1;

		//chess.__init__();
	}

	public void Exit()
	{
		GUIManager.DestroyAll();

		KBEngine.Event.deregisterOut("Account", this, "OnAccountCreate");
		KBEngine.Event.deregisterOut("AccountReName", this, "AccountReName");
	}

	public void Update()
	{
	}

	public void AccountReName(int arg)
	{
		Debug.Log("rename change.");

		Account account = KBEngine.KBEngineApp.app.player() as Account;

		//GameStateManager.ChangeState(StateEnum.WORLD);
		EntryCity();
	}

	public void OnAccountCreate(Account account)
	{
		Debug.Log("account create." + account.RoleName);
		if (account.RoleType <= 0)
		{
			GUIManager.Destroy("LoginUIPanel");

			GUIManager.Open<RenameUIPanel>("ReName", "RenameUIPanel");
		}
		else
		{
			//GameStateManager.ChangeState(StateEnum.WORLD);
			EntryCity();
		}
	}

	private void EntryCity()
	{
		SceneManager.ChangeScene(1);
	}
}
