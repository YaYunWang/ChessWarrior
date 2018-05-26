using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessXiang : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		// 自己的相
		if (chessObj.chess_owner_player == 1)
		{
			if(!hasEntity(index_x + 1, index_x - 1))
				SetIndexPath(index_x + 2, index_z - 2);

			if(!hasEntity(index_x - 1, index_x - 1))
				SetIndexPath(index_x - 2, index_z - 2);

			if (index_z + 2 <= 4)
			{
				if(!hasEntity(index_x + 1, index_x + 1))
					SetIndexPath(index_x + 2, index_z + 2);
				if(!hasEntity(index_x - 1, index_x + 1))
					SetIndexPath(index_x - 2, index_z + 2);
			}
		}
		else
		{
			if(!hasEntity(index_x + 1, index_x + 1))
				SetIndexPath(index_x + 2, index_z + 2);
			if(!hasEntity(index_x - 1, index_x + 1))
				SetIndexPath(index_x - 2, index_z + 2);

			if(index_z - 2 <= 5)
			{
				if(!hasEntity(index_x + 1, index_x - 1))
					SetIndexPath(index_x + 2, index_z - 2);
				if(!hasEntity(index_x - 1, index_x - 1))
					SetIndexPath(index_x - 2, index_z - 2);
			}
		}
	}

	public override ChessType GetChessType()
	{
		return ChessType.ChessTypeXiang;
	}
}
