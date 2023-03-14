using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceMarker : MonoBehaviour
{
    public GameObject redSpherePrefab;
    public GameObject arrowPrefab;
    public AudioSource loadingSound;
    private ARFaceManager arFaceManager;
    private ARSessionOrigin arOrigin;
    private Vector3 initialArrowPosition;

    void Awake()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arOrigin = GetComponent<ARSessionOrigin>();
        initialArrowPosition = arrowPrefab.transform.position;
    }

    void OnEnable()
    {
        arFaceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        arFaceManager.facesChanged -= OnFacesChanged;
    }

    void Update()
    {
        Touch touch = Input.GetTouch(0);

        if (Input.touchCount > 0)
        {
            Vector3 arrowPosition = arrowPrefab.transform.position;
            arrowPosition.y -= .005f;
            arrowPosition.z -= .005f;
            if (arrowPosition.y > initialArrowPosition.y - .3)
            {
                arrowPrefab.transform.position = arrowPosition;
                if (touch.phase == TouchPhase.Began)
                {
                    loadingSound.Play();
                }
            }
        }

        if(touch.phase == TouchPhase.Ended)
        {
            arrowPrefab.transform.position = initialArrowPosition;
            loadingSound.Stop();
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            // Instantiate a red sphere on top of the detected face
            GameObject apple = Instantiate(redSpherePrefab, face.transform.position, Quaternion.identity);

            apple.transform.parent = face.transform;
            apple.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // sphere.transform.localPosition = new Vector3(0f, face.transform.localScale.y * 0.5f + 0.05f, 0f);
            apple.transform.localPosition = new Vector3(0f, face.transform.localScale.y * 0.225f, 0f);

            Animation animation = apple.AddComponent<Animation>();
            //Animation animation = apple.GetComponent<Animation>();
            if (animation == null)
            {
                Application.Quit();
            }

            AnimationClip rotationClip = GetRotationAnimationClip();
            AnimationClip jumpingClip = GetJumpingAnimationClip();

            animation.AddClip(rotationClip, rotationClip.name);
            animation.AddClip(jumpingClip, jumpingClip.name);
            animation.Play(rotationClip.name);
            animation.Play(jumpingClip.name);
        }
    }

    AnimationClip GetRotationAnimationClip()
    {
        AnimationClip clip = new()
        {
            name = "Rotating"
        };

        // Define the animation curve for rotation
        AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 360);

        // Add the rotation curve to the clip
        clip.SetCurve("", typeof(Transform), "localEulerAngles.y", curve);

        // Set the clip length and loop time
        clip.wrapMode = WrapMode.Loop;
        clip.legacy = true;
        clip.EnsureQuaternionContinuity();
        //clip.frameRate = 60f;

        return clip;
    }

    AnimationClip GetJumpingAnimationClip()
    {
        AnimationClip clip = new AnimationClip
        {
            name = "Jumping"
        };

        // Define the animation curve for rotation
        AnimationCurve curve = new(
            new Keyframe(0, 0, 0, 5),     // Start at 0
            new Keyframe(0.5f, 1, 10, 0), // Sharp upward movement
            new Keyframe(1, 0, -5, 0)     // Gradual downward descent
        );

        // Add the rotation curve to the clip
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

        // Set the clip length and loop time
        clip.wrapMode = WrapMode.Loop;
        clip.legacy = true;
        clip.EnsureQuaternionContinuity();
        //clip.frameRate = 60f;

        return clip;
    }
}