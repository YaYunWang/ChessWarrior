using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectQualitySwitcher : MonoBehaviour
{
    public GameObject[] optionals;

    private EffectRuntimeQuality m_quality = EffectRuntimeQuality.High;

    public void SetQuality(EffectRuntimeQuality quality)
    {
        if (m_quality == quality)
            return;

        for (int i = 0; i < optionals.Length; i++)
        {
            optionals[i].SetActive(quality == EffectRuntimeQuality.High);

        }
    }
}



