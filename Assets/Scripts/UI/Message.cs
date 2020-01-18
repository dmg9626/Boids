using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class Message
{
    [Header("Text Settings")]

    /// <summary>
    /// Text to show in larger main text element
    /// </summary>
    public string mainText;

    /// <summary>
    /// Font to use for main text
    /// </summary>
    public Font mainTextFont;

    /// <summary>
    /// Size of main text
    /// </summary>
    public int mainTextSize = 60;
    
    [Space()]
    
    /// <summary>
    /// Text to show in smaller sub-text element
    /// </summary>
    public string subText;
    
    /// <summary>
    /// Font to use for sub-text
    /// </summary>
    public Font subTextFont;

    /// <summary>
    /// Size of sub text
    /// </summary>
    [SerializeField]
    public int subTextSize = 40;

    /// <summary>
    /// Amount of time to wait after main text appears before showing sub-text 
    /// </summary>
    public float subTextDelay;

    [Header("Other Settings")]

    /// <summary>
    /// Duration to show text
    /// </summary>
    public float duration = 3;

    /// <summary>
    /// If true, specified settings will be applied to boids
    /// </summary>
    public bool setBoidSettings = false;

    /// <summary>
    /// Settings to apply to boids (only used if setBoidSettings is true)
    /// </summary>
    public Boid.Settings boidSettings;
}
