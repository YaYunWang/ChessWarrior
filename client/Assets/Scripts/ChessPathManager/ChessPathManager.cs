﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPathManager : ManagerTemplateBase<ChessPathManager>
{
	public static List<List<GameObject>> points = new List<List<GameObject>>();

	protected override void InitManager()
	{
		GameEventManager.RegisterEvent(GameEventTypes.ExitScene, Clear);
	}

	private void Clear(GameEventTypes eventType, object[] args)
	{
		points.Clear();
	}

	public static void CreatePathPoint()
	{
		GameObject chessPaht = Resources.Load<GameObject>("ChessPath");

		for (int idx = 0; idx <= 8; idx++)
		{
			List<GameObject> list = new List<GameObject>();
			for (int idz = 0; idz <= 9; idz++)
			{
				GameObject go = GameObject.Instantiate<GameObject>(chessPaht);
				go.SetActive(false);

				go.transform.position = new Vector3(idx * ChessEntity.ChessInterval, 0, idz * ChessEntity.ChessInterval);
				go.transform.localScale = Vector3.one * 2;
				go.name = "ChessPath";

				list.Add(go);
			}

			points.Add(list);
		}
	}

	public static void SetPathIndex(int index_x, int index_z)
	{
		points[index_x][index_z].SetActive(true);
	}

	public static void HideAllPath()
	{
		for (int idx = 0; idx <= 8; idx++)
		{
			for (int idz = 0; idz <= 9; idz++)
			{
				points[idx][idz].SetActive(false);
			}
		}

		ChessManager.SetUnCanAttackClick();
	}

	public static bool GetChessPathIndex(GameObject go, out int index_x, out int index_z)
	{
		index_x = 0;
		index_z = 0;
		for (int idx = 0; idx <= 8; idx++)
		{
			for (int idz = 0; idz <= 9; idz++)
			{
				if (points[idx][idz] == go)
				{
					index_x = idx;
					index_z = idz;

					return true;
				}
			}
		}

		return false;
	}
}
