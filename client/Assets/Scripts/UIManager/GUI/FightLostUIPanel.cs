using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using FairyGUI;
using System;

public class FightLostUIPanel : GUIBase
{
	protected override void OnStart()
	{
		base.OnStart();

		SetEventListener("exit_btn", EventListenerType.onClick, OnLeveFbGame);
	}

	private void OnLeveFbGame(EventContext context)
	{
		Account account = KBEngine.KBEngineApp.app.player() as Account;
		if (account == null)
			return;

		account.baseCall("ExitFBScene", 0);
	}
}
