using UnityEngine;
using System.Collections;

public class RectAlertEffectHelper : MonoBehaviour {

    public Color color = Color.red;
    public float fadeoutTime = 2;
    public float alertTime = 3;

    public float width = 1;
    public float length = 1;

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
        material.SetFloat("_Edge", 0.1f / length);

        playing = true;
        elapsed = 0;

        Vector3 offset = new Vector3(0, 0, 0.5f * length);
        Vector3 scale = new Vector3(width, length, 1);

        childTrans.localPosition = offset;
        childTrans.localScale = scale;
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
