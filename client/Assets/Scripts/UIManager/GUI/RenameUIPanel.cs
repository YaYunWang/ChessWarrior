using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using KBEngine;

public class RenameUIPanel : GUIBase
{
	protected override void OnStart()
	{
		SetEventListener("ok_btn", EventListenerType.onClick, OnRenameClick);
	}

	private void OnRenameClick(EventContext ec)
	{
		string name = GetTextInput("name_input");

		if(string.IsNullOrEmpty(name))
		{
			DebugLogger.LogError("名字为空，起名失败.");

			return;
		}
		Debug.Log("rename :" + name);

		Account account = KBEngine.KBEngineApp.app.player() as Account;

		account.baseCall("ReCreateAccountRequest", 1, name);
	}
}
