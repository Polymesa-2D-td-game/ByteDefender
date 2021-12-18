using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour
{
    public Canvas parentCanvas;
    private bool isMouseDonwn = false;
    private void Start()
    {
        
    }
    public void MouseDown()
    {
        isMouseDonwn = true;
    }

    public void MouseUp()
    {
        isMouseDonwn = false;
    }

    private void Update()
    {
        MoveWindowWithMouse();
    }

    private void MoveWindowWithMouse()
    {
        if (isMouseDonwn)
        {
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);

            float yOffset = GetComponent<RectTransform>().rect.height / 2;
            float xOffset = GetComponent<RectTransform>().rect.height / 2; 
            transform.position = parentCanvas.transform.TransformPoint(movePos - new Vector2(0f, yOffset));
        }
    }
}
