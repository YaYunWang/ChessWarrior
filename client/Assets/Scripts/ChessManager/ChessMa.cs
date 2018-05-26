using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMa : ChessEntity
{
	//马，暂时不判断别马腿的情况了 
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		if(!hasEntity(index_x + 1, index_z))
		{
			SetIndexPath(index_x + 2, index_z + 1);
			SetIndexPath(index_x + 2, index_z - 1);
		}

		if(!hasEntity(index_x - 1, index_z))
		{
			SetIndexPath(index_x - 2, index_z + 1);
			SetIndexPath(index_x - 2, index_z - 1);
		}

		if(!hasEntity(index_x,index_z + 1 ))
		{
			SetIndexPath(index_x + 1, index_z + 2);
			SetIndexPath(index_x - 1, index_z + 2);
		}

		if(!hasEntity(index_x, index_z - 1))
		{
			SetIndexPath(index_x - 1, index_z - 2);
			SetIndexPath(index_x + 1, index_z - 2);
		}
	}

	public override ChessType GetChessType()
	{
		return ChessType.ChessTypeMa;
	}
}
