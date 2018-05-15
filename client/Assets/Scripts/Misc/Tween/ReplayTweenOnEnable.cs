using UnityEngine;
using System.Collections;

public class ReplayTweenOnEnable : MonoBehaviour
{
	public UITweener[] tweens;

	void OnEnable()
	{
		if (tweens != null)
		{
			for (int i = 0; i < tweens.Length; i++)
			{
				var tween = tweens[i];
				tween.ResetToBeginning();
				tween.Play(true);
			}
		}
	}

}
