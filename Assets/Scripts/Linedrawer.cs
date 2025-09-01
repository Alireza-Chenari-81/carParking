using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Linedrawer : MonoBehaviour
{
    [SerializeField] UserInput userInput;
    [SerializeField] int interactableLayer;

    private Line currentLine;
    private Route currentRoute;

    RaycastDetector raycastDetector = new();

    public UnityEvent<Route> OnBeginDrawn;
    public UnityEvent OnDrawn;
    public UnityEvent OnEndDrawn;

    public UnityAction<Route, List<Vector3>> OnParkLinkedToLine;

    private void Start()
    {
        userInput.OnMouseDown += OnMouseDownHandler;
        userInput.OnMouseMove += OnMouseMoveHandler;
        userInput.OnMouseUp += OnMouseUpHandler;
    }

    /// <summary>
    /// Handles mouse down events to start drawing a line.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnMouseDownHandler()
    {
        ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

        if (contactInfo.contacted)
        {
            bool isCar = contactInfo.collider.TryGetComponent(out Car _car);

            if (isCar && _car.route.isActive) { 
                currentRoute = _car.route;
                currentLine = currentRoute.line;
                currentLine.Init();

                OnBeginDrawn?.Invoke(currentRoute);
            }
        }
    }

    /// <summary>
    /// Handles mouse move events to update the line being drawn or the route being created.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnMouseMoveHandler()
    {
        if(currentRoute!= null) {

            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;
                if(currentLine.length >= currentRoute.maxLineLength)
                {
                    currentLine.Clear();
                    OnMouseUpHandler();
                    return;
                }

                currentLine.AddPoint(newPoint);
                OnDrawn?.Invoke();

                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);

                if (isPark)
                {
                    Route parkRoute = _park.route;
                    if (parkRoute == currentRoute)
                    {
                        currentLine.AddPoint(contactInfo.transform.position);
                        OnDrawn?.Invoke();
                    }
                    else
                    {
                        //delete the line:
                        currentLine.Clear();
                    }
                    OnMouseUpHandler();
                }
            }
        }
    }

    /// <summary>
    /// Handles mouse up events to finalize the line drawing or route creation.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnMouseUpHandler()
    {
        if (currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.RayCast(interactableLayer);

            if (contactInfo.contacted)
            {
                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);

                if (currentLine.pointsCount < 2 || !isPark)
                {
                    currentLine.Clear();
                }
                else 
                {
                    OnParkLinkedToLine?.Invoke(currentRoute,currentLine.points);
                    currentRoute.Disactivate();
                }
            }
            else
            {
                currentLine.Clear();
            }
        }
        ResetDrawer();
        OnEndDrawn?.Invoke();
    }
    private void ResetDrawer()
    {
        currentLine = null;
        currentRoute = null;    

    }
}
