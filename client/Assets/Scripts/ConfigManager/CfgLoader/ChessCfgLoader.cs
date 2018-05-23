using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class ChessCfg
{
	public int ID;
	public string Model;
	public string Controller;
	public string Script;
}

public class ChessCfgLoader : CfgLoaderBase
{
	private Dictionary<int, ChessCfg> m_data = new Dictionary<int, ChessCfg>();
	protected override void OnLoad()
	{
		ReadConfig<ChessCfg>("Config/chess_cfg.xml", OnReadRow);
	}

	private void OnReadRow(ChessCfg obj)
	{
		m_data[obj.ID] = obj;
	}

	protected override void OnUnload()
	{
		m_data.Clear();
	}

	public ChessCfg GetConfig(int configID)
	{
		ChessCfg config = null;
		m_data.TryGetValue(configID, out config);
		return config;
	}
}
