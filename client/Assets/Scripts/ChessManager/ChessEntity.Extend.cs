using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChessEntity
{
	public virtual void BeSelect()
	{
		transform.localScale = Vector3.one * 4;

		ChessPathManager.HideAllPath();
		ShowChessPath();

		MaterialsColor(Color.green);
	}

	public virtual void UnSelect()
	{
		transform.localScale = Vector3.one * 2;
		ChessPathManager.HideAllPath();

		RevertMaterialsColor();
	}

	public virtual void ShowChessPath()
	{
	}

	public virtual void MoveTo(int index_x, int index_z)
	{
		this.transform.localPosition = new Vector3(ChessInterval * index_x, 0, ChessInterval * index_z);
	}

	public virtual void MoveTo(Vector3 pos)
	{
		this.transform.localPosition = pos;
	}

	public virtual ChessType GetChessType()
	{
		return ChessType.ChessTypeNone;
	}
}

public enum ChessType
{
	ChessTypeNone,
	ChessTypeJu,
	ChessTypeMa,
	ChessTypeXiang,
	ChessTypeShi,
	ChessTypeJiang,
	ChessTypePao,
	ChessTypeZu
}