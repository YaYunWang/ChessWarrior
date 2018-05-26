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

		bool tempChess = false;

		for (int idx = index_x; idx <= 8; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if (entity == this)
				continue;
			if(entity != null)
			{
				if(!tempChess)
				{
					tempChess = true;
				}
				else
				{
					SetIndexPath(idx, index_z);
					break;
				}
			}
			else
			{
				if(!tempChess)
					SetIndexPath(idx, index_z);
			}
		}

		tempChess = false;
		for (int idx = index_x; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if (entity == this)
				continue;
			if (entity != null)
			{
				if (!tempChess)
				{
					tempChess = true;
				}
				else
				{
					SetIndexPath(idx, index_z);
					break;
				}
			}
			else
			{
				if (!tempChess)
					SetIndexPath(idx, index_z);
			}
		}

		tempChess = false;
		for (int idx = index_z; idx <= 9; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity == this)
				continue;
			if (entity != null)
			{
				if (!tempChess)
				{
					tempChess = true;
				}
				else
				{
					SetIndexPath(index_x, idx);
					break;
				}
			}
			else
			{
				if (!tempChess)
					SetIndexPath(index_x, idx);
			}
		}

		tempChess = false;
		for (int idx = index_z; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity == this)
				continue;
			if (entity != null)
			{
				if (!tempChess)
				{
					tempChess = true;
				}
				else
				{
					SetIndexPath(index_x, idx);
					break;
				}
			}
			else
			{
				if (!tempChess)
					SetIndexPath(index_x, idx);
			}
		}
	}

	public override ChessType GetChessType()
	{
		return ChessType.ChessTypePao;
	}
}
