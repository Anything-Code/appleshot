using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceMarker : MonoBehaviour
{
    public GameObject redSpherePrefab;
    private ARFaceManager arFaceManager;
    private ARSessionOrigin arOrigin;

    void Awake()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arOrigin = GetComponent<ARSessionOrigin>();
    }

    void OnEnable()
    {
        arFaceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        arFaceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            // Instantiate a red sphere on top of the detected face
            GameObject sphere = Instantiate(redSpherePrefab, face.transform.position, Quaternion.identity);
            sphere.transform.parent = face.transform;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            // sphere.transform.localPosition = new Vector3(0f, face.transform.localScale.y * 0.5f + 0.05f, 0f);
            sphere.transform.localPosition = new Vector3(0f, face.transform.localScale.y * 0.225f, 0f);
        }
    }
}