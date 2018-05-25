using UnityEngine;
using FairyGUI;

public class CombatNormal : FlowBase
{
	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		if (childList == null || childList.Count <= 0)
			return false;

		RunningChild(args);

		return true;
	}

	public override bool OnStop(SkillArgs args)
	{
		if (!base.OnStop(args))
			return false;

		for (int idx = 0; idx < childList.Count; idx++)
		{
			childList[idx].OnStop(args);
		}

		return true;
	}
}