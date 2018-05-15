using UnityEngine;
using System.Collections;

public class AlertEffectHelper : MonoBehaviour {

    public Color color = Color.red;
    public float fadeoutTime = 2;
    public float alertTime = 3;
    public float radius = 0.5f;

    private float elapsed = 0;
    private bool playing = false;
    private Material material;
    private Transform childTrans;

    void Awake()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer == null)
            return;

        material = renderer.material;
        if (material == null)
            return;

        childTrans = renderer.transform;
    }

    public void Replay()
    {
        material.SetColor("_Color", color);
        material.SetFloat("_Scale", 0);
        playing = true;
        elapsed = 0;
        float scale = radius / 0.5f;
        childTrans.localScale = new Vector3(scale, scale, 1);
    }

	public void Update()
    {
        if (!playing)
            return;

        elapsed += Time.deltaTime;

        float percent = Mathf.Clamp01(elapsed / alertTime);
        material.SetFloat("_Scale", percent);

        if (elapsed > fadeoutTime + alertTime)
        {
            playing = false;
        }
    }
}
