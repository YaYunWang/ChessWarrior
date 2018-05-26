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

    private static VectorLine pathLine;
    public static void ShowChessBoard()
    {
        //GameObject go = new GameObject("ChessBoard");
        //var linePoints = new Vector3[2];
        //linePoints[0] = new Vector3(0, 1, 0);
        //linePoints[1] = new Vector3(10, 1, 0);

        //var line = new VectorLine("Line", linePoints, null, 2.0f);
        //line.drawTransform = go.transform;

        //line.points3.Add(new Vector3(10, 1, 10));

        //line.Draw();
        //line.Draw3D();

        pathLine = new VectorLine("Path", new List<Vector3>(), 12.0f);
        pathLine.color = Color.green;
        pathLine.textureScale = 1.0f;

        pathLine.points3.Add(new Vector3(10, 0, 0));
        pathLine.points3.Add(new Vector3(10, 0, 10));

        pathLine.Draw();
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
