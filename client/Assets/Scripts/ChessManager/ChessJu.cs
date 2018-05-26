using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessJu : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		bool tempChess = false;

		for(int idx = index_x; idx <= 8; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if(entity != null)
			{
				if (entity == this)
					continue;

				if (entity.chessObj.chess_owner_player == chessObj.chess_owner_player)          // 自己人
				{
					break;
				}
				else
				{
					if (tempChess)
						break;
					tempChess = true;
				}
			}
			
			SetIndexPath(idx, index_z);
		}

		for (int idx = index_x; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if (entity != null)
			{
				if (entity == this)
					continue;

				if (entity.chessObj.chess_owner_player == chessObj.chess_owner_player)          // 自己人
				{
					break;
				}
				else
				{
					if (tempChess)
						break;
					tempChess = true;
				}
			}

			SetIndexPath(idx, index_z);
		}

		for (int idx = index_z; idx <= 9; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity != null)
			{
				if (entity == this)
					continue;

				if (entity.chessObj.chess_owner_player == chessObj.chess_owner_player)          // 自己人
				{
					break;
				}
				else
				{
					if (tempChess)
						break;
					tempChess = true;
				}
			}

			SetIndexPath(index_x, idx);
		}

		for (int idx = index_z; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity != null)
			{
				if (entity == this)
					continue;

				if (entity.chessObj.chess_owner_player == chessObj.chess_owner_player)          // 自己人
				{
					break;
				}
				else
				{
					if (tempChess)
						break;
					tempChess = true;
				}
			}

			SetIndexPath(index_x, idx);
		}
	}

	public override ChessType GetChessType()
	{
		return ChessType.ChessTypeJu;
	}
}
