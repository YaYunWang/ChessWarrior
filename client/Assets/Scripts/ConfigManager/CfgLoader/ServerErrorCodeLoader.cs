using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class ServerErrorCodeCfg
{
	public int ID;
	public string Error;
}

public class ServerErrorCodeLoader : CfgLoaderBase
{
	private Dictionary<int, ServerErrorCodeCfg> m_data = new Dictionary<int, ServerErrorCodeCfg>();
	protected override void OnLoad()
	{
		ReadConfig<ServerErrorCodeCfg>("Config/server_error_code.xml", OnReadRow);
	}

	private void OnReadRow(ServerErrorCodeCfg obj)
	{
		m_data[obj.ID] = obj;
	}

	protected override void OnUnload()
	{
		m_data.Clear();
	}

	public ServerErrorCodeCfg GetConfig(int configID)
	{
		ServerErrorCodeCfg config = null;
		m_data.TryGetValue(configID, out config);
		return config;
	}
}
