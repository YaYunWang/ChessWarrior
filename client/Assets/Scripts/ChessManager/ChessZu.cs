using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessZu : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;
		
		// 自己的卒
		if (chessObj.chess_owner_player == 1)
		{
			if(index_z < 9)
			{
				SetIndexPath(index_x, index_z + 1);
			}

			if (index_z >= 5)
			{
				SetIndexPath(index_x - 1, index_z);
				SetIndexPath(index_x + 1, index_z);
			}
		}
		else
		{
			if (index_z > 0)
			{
				SetIndexPath(index_x, index_z - 1);
			}

			if (index_z <= 4)
			{
				SetIndexPath(index_x - 1, index_z);
				SetIndexPath(index_x + 1, index_z);
			}
		}
	}
}
