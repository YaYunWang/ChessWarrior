using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBEngine
{
	public class Account : AccountBase
	{
		public override void __init__()
		{
			base.__init__();

			KBEngine.Event.fireOut("Account", this);
		}

		public override void ReNameResult(short arg1)
		{
			KBEngine.Event.fireOut("AccountReName", (int)arg1);
		}

		public override void EntryFB()
		{
			KBEngine.Event.fireOut("EntryFb");
		}

		public override void OnStartRound(short arg1, int arg2)
		{
			KBEngine.Event.fireOut("OnStartRound", (int)arg1, (int)arg2);
		}

		public override void OnMove(int arg1, int arg2, int arg3)
		{
			KBEngine.Event.fireOut("OnChessMove", (int)arg1, (int)arg2, (int)arg3);
		}

		public override void OnAttack(int arg1, int arg2)
		{
			KBEngine.Event.fireOut("AttackChess", arg1, arg2);
		}
	}
}