using System;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class CameraAni
{
    [Header("Find a camera in the scene")]
    [Tooltip("Finds a camera with a MainCamera Tag")]
    public bool findCamera;
    [Header("Camera for animation")]
    public Transform cameraTransform;
    [Space(10.0f)]
    [Header("For camera Animation")]
    [Tooltip("How long for the animation to take")]
    public float animationTime;
    public AnimationCurve animatedCurve;
    [Range(0, 1)]
    public float cameraShakeAmount;
    [Header("Looking at target")]
    public bool lookAt = false;
    public Transform lookAtTarget;
}
[Serializable]
public class FadeBox
{
    public GameObject fadeBoxPrefab;
    [HideInInspector]
    public UnityEngine.UI.Image fadeBox;
    public Color fadeColor;
    public bool fadeIn;
    public bool fadeOut;

    [Header("How long the fade takes in seconds")]
    public float fadeDuration;

}
[Serializable]
public class splineCameraMovement
{
    public SplineContainer animationSpline;
    public float animationTime = 1.0f;
    public bool loop = false;

}