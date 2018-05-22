using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChessEntity
{
	protected RuntimeAnimatorController baseController;
	protected AnimatorOverrideController overrideController;

	private bool pauseAnimation = false;
	private float animationSpeed = 1;

	public bool EnableEntityAction = false;

	protected void SetAnimatorController(RuntimeAnimatorController controller)
	{
		EnableEntityAction = false;
		baseController = controller;

		if (overrideController == null)
		{
			overrideController = new AnimatorOverrideController();
			overrideController.runtimeAnimatorController = baseController;
		}

		animator.runtimeAnimatorController = overrideController;
	}

	public void PlayAction(string action_name, bool force = false, int layer = 0, int normailed = 0)
	{
		if(force)
		{
			if (IsAnimatorValid())
				animator.Play(action_name, layer, normailed);
		}
		else
		{
			if(!EnableEntityAction)
			{
				if (IsAnimatorValid())
					animator.Play(action_name, layer, normailed);
			}
		}
	}

	public bool GetAnimationBool(string name)
	{
		if (!IsAnimatorValid())
			return false;

		return animator.GetBool(name);
	}

	public void SetAnimationBool(string name, bool val)
	{
		if (IsAnimatorValid())
			animator.SetBool(name, val);
	}

	public void SetAnimationFloat(string name, float val)
	{
		if (IsAnimatorValid())
			animator.SetFloat(name, val);
	}

	public void SetAnimationInt(string name, int val)
	{
		if (IsAnimatorValid())
		{
			animator.SetInteger(name, val);
			animator.Update(Time.deltaTime);
		}
	}

	public void SetAnimationTrigger(string name)
	{
		if (IsAnimatorValid())
			animator.SetTrigger(name);
	}

	public void PlayDeadAnimation()
	{
		if (animator == null)
			return;

		if (IsAnimatorValid())
			animator.CrossFade("Dead", 0.2f);
	}

	public void PlayDeadStateAnimation()
	{
		if (animator == null)
			return;

		if (IsAnimatorValid())
			animator.Play("Dead");
	}

	public void StopDeadAnimation()
	{
		if (animator == null)
			return;

		if (IsAnimatorValid())
			animator.Play("Stand");
	}

	public void PauseAnimation()
	{
		pauseAnimation = true;
		RefreshAnimationSpeed();
	}

	public void ResumeAnimation()
	{
		pauseAnimation = false;
		RefreshAnimationSpeed();
	}

	private void RefreshAnimationSpeed()
	{
		if (animator == null)
			return;

		if (pauseAnimation)
		{
			animator.speed = 0;
		}
		else
		{
			animator.speed = animationSpeed;
		}
	}

	public void SetSkillSpeed(float speedScale = 1.0f)
	{
		SetAnimationFloat("skill_speed", speedScale);
	}

	public void SetMoveSpeed(float speedScale = 1.0f)
	{
		SetAnimationFloat("move_speed", speedScale);
	}

	private bool IsAnimatorValid()
	{
		return animator != null && animator.isInitialized;
	}
}
