using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private Theme theme;

    public TextMeshProUGUI[] texts;
    public Image[] images;
    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        ThemeAll();
    }

    private void ThemeAll()
    {
        texts = FindObjectsOfType<TextMeshProUGUI>();
        ThemeTexts();
        images = FindObjectsOfType<Image>();
        ThemeImages();
        buttons = FindObjectsOfType<Button>();
        ThemeButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ThemeButtons()
    {
        foreach (Button btn in buttons)
        {
            ItemProperties itemProperties = btn.GetComponent<ItemProperties>();
            ColorBlock buttonColors = btn.colors;
            buttonColors.highlightedColor = SetColor((int)itemProperties.type);
            btn.colors = buttonColors;
        }
    }

    private void ThemeTexts()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            ItemProperties itemProperties = text.GetComponent<ItemProperties>();
            text.color = SetColor((int)itemProperties.type);
        }
    }

    private void ThemeImages()
    {
        foreach (Image img in images)
        {
            ItemProperties itemProperties = img.GetComponent<ItemProperties>();
            img.color = SetColor((int)itemProperties.type);
        }    
    }

    private Color SetColor(int index)
    {
        switch (index)
        {
            case 0:
                return theme.background;
            case 1:
                return theme.background2;
            case 2:
                return theme.foreground;
            case 3:
                return theme.comment;
            case 4:
                return theme.highlighted;
            case 5:
                return theme.mainColor;
            case 6:
                return theme.secondaryColor;
            case 7:
                return theme.negativeColor;
            case 8:
                return theme.neutralColor;
            case 9:
                return theme.positiveColor;
            default:
                return theme.background;
        }
    }
}
