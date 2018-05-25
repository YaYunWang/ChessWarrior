using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkillCfgLoader : CfgLoaderBase
{
	private Dictionary<int, SkillBase> m_data = new Dictionary<int, SkillBase>();

	private static Dictionary<string, System.Type> skillNameType = new Dictionary<string, System.Type>();

	public SkillBase GetSkillCfg(int id)
	{
		return m_data.ContainsKey(id) ? m_data[id] : null;
	}

	protected override void OnLoad()
	{
		InitSkillType();

		XmlDocument document = ConfigManager.GetDocument("Config/skill_cfg.xml", "config.bundle");

		XmlNode mainNode = document.SelectSingleNode("Object");
		if (mainNode == null)
		{
			DebugLogger.LogError("skill cfg load error <<< Object Node not find.");
			return;
		}

		XmlNodeList mainNodeList = mainNode.ChildNodes;

		foreach (XmlNode mainChildNode in mainNodeList)
		{
			if (mainChildNode is XmlComment)
				continue;

			SkillBase cfg = ParseSkillCfg(mainChildNode);

			m_data.Add(cfg.ID, cfg);
		}
	}

	private SkillBase ParseSkillCfg(XmlNode combatCfg)
	{
		System.Type typeNode = GetSkillType(combatCfg.Name);
		if (typeNode == null)
		{
			return null;
		}

		SkillBase cfg = System.Activator.CreateInstance(typeNode) as SkillBase;

		if (!cfg.OnLoad(combatCfg))
		{
			return null;
		}
		return cfg;
	}

	public static System.Type GetSkillType(string nodeName)
	{
		return skillNameType.ContainsKey(nodeName) ? skillNameType[nodeName] : null;
	}

	private static void InitSkillType()
	{
		var types = typeof(SkillBase).Assembly.GetTypes();

		for (int i = 0; i < types.Length; i++)
		{
			var type = types[i];
			if (type.IsSubclassOf(typeof(SkillBase)))
			{
				skillNameType.Add(type.ToString(), type);
			}
		}
	}

	protected override void OnUnload()
	{
		m_data.Clear();
	}
}
