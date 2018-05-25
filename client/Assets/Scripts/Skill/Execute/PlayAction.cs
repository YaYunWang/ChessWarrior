using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PlayAction : ExecuteBase
{
	private string actionName = string.Empty;

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		actionName = GetProp("ActionName");

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		ChessEntity target = GetTarget(args);
		if (target == null)
			return false;

		target.PlayAction(actionName);

		return true;
	}
}
