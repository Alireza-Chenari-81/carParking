using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Linedrawer linedrawer;

    [Space]
    [SerializeField] private CanvasGroup availablelinecanvasgroup;
    [SerializeField] private GameObject availablelineHolder;
    [SerializeField] private Image availablelineFill;

    private bool isAvailableLineUIActive = false;

    private Route activeRoute;

    [Space]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration;

    private void Start()
    {
        fadePanel.DOFade(0f, fadeDuration).From(1f);

        availablelinecanvasgroup.alpha = 0f;

        // Subscribe to the UnityEvent using AddListener instead of '+='
        linedrawer.OnBeginDrawn.AddListener(OnBeginDrawHandler);
        linedrawer.OnDrawn.AddListener(OnDrawHandler);
        linedrawer.OnEndDrawn.AddListener(OnEndDrawHandler);
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;

        availablelineFill.color = activeRoute.carColor;
        availablelineFill.fillAmount = 1f;
        availablelinecanvasgroup.DOFade(1f, 0.25f).From(0f);
        isAvailableLineUIActive = true;
    }
    private void OnDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            float maxlineLength = activeRoute.maxLineLength;
            float linelength = activeRoute.line.length;

            availablelineFill.fillAmount = 1 - (linelength / maxlineLength);
        }
    }
    private void OnEndDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;

            availablelinecanvasgroup.DOFade(0f, 0.25f).From(1f);
        }
    }

}
