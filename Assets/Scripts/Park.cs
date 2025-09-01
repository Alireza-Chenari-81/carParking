using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    public Route route;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem fx;
    private ParticleSystem.MainModule fxMainModule;

    private void Start()
    {
        fxMainModule = fx.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Car car))
        {
            if(car.route == route)
            {
                Game.Instance.onCarEntersPark?.Invoke(route);
                StartFX();
            }
        }
    }

    private void StartFX()
    {
        fxMainModule.startColor = route.carColor;
        fx.Play();
    }

    public void setColor(Color color)
    {
        spriteRenderer.color = color;
    }

}
