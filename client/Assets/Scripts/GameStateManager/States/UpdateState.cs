using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateState : IState
{
	public void Enter()
	{
		DebugLogger.Log("enter update state...");

		GameStateManager.ChangeState(StateEnum.LOADCONFIG);
	}

	public void Exit()
	{
	}

	public void Update()
	{
	}
}
