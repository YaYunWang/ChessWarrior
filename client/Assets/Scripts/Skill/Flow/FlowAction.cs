using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FlowAction : FlowTime
{
	private string m_action_name = string.Empty;

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		m_action_name = GetProp("Action");

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		if(!string.IsNullOrEmpty(m_action_name))
			args.Owner.PlayAction(m_action_name);

		return true;
	}
}
