using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using FairyGUI;
using System;

public class CityUIPanel : GUIBase
{
	protected override void OnStart()
	{
		base.OnStart();

		Account account = KBEngine.KBEngineApp.app.player() as Account;
		SetText("role_name", account.RoleName);

        SetEventListener("entry_game", EventListenerType.onClick, OnEntryGame);
        SetEventListener("pipei_game", EventListenerType.onClick, OnPiPeiGame);
	}

	private void OnPiPeiGame(EventContext context)
	{
		Account account = KBEngine.KBEngineApp.app.player() as Account;
		if (account == null)
			return;

		account.baseCall("StartMatch");

		GUIManager.Open<PiPeiUIPanel>("City", "PiPeiUIPanel");
	}

	private void OnEntryGame(EventContext context)
	{
		Account account = KBEngine.KBEngineApp.app.player() as Account;
		if (account == null)
			return;

		account.baseCall("EntryFBScene");
	}
}
