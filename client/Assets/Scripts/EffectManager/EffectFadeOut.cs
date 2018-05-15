using UnityEngine;
using System.Collections;


public class EffectFadeOut : MonoBehaviour
{

    public ParticleSystem[] particleSystems;
    public UITweener[] tweeners;

    public void OnDisable()
    {
        for (int i = 0; i < tweeners.Length; i++)
        {
            if (tweeners[i] != null)
                tweeners[i].ResetToBeginning();
        }
    }

    public void BeginFadeOut()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i] != null)
                particleSystems[i].Stop(false);
        }

        for (int i = 0; i < tweeners.Length; i++)
        {
            if (tweeners[i] != null)
            {
                tweeners[i].PlayForward();
            }
        }
    }

    public void EnableFadeOut()
    {
        for (int i = 0; i < tweeners.Length; i++)
        {
            if (tweeners[i] != null)
            {
                tweeners[i].enabled = false;
            }
        }
    }
}



