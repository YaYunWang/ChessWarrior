using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class CityUIPanel : GUIBase
{
	protected override void OnStart()
	{
		base.OnStart();

		Account account = KBEngine.KBEngineApp.app.player() as Account;
		SetText("role_name", account.RoleName);
	}
}
