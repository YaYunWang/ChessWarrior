using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class SkillFlow : FlowBase
{
	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		if (childList == null || childList.Count <= 0)
			return false;

		OnNext(args);

		return true;
	}

	public bool OnNext(SkillArgs args)
	{
		if(args.FlowIndex >= 0)
		{
			childList[args.FlowIndex].OnLeave(args);
		}

		args.FlowIndex++;

		if (args.FlowIndex >= childList.Count)
		{
			args.FlowIndex--;

			OnFinish(args);
			return true;
		}

		//childList[args.FlowIndex].OnEntry(args);
		//EngineLauncher.Instance.StartCoroutine(DoNext(args));

		TimerManager.Instance.AddOnceTimer(string.Format("CombatFlow_{0}", args.Index), 0.001f, OnCombatFlowCallBack, args);
		return true;
	}

	private void OnCombatFlowCallBack(object[] arg1)
	{
		SkillArgs args = (SkillArgs)arg1[0];

		if (args.FlowIndex < 0 || args.FlowIndex >= childList.Count)
			return;

		childList[args.FlowIndex].OnEntry(args);
	}

	public bool OnFinish(SkillArgs args)
	{
		if(args.Owner != null)
		{
			args.Owner.StopCurrentSkill();
		}

		return true;
	}

	public override bool OnStop(SkillArgs args)
	{
		if (!base.OnStop(args))
			return false;

		TimerManager.Instance.RemoveTimer(string.Format("CombatFlow_{0}", args.Index));

		childList[args.FlowIndex].OnStop(args);

		return true;
	}
}
