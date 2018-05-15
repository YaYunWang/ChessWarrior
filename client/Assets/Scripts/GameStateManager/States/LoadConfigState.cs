using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadConfigState : IState
{
	public void Enter()
	{
		DebugLogger.Log("enter load config state...");

		ConfigManager.CreateInstance();
	}

	public void Exit()
	{
	}

	public void Update()
	{
		if (!ConfigManager.isDone)
			return;

		GameStateManager.ChangeState(StateEnum.LOGIN);
	}
}
