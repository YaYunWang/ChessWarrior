using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChessEntity
{
	public virtual void BeSelect()
	{
		transform.localScale = Vector3.one * 2;

		ChessPathManager.HideAllPath();
		ShowChessPath();
	}

	public virtual void UnSelect()
	{
		transform.localScale = Vector3.one;
		ChessPathManager.HideAllPath();
	}

	public virtual void ShowChessPath()
	{
	}

	public virtual void MoveTo(int index_x, int index_z)
	{
		this.transform.localPosition = new Vector3(ChessInterval * index_x, 0, ChessInterval * index_z);
	}
}
