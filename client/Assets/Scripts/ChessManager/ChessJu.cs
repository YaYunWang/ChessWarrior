using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessJu : ChessEntity
{
	public override void ShowChessPath()
	{
		base.ShowChessPath();

		List<List<GameObject>> list = ChessPathManager.points;

		int index_x = (int)chessObj.chess_index_x;
		int index_z = (int)chessObj.chess_index_z;

		for(int idx = index_x; idx <= 8; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if (entity == this)
				continue;
			if (entity != null)
				break;

			list[idx][index_z].SetActive(true);
		}

		for (int idx = index_x; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(idx, index_z);
			if (entity == this)
				continue;
			if (entity != null)
				break;

			list[idx][index_z].SetActive(true);
		}

		for (int idx = index_z; idx <= 9; idx++)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity == this)
				continue;
			if (entity != null)
				break;

			list[index_x][idx].SetActive(true);
		}

		for (int idx = index_z; idx >= 0; idx--)
		{
			ChessEntity entity = ChessManager.Instance.FindChessByIndex(index_x, idx);
			if (entity == this)
				continue;
			if (entity != null)
				break;

			list[index_x][idx].SetActive(true);
		}
	}
}
