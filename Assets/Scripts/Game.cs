using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class Game : MonoBehaviour
{
    //singleton class:
    public static Game Instance;

    [HideInInspector] public List<Route> readyRoutes = new();

    private int totalRoutes;
    private int successfulParks;

    public UnityAction<Route> onCarEntersPark;
    public UnityAction onCarCollision;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        successfulParks = 0;
        onCarEntersPark += OnCarEntersParkHandler;
        onCarCollision += OnCarCollisionHandler;
    }

    private void OnCarCollisionHandler()
    {
       Debug.Log("Game Over! Restarting level...");

        DOVirtual.DelayedCall(2f, () =>
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        });
    }

    private void OnCarEntersParkHandler(Route route)
    {
        route.car.stopdancinganim();
        successfulParks++;

        if (successfulParks == totalRoutes)
        {
            Debug.Log("All cars parked successfully!");
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (nextLevel < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextLevel);
                }
                else
                {
                    Debug.LogWarning("No more levels to load!");
                }
            });
        }

    }

    public void RegisterRoute(Route route) {
        readyRoutes.Add(route);

        if(readyRoutes.Count == totalRoutes)
        {
            MoveAllCars();
            Debug.Log("All routes are ready!");
        }   
    }

    private void MoveAllCars()
    {
        foreach (var route in readyRoutes) {
            route.car.Move(route.linePoints);
        }
    }
}
