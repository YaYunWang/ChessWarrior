using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessJiang : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		// 自己的将
		if (chessObj.chess_owner_player == 1)
		{
			// 左
			if(index_x > 3)
			{
				SetIndexPath(index_x - 1, index_z);
			}
			// 右
			if(index_x < 5)
			{
				SetIndexPath(index_x + 1, index_z);
			}
			// 上
			if(index_z < 2)
			{
				SetIndexPath(index_x, index_z + 1);
			}
			// 下
			SetIndexPath(index_x, index_z - 1);
		}
		else
		{
		}
	}
}
