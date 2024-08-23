using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = canvas.GetComponent<RectTransform>().sizeDelta.x - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = canvas.GetComponent<RectTransform>().sizeDelta.y - minY;

        Vector2 pos = Camera.main.WorldToViewportPoint(target.position);

        if(Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            if(pos.x < canvas.GetComponent<RectTransform>().sizeDelta.x / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        Debug.LogWarning(pos);

        img.rectTransform.position = pos;
    }
}
