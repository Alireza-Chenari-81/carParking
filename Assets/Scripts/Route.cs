using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public Vector3[] linePoints;
    public float maxLineLength; 

    [SerializeField] Linedrawer linedrawer;

    public Line line;
    public Park park;
    public Car car;

    [Space]
    [Header("Color :")]
    public Color carColor;
    [SerializeField] Color lineColor;

    public void Start()
    {
        linedrawer.OnParkLinkedToLine += OnParkLinkedToLineHandler;
    }

    private void OnParkLinkedToLineHandler(Route route, List<Vector3> points)
    {
        if(route == this)
        {
            linePoints = points.ToArray();
            Game.Instance.RegisterRoute(this);
        }
    }

    public void Disactivate()
    {
        isActive = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && line!=null && car!=null && park!=null) 
        {
            line.lineRenderer.SetPosition(0, car.buttomtransform.position);
            line.lineRenderer.SetPosition(1, park.transform.position);

            car.setColor(carColor);
            line.setColor(lineColor);
            park.setColor(carColor);
        }
    }
#endif
}
