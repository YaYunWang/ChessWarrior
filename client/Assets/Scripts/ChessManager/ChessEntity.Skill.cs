using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public partial class ChessEntity
{
	private SkillBase currentSkill = null;
	private SkillArgs currentSkillArgs = null;

	public bool UseNormalSkill()
	{
		return UseSkill(chessCfg.Skill);
	}

	public bool UseSkill(int skillID)
	{
		if (currentSkill != null)
			return false;

		currentSkill = ConfigManager.Get<SkillCfgLoader>().GetSkillCfg(skillID);
		if (currentSkill == null)
			return false;

		currentSkillArgs = new SkillArgs();
		currentSkillArgs.OnReset();
		currentSkillArgs.Owner = this;
		currentSkillArgs.Sender = this;

		return currentSkill.OnEntry(currentSkillArgs);
	}

	public bool StopCurrentSkill()
	{
		if (currentSkill != null)
		{
			PlayAction("Idle");

			currentSkill.OnStop(currentSkillArgs);
			currentSkill = null;
			currentSkillArgs = null;

			// 
			if(Target != null)
			{
				if(Target.IsDead)
				{
					// 发送，杀死对方，继续下一回合
					GameEventManager.RaiseEvent(GameEventTypes.KillChess, this, Target);
				}
				else
				{
					Target.UseNormalSkill();
				}
			}
		}

		return true;
	}
}
