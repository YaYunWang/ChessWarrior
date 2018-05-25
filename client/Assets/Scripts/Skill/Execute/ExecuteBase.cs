using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteBase : SkillBase
{
	protected ChessEntity GetTarget(SkillArgs args, string targetStr = "")
	{
		if(string.IsNullOrEmpty(targetStr))
		{
			targetStr = GetProp("Target");
		}

		if (string.IsNullOrEmpty(targetStr))
			return args.Owner;

		targetStr = targetStr.ToLower();

		switch (targetStr)
		{
			case "owner":
				return args.Owner;
			case "sender":
				return args.Sender;
			case "target":
				return args.Owner.Target;
		}

		return args.Owner;
	}
}
