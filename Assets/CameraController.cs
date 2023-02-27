using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CameraController : MonoBehaviour
{
    public ARCameraManager cameraManager;
    public GameObject dotPrefab;
    private GameObject dot;

    void Awake()
    {

        if (dot == null)
        {
            dot = Instantiate(dotPrefab);
            dot.transform.SetParent(GameObject.Find("Canvas").transform, false);
            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Screen.height / 4f);
        }
    }

    void Start()
    {
        // Check if the AR camera is facing the user
        //if (cameraManager.requestedFacingDirection == CameraFacingDirection.User)
        //{
        //    Debug.Log("AR camera is facing the user");
        //}
        // Set the AR camera to face the world
        // cameraManager.requestedFacingDirection = CameraFacingDirection.World;
    }

    void Update()
    {
    }
}