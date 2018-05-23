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

		KBEngine.Event.registerOut("OnStartRound", this, "OnStartRound");

		ChessPathManager.CreatePathPoint();
	}

	public void Exit()
	{
	}

	public void Update()
	{
	}

	public void OnStartRound(int type, int time)
	{
		if(type == 1)
		{
			Debug.Log("自己回合，开始走棋。");
			InputManager.CanMove = true;
		}
		else
		{
			InputManager.CanMove = false;
			InputManager.ClearSelectChess();
			Debug.Log("敌人回合，暂时不做处理。");
		}
	}
}
