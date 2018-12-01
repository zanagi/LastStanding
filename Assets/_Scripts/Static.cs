using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using System.Collections.Generic;

public static class Static
{
    public static readonly string horizontalAxis = "Horizontal", verticalAxis = "Vertical";

    public static void SetAlpha(this MaskableGraphic graphics, float alpha)
    {
        Color color = graphics.color;
        color.a = alpha;
        graphics.color = color;
    }

    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool TouchedOverUI
    {
        get { return EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null; }
    }

    public static void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int idx = Random.Range(i, array.Length);

            //swap elements
            T tmp = array[i];
            array[i] = array[idx];
            array[idx] = tmp;
        }
    }

    public static T GetRandom<T>(this T[] array)
    {
        if (array.Length == 0)
        {
            return default(T);
        }
        return array[Random.Range(0, array.Length)];
    }
}
