using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using FairyGUI;
using System;

public class FightMainUIPanel : GUIBase
{
	private GTextField stateInfoText = null;

	private int stateCountTime = 0;
	private float myRoundTime = 0;
	private bool isMyRound = false;

	protected override void OnStart()
	{
		base.OnStart();
		stateInfoText = GetChildren("state_info").asTextField;

		stateInfoText.text = "";
	}

	public void SetStateInfoString(string info)
	{
		stateInfoText.text = info;
		isMyRound = false;
	}

	public void MyRound(int time)
	{
		stateCountTime = time;
		myRoundTime = Time.time;
		isMyRound = true;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		if(isMyRound)
		{
			float lastTime = (int)(stateCountTime - (Time.time - myRoundTime));

			stateInfoText.text = "你的回合：" + lastTime;
		}
	}
}
