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
	}

	private void OnEntryGame(EventContext context)
	{
		Debug.Log("entry game buttom click.");
		Account account = KBEngine.KBEngineApp.app.player() as Account;
		if (account == null)
			return;

		account.baseCall("EntryFBScene");
	}
}
