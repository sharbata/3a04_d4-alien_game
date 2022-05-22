using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    // Start is called before the first frame update

    
    private bool isFadingIn;
    private bool isFadingOut;
    private bool isBlackScreen;
    private float blackScreenCounter;
    
    private Color fadeColor;
    [SerializeField] private Image ScreenFadeBackground;
    [SerializeField] private CanvasGroup UICanvasGroup;

    [SerializeField] private float fadeInTime = 3.0f;
    [SerializeField] private float fadeOutTime = 2.0f;
    [SerializeField] private float blackScreenTime = 0.5f;

    [SerializeField] private SpecimenBase specimenBase;

    public void StartFade()
    {
        isFadingIn = true;
        UICanvasGroup.interactable = false;
    }

    private void StopFade()
    {
        isFadingIn = false;
        isFadingOut= false;
        isBlackScreen = false;
        blackScreenCounter = 0;
        UICanvasGroup.interactable = true;
    }
    void Awake()
    {
        if (ScreenFadeBackground == null)
        {
            ScreenFadeBackground = GetComponent<Image>();
        }
        isFadingIn = false;
        isFadingOut = false;
        isBlackScreen = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFadingIn)
        {
            float incrementer = Time.deltaTime / fadeInTime ;
            fadeColor = ScreenFadeBackground.color;
            fadeColor.a += incrementer;
            ScreenFadeBackground.color = fadeColor;
            if (Math.Abs(fadeColor.a - 1) < 0.001f) 
            {
                blackScreenCounter = 0;
                isBlackScreen = true;
                isFadingIn = false;        
                specimenBase.EnergyPoints = 10;
            }
            
        }
        else if (isBlackScreen)
        {
            float incrementer = Time.deltaTime / blackScreenTime ;
            blackScreenCounter += incrementer;
            if (blackScreenCounter >= 1.0f - 0.01f) 
            {
                blackScreenCounter = 0;
                isFadingOut = true;
                isBlackScreen = false;
            }
        }
        else if (isFadingOut)
        {
            float incrementer = Time.deltaTime / fadeOutTime ;
            fadeColor = ScreenFadeBackground.color;

            if (incrementer >= fadeColor.a)
            {
                fadeColor.a = 0;
            }
            else
            {
                fadeColor.a -= incrementer;
            }
            ScreenFadeBackground.color = fadeColor;
            
            if (Math.Abs(fadeColor.a) < 0.001f) 
            {
                isFadingOut = false;
                StopFade();
            }
        }
    }
}
