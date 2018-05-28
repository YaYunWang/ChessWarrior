using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

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

    public static void ShowChessBoard()
    {
		float dakuangTemp = 0.5f;
		VectorLine dakuangPathLine = new VectorLine("DaKuang", new List<Vector3>(), 6.0f);
        dakuangPathLine.color = Color.black;
        dakuangPathLine.textureScale = 1f;
        dakuangPathLine.points3.Add(new Vector3(0 - dakuangTemp, 1f, 0 - dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8 + dakuangTemp, 1f, 0 - dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8 + dakuangTemp, 1f, 0 - dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8 + dakuangTemp, 1f, ChessEntity.ChessInterval * 9 + dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8 + dakuangTemp, 1f, ChessEntity.ChessInterval * 9 + dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(0 - dakuangTemp, 1f, ChessEntity.ChessInterval * 9 + dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(0 - dakuangTemp, 1f, ChessEntity.ChessInterval * 9 + dakuangTemp));
        dakuangPathLine.points3.Add(new Vector3(0 - dakuangTemp, 1f, 0 - dakuangTemp));
        dakuangPathLine.Draw3DAuto();
		LayerManager.SetLayer(dakuangPathLine.rectTransform.gameObject, LayerManager.DefaultLayer);

		VectorLine xiaokuangPathLine = new VectorLine("XiaoKuang", new List<Vector3>(), 3.0f);
		xiaokuangPathLine.color = Color.black;
		xiaokuangPathLine.textureScale = 1f;
		//xiaokuangPathLine.points3.Add(new Vector3(0, 1f, 0));
		//xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8, 1f, 0));
		//xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8, 1f, 0));
		//xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8, 1f, ChessEntity.ChessInterval * 9));
		//xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8, 1f, ChessEntity.ChessInterval * 9));
		//xiaokuangPathLine.points3.Add(new Vector3(0, 1f, ChessEntity.ChessInterval * 9));
		//xiaokuangPathLine.points3.Add(new Vector3(0, 1f, ChessEntity.ChessInterval * 9));
		//xiaokuangPathLine.points3.Add(new Vector3(0, 1f, 0));

		// 竖
		for(int idx = 0; idx <= 8; idx++)
		{
			if(idx == 0 || idx == 8)
			{
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, 0));
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, ChessEntity.ChessInterval * 9));
			}
			else
			{
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, 0));
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, ChessEntity.ChessInterval * 4));
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, ChessEntity.ChessInterval * 5));
				xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * idx, 1f, ChessEntity.ChessInterval * 9));
			}
		}
		// 横
		for (int idx = 0; idx <= 9; idx++)
		{
			xiaokuangPathLine.points3.Add(new Vector3(0, 1f, ChessEntity.ChessInterval * idx));
			xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 8, 1f, ChessEntity.ChessInterval * idx));
		}
		// 士
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 3, 1f, ChessEntity.ChessInterval * 2));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 5, 1f, 0));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 3, 1f, 0));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 5, 1f, ChessEntity.ChessInterval * 2));

		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 3, 1f, ChessEntity.ChessInterval * 9));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 5, 1f, ChessEntity.ChessInterval * 7));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 3, 1f, ChessEntity.ChessInterval * 7));
		xiaokuangPathLine.points3.Add(new Vector3(ChessEntity.ChessInterval * 5, 1f, ChessEntity.ChessInterval * 9));

		// 
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 1, 1f, ChessEntity.ChessInterval * 2));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 7, 1f, ChessEntity.ChessInterval * 2));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 0, 1f, ChessEntity.ChessInterval * 3));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 2, 1f, ChessEntity.ChessInterval * 3));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 4, 1f, ChessEntity.ChessInterval * 3));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 6, 1f, ChessEntity.ChessInterval * 3));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 8, 1f, ChessEntity.ChessInterval * 3));

		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 1, 1f, ChessEntity.ChessInterval * 7));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 7, 1f, ChessEntity.ChessInterval * 7));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 0, 1f, ChessEntity.ChessInterval * 6));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 2, 1f, ChessEntity.ChessInterval * 6));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 4, 1f, ChessEntity.ChessInterval * 6));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 6, 1f, ChessEntity.ChessInterval * 6));
		DrawXiHuaPoint(xiaokuangPathLine, new Vector3(ChessEntity.ChessInterval * 8, 1f, ChessEntity.ChessInterval * 6));

		xiaokuangPathLine.Draw3DAuto();
		LayerManager.SetLayer(xiaokuangPathLine.rectTransform.gameObject, LayerManager.DefaultLayer);
	}

	private static void DrawXiHuaPoint(VectorLine line, Vector3 v)
	{
		float temp = 1;
		if(v.x > 0)
		{
			// 左上
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z + temp));
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z + temp / 2));
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z + temp / 2));
			line.points3.Add(new Vector3(v.x - temp, 1f, v.z + temp / 2));
			// 左下
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z - temp));
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z - temp / 2));
			line.points3.Add(new Vector3(v.x - temp / 2, 1f, v.z - temp / 2));
			line.points3.Add(new Vector3(v.x - temp, 1f, v.z - temp / 2));
		}

		if(v.x < ChessEntity.ChessInterval * 8)
		{
			// 右上
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z + temp));
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z + temp / 2));
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z + temp / 2));
			line.points3.Add(new Vector3(v.x + temp, 1f, v.z + temp / 2));
			// 右下
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z - temp));
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z - temp / 2));
			line.points3.Add(new Vector3(v.x + temp / 2, 1f, v.z - temp / 2));
			line.points3.Add(new Vector3(v.x + temp, 1f, v.z - temp / 2));
		}
	}

	public static void CreatePathPoint()
	{
		GameObject parent = new GameObject("ChessPathParent");

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
				go.transform.parent = parent.transform;

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
