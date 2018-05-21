using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;

public class WorldFightState : IState
{
	public void Enter()
	{
		Debug.Log("==== 进入fb ");
		Account account = KBEngine.KBEngineApp.app.player() as Account;

		account.baseCall("ClientReady");
	}

	public void Exit()
	{
	}

	public void Update()
	{
	}
}
