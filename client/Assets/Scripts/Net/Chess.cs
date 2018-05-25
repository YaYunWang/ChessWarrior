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

			KBEngine.Event.fireOut("ChessCreate", this);
		}

		public override void onChess_index_xChanged(ulong oldValue)
		{
			base.onChess_index_xChanged(oldValue);
		}

		public override void onChess_index_zChanged(ulong oldValue)
		{
			base.onChess_index_zChanged(oldValue);
		}
	}
}

