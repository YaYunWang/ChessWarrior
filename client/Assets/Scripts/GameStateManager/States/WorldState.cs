using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldState : IState
{
    public void Enter()
	{
		DebugLogger.Log("========================entry world state.");

		GUIManager.Open<CityUIPanel>("City", "CityUIPanel");
	}

    public void Exit()
	{
		GUIManager.CloseAll();
	}

    public void Update()
    {
    }
}


