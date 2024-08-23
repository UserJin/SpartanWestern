using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform firePosition;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, firePosition.position + firePosition.TransformDirection(new Vector3(0, 0, 10)));
    }

}
