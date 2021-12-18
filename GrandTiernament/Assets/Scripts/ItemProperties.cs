using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public enum Type
    {
        background,
        background2,
        foreground,
        comment,
        highlighted,
        main,
        secondary,
        negative,
        neutral,
        positive,
    }

    public Type type;
}
