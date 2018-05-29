using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using KBEngine;

public class PiPeiUIPanel : GUIBase
{
	protected override void OnStart()
	{
		base.OnStart();

        SetEventListener("cancel_btn", EventListenerType.onClick, OnCancelBtn);
	}

	private void OnCancelBtn(EventContext context)
	{
		Account account = KBEngine.KBEngineApp.app.player() as Account;
		if (account == null)
			return;

		account.baseCall("UnStartMatch");

		GUIManager.Destroy(this);
	}
}
