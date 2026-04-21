using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class CameraAnimation : MonoBehaviour
{
    [Header("For moving and animating the camera")]
    public CameraAni cameraAni;

    [Header("For camera fading in and out")]
    public FadeBox cameraFade;

    [Header("For animating the camera on a spline")]
    public splineCameraMovement splineCameraMovement;
    Transform camTransform;
    Transform camStartTransform;
    Transform camEndTransform;
    Transform[] transformsArray;
    AnimationCurve animatedCurve;
    GameObject tempCanvus;
    public Coroutine camAnimation;

    float animationTime;
    float cameraShakeAmount;
    bool findMainCamera;
    bool manyTransforms;
    bool corutineRunning;
    bool camShakeRunning;
    void Start()
    {
        findMainCamera = cameraAni.findCamera;
        camTransform = cameraAni.cameraTransform;
        animationTime = cameraAni.animationTime;
        animatedCurve = cameraAni.animatedCurve;
        cameraShakeAmount = cameraAni.cameraShakeAmount;

        

        // getting and setting references
        if (findMainCamera)
            camTransform = GameObject.FindWithTag("MainCamera").transform;
        if (transform.childCount > 2)
        {
            int count = transform.childCount;
            transformsArray = new Transform[count];

            for (int i = 0; i < transform.childCount; i++)
            {
                transformsArray.SetValue(transform.GetChild(i).transform, i);
            }
            manyTransforms = true;
        }

        // check for errors
        try
        {
            camStartTransform = transform.Find("CameraStartPos").transform;
            camEndTransform = transform.Find("CameraEndPos").transform;
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Couldn't find a instance of a camera start or end position as a child of this game object");
            this.enabled = false;
        }
        if (animatedCurve.length <= 1)
        {
            Debug.LogError("The animation curve is empty, add in a curve");
            this.enabled = false;
        }
    }

    /// <summary>
    /// Play the camera animations
    /// </summary>
    [ContextMenu("Play Animation")]
    private void startCameraMovement()
    {
        if (corutineRunning)
            return;
        if (GameObject.Find("FadeToBlackBox(Clone)") == null)
        {
            tempCanvus = Instantiate(cameraFade.fadeBoxPrefab, camTransform);
        }
        else
        {
            tempCanvus = GameObject.Find("FadeToBlackBox(Clone)");
        }

        cameraFade.fadeBox = tempCanvus.transform.Find("FadeOutbox").GetComponent<Image>();

        camAnimation = StartCoroutine(camMovement());
    }


    /// <summary>
    /// Animate the camera between two or more locations
    /// </summary>
    /// <returns></returns>
    public IEnumerator camMovement()
    {
        cameraFade.fadeBox.color = new Color(0, 0, 0, 0);
        camTransform.position = camStartTransform.position;

        // fade camera in
        if (cameraFade.fadeIn)
            yield return fadeEffect(true, false, 255);

        if (cameraShakeAmount != 0)
            //StartCoroutine(cameraShake());
        corutineRunning = true;
        float timeTaken = 0;
        Vector3 pos = camTransform.position;

        // play the animation if there is more than two children
        if (manyTransforms)
        {
            for (int i = 1; i < transformsArray.Length; i++)
            {
                timeTaken = 0;
                while (timeTaken <= animationTime)
                {
                    timeTaken += Time.deltaTime;
                   
                    camTransform.position = Vector3.Lerp(transformsArray[i - 1].position, transformsArray[i].position, animatedCurve.Evaluate(timeTaken / animationTime));
                    if (!camShakeRunning)
                        camTransform.eulerAngles = Vector3.Lerp(transformsArray[i - 1].eulerAngles, transformsArray[i].eulerAngles, animatedCurve.Evaluate(timeTaken / animationTime));  camTransform.eulerAngles += shakeValue();
                    if (cameraAni.lookAt)
                        camTransform.LookAt(cameraAni.lookAtTarget);
                    yield return null;
                }
                camTransform.position = transformsArray[i].position;
            }
        }

        // play animation if there is only two animation spots
        while (timeTaken <= animationTime)
        {
            timeTaken += Time.deltaTime;
            
            camTransform.position = Vector3.Lerp(camStartTransform.position, camEndTransform.position, animatedCurve.Evaluate(timeTaken / animationTime));
            if (!camShakeRunning)
                camTransform.eulerAngles = Vector3.Lerp(camStartTransform.eulerAngles, camEndTransform.eulerAngles, animatedCurve.Evaluate(timeTaken / animationTime)); camTransform.eulerAngles += shakeValue();
            if (cameraAni.lookAt)
                camTransform.LookAt(cameraAni.lookAtTarget);
            yield return null;
        }

        // set the camera to the exact position of the end point
        if (!manyTransforms)
            camTransform.position = camEndTransform.position;

        // fade camera out
        if (cameraFade.fadeOut)
            yield return fadeEffect(false, true, 0);
        // stop the camera shaking
        corutineRunning = false;
        camShakeRunning = false;
    }

    Vector3 shakeValue()
    {
        Vector3 camRotationShake = Vector3.zero;
        camRotationShake.x = Random.Range(-cameraShakeAmount, cameraShakeAmount);
        camRotationShake.y = Random.Range(-cameraShakeAmount, cameraShakeAmount);
        return camRotationShake;
    }
    IEnumerator fadeEffect(bool fadeBoxIn, bool fadeBoxOut, float fadeAmount)
    {
        Color boxColor = cameraFade.fadeColor;
        boxColor.a = fadeAmount;
        cameraFade.fadeBox.color = boxColor;
        
        yield return new WaitForSeconds(cameraFade.fadeDuration/2);

        if (fadeBoxIn)
        {
            while (fadeAmount > 0)
            {
                fadeAmount -= 1;
                boxColor.a = fadeAmount / 255;
                cameraFade.fadeBox.color = boxColor;
                yield return new WaitForSeconds(cameraFade.fadeDuration / 255);

            }
        }
        if (fadeBoxOut)
        {
            while (fadeAmount < 255)
            {
                fadeAmount += 1;
                boxColor.a = fadeAmount / 255;
                cameraFade.fadeBox.color = boxColor;
                yield return new WaitForSeconds(cameraFade.fadeDuration / 255);
            }
        }
        yield return null;
    }
    [ContextMenu("Spline Animation")]
    private void playSpineAnimation()
    {
        GameObject camera = camTransform.gameObject;

        SplineAnimate cameraSplineAnimate = camera.AddComponent<SplineAnimate>();
        cameraSplineAnimate.Container = splineCameraMovement.animationSpline;
        cameraSplineAnimate.Loop = SplineAnimate.LoopMode.Once;
        cameraSplineAnimate.Duration = splineCameraMovement.animationTime;
        if (splineCameraMovement.loop)
            cameraSplineAnimate.Loop = SplineAnimate.LoopMode.Loop;
        cameraSplineAnimate.Play();
    }
}
