using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChessEntity
{
	private List<Material> materialsCache;
	private List<Color> savedMaterialColors;

	private bool SearchMaterials()
	{
		if (Renderers == null)
		{
			return false;
		}

		if (materialsCache == null)
		{
			materialsCache = new List<Material>();

			for (int i = 0; i < Renderers.Length; i++)
			{
				var mats = Renderers[i].materials;
				materialsCache.AddRange(mats);
			}

			savedMaterialColors = new List<Color>(materialsCache.Count);

			for (int i = 0; i < materialsCache.Count; i++)
			{
				var color = materialsCache[i].GetColor("_Color");

				savedMaterialColors.Add(color);
			}
		}

		return true;
	}

	public void MaterialsColor(Color color)
	{
		if (!SearchMaterials())
			return;

		for(int idx = 0; idx < materialsCache.Count; idx++)
		{
			materialsCache[idx].SetColor("_Color", color);
		}
	}

	public void RevertMaterialsColor()
	{
		if (!SearchMaterials())
			return;

		for (int idx = 0; idx < materialsCache.Count; idx++)
		{
			materialsCache[idx].SetColor("_Color", savedMaterialColors[idx]);
		}
	}
}
