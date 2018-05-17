using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBEngine
{
	public class Chess : ChessBase
	{
		public override void __init__()
		{
			base.__init__();

			Debug.Log("============= chess init.");
		}
	}
}

