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
	}

	public void Exit()
	{
		GUIManager.DestroyAll();
	}

	public void Update()
	{
	}

	public void AccountReName(int arg)
	{
		Debug.Log("rename change.");

		Account account = KBEngine.KBEngineApp.app.player() as Account;

		GameStateManager.ChangeState(StateEnum.WORLD);// ��ʼ��Ϸ
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
			GameStateManager.ChangeState(StateEnum.WORLD);// ��ʼ��Ϸ
		}
	}
}
