using UnityEngine;
using System.Collections;

public class RectAlertDiffusionEffectHelper : MonoBehaviour {

    public Color color = Color.red;
    public float fadeoutTime = 2;
    public float alertTime = 3;

    public float width = 1;
    public float length = 1;

    private float elapsed = 0;
    private bool playing = false;
	private Material material;
	public Renderer m_renderer;
	public Transform childTrans;

    void Awake()
    {
		if (m_renderer == null)
			return;

		material = m_renderer.material;
	}

    public void Replay()
    {
        material.SetColor("_Color", color);
        material.SetFloat("_Scale", 0);
        material.SetFloat("_Edge", 0.1f / length);

        playing = true;
        elapsed = 0;

        Vector3 scale = new Vector3(width, 1, length);

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
