using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class InputManager : ManagerTemplateBase<InputManager>
{
	protected override void InitManager()
    {
    }

    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.B))
        {
			Account account = KBEngine.KBEngineApp.app.player() as Account;
			if (account == null)
				return;

			account.baseCall("TestChessEntity", 1);
        }
#endif
    }
}
