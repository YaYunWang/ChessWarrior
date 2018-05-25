using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FlowTime : FlowBase
{
	private List<float> m_time_list = new List<float>();

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		string[] time_ary = GetProp("Time").Split(',');
		for (int idx = 0; idx < time_ary.Length; idx++)
		{
			float time = float.Parse(time_ary[idx]) / 1000f;

			m_time_list.Add(time);
		}

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		RunningChild(args, "StartPut");

		// 2、开定时器
		TimerManager.Instance.AddOnceTimer(string.Format("FlowTime_{0}", args.Index), m_time_list[0], OnTimerCallback, args, 0);

		return true;
	}

	private void OnTimerCallback(object[] args)
	{
		SkillArgs combat_args = args[0] as SkillArgs;
		int index = (int)args[1];

		RunningChild(combat_args, "OutPut", index);
		index++;

		if (index >= m_time_list.Count)
		{
			DoNext(combat_args);
		}
		else
		{
			TimerManager.Instance.AddOnceTimer(string.Format("FlowTime_{0}", combat_args.Index), m_time_list[index], OnTimerCallback, combat_args, index);
		}
	}

	public override bool OnLeave(SkillArgs args)
	{
		if (!base.OnLeave(args))
			return false;

		TimerManager.Instance.RemoveTimer(string.Format("FlowTime_{0}", args.Index));
		return true;
	}

	public bool DoNext(SkillArgs args)
	{
		if (Parent == null)
		{
			DebugLogger.LogError("skill execute error...");

			return false;
		}

		SkillFlow flow = Parent as SkillFlow;

		if (flow == null)
		{
			DebugLogger.LogError("skill execute error...");

			return false;
		}

		flow.OnNext(args);

		return true;
	}
}
