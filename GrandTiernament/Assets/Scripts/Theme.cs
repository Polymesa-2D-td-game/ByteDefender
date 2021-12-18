using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName ="Theme", menuName ="new Theme", order =0)]
public class Theme : ScriptableObject
{
    public Color background;
    public Color background2;

    public Color foreground;
    public Color comment;
    public Color highlighted;

    public Color mainColor;
    public Color secondaryColor;

    public Color negativeColor;
    public Color neutralColor;
    public Color positiveColor;
}
