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
}
