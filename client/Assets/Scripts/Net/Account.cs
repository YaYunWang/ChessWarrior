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

		//public override void ReNameResult(KBEngine.INT16 arg1)
		//{
		//	KBEngine.Event.fireOut("AccountReName", arg1);
		//}
		public override void ReNameResult(short arg1)
		{
			KBEngine.Event.fireOut("AccountReName", (int)arg1);
		}
	}
}