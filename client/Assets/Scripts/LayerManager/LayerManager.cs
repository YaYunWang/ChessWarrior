

using UnityEngine;


public class LayerManager : ManagerTemplateBase<LayerManager>
{
    public static int InvisibleLayer{ private set; get; }

    public static int UILayer { private set; get; }

    public static int DefaultLayer { private set; get; }

    protected override void InitManager()
    {
        InvisibleLayer = LayerMask.NameToLayer("Invisible");
        UILayer = LayerMask.NameToLayer("UI");
        DefaultLayer = LayerMask.NameToLayer("Default");
    }

	public static void SetLayer(GameObject go, int layer)
	{
		if (go == null)
			return;

		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
		SetLayer(renderers, layer);
	}

    public static void SetLayer(Renderer[] renderers, int layer)
    {
        if (renderers == null)
            return;
        int len = renderers.Length;
        for (int i = 0; i < len; i++)
        {
            renderers[i].gameObject.layer = layer;
        }
    }
}


