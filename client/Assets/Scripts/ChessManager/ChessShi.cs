using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessShi : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		// 自己的士
		if (chessObj.chess_owner_player == 1)
		{
			if(index_x == 4)
			{
				SetIndexPath(3, 0);
				SetIndexPath(3, 2);
				SetIndexPath(5, 0);
				SetIndexPath(5, 2);
			}

			if(index_x == 3 || index_x == 5)
			{
				SetIndexPath(4, 1);
			}
		}
		else
		{
			if (index_x == 4)
			{
				SetIndexPath(3, 9);
				SetIndexPath(3, 7);
				SetIndexPath(5, 9);
				SetIndexPath(5, 7);
			}

			if (index_x == 3 || index_x == 5)
			{
				SetIndexPath(4, 8);
			}
		}
	}
}
