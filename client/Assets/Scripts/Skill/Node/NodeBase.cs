using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBase : FlowBase
{
	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		for (int idx = 0; idx < childList.Count; idx++)
		{
			childList[idx].OnEntry(args);
		}

		return true;
	}
}
