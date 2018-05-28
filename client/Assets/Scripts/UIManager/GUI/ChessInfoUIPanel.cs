using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class ChessInfoUIPanel : GUIBase
{
	private Dictionary<ChessEntity, GComponent> m_infos = new Dictionary<ChessEntity, GComponent>();
	protected override void OnStart()
	{
		base.OnStart();

		UIPackage.AddPackage("ChessInfo", (string name, string extension, System.Type type) => { return AssetLoadManager.LoadAsset("ui/chessinfo.bundle", name, type); });
	}

	public void ShowChessInfo(ChessEntity chess)
	{
		GComponent info = (GComponent)UIPackage.CreateObject("ChessInfo", "ChessInfoItem");
		if(info == null)
		{
			Debug.LogError("show chess info error.");
			return;
		}
		this.AddChild(info);

		GTextField name = info.GetChild("name").asTextField;
		name.text = chess.chessCfg.Name;

		m_infos.Add(chess, info);
	}

	public void RemoveChessInfo(ChessEntity chess)
	{
		if(m_infos.ContainsKey(chess))
		{
			GComponent info = m_infos[chess];

			m_infos.Remove(chess);

			info.RemoveFromParent();

			info.Dispose();
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

		foreach (var chess in m_infos.Keys)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(chess.transform.position);
			//原点位置转换
			screenPos.y = Screen.height - screenPos.y;
			Vector2 pt = GRoot.inst.GlobalToLocal(screenPos);

			m_infos[chess].SetXY(pt.x, pt.y);

			GTextField hp = m_infos[chess].GetChild("hp").asTextField;

			hp.text = string.Format("{0}/{1}", chess.HP, chess.chessObj.max_hp);
		}
	}
}
