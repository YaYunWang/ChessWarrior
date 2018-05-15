using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformMisc
{
    public static void ResetPRS(this Transform root)
    {
        root.localPosition = Vector3.zero;
        root.localRotation = Quaternion.identity;
        root.localScale = Vector3.one;
    }

    public static Transform Search(this Transform root, string name)
    {
        if (root.name == name)
            return root;

        int count = root.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = root.GetChild(i);

            Transform found = Search(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

    public static string GetChildPath(this Transform root, Transform node)
    {
        List<string> nameList = new List<string>();
        GetChildPath(nameList, root, node);
        nameList.Reverse();
        return string.Join("/", nameList.ToArray());
    }

    private static void GetChildPath(List<string> nameList, Transform root, Transform node)
    {
        if (node.parent == null || node == root)
            return;

        nameList.Add(node.name);

        GetChildPath(nameList, root, node.parent);
    }
}



