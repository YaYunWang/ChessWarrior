using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPao : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		for (int idx = index_x; idx <= 8; idx++)
		{
			SetIndexPath(idx, index_z);
		}

		for (int idx = index_x; idx >= 0; idx--)
		{
			SetIndexPath(idx, index_z);
		}

		for (int idx = index_z; idx <= 9; idx++)
		{
			SetIndexPath(index_x, idx);
		}

		for (int idx = index_z; idx >= 0; idx--)
		{
			SetIndexPath(index_x, idx);
		}
	}
}
