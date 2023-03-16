using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FaceMarker : MonoBehaviour
{
    private const float HIT_POINT = 1;
    private const float MAX_ARROWS = 10;

    public GameObject redSpherePrefab, arrowPrefab;
    public AudioSource loadingSound, shootingSound, hittingSound, winningSound;
    private ARFaceManager arFaceManager;
    private ARSessionOrigin arOrigin;
    private Vector3 initialArrowPosition;
    private GameObject apple;
    private Rigidbody arrowRB;
    private bool isShooting = false;
    private float force = 1;

    private bool isGameOver = false;
    private bool canShoot = true;
    private float currentScore = 0;
    private float arrowsLeft = MAX_ARROWS;

    [SerializeField] private TextMeshProUGUI scoretext, arrowsLeftText, gameOverText, reloadButton;
    [SerializeField] private Button reloadButtonButton;

    void Awake()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arOrigin = GetComponent<ARSessionOrigin>();
    }

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        arrowsLeft = MAX_ARROWS;
        initialArrowPosition = arrowPrefab.transform.position;
        arrowRB = arrowPrefab.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        arFaceManager.facesChanged += OnFacesChanged;
        Rotate.ArrowHit += OnArrowHit;
    }

    void OnDisable()
    {
        arFaceManager.facesChanged -= OnFacesChanged;
        Rotate.ArrowHit -= OnArrowHit;
    }

    private void OnArrowHit()
    {
        currentScore += HIT_POINT;
        scoretext.SetText("Score: " + currentScore);
    }

    public void ResetArrow()
    {
        arrowRB.velocity = new Vector3(0f, 0f, 0f);
        arrowRB.angularVelocity = new Vector3(0f, 0f, 0f);
        arrowRB.useGravity = false;
        arrowPrefab.transform.position = initialArrowPosition;
        isShooting = false;
    }

    void Update()
    {
        if(isGameOver)
        {
            return;
        }
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.position.y > 1600 || isShooting) return;

        if (Input.touchCount > 0 && arrowsLeft > 0 && canShoot)
        {
            Vector3 arrowPosition = arrowPrefab.transform.position;
            arrowPosition.y -= .004f;
            arrowPosition.z -= .004f;
            if (arrowPosition.y > initialArrowPosition.y - .25)
            {
                force += 1;
                arrowPrefab.transform.position = arrowPosition;
                if (touch.phase == TouchPhase.Began)
                {
                    loadingSound.Play();
                }
            }
        }

        if(touch.phase == TouchPhase.Ended && arrowsLeft > 0 && canShoot)
        {
            canShoot = false;
            //arrowPrefab.transform.position = initialArrowPosition;
            loadingSound.Stop();
            shootingSound.Play();
            isShooting = true;

            Vector3 applePosition = new Vector3(
                apple.transform.position.x,
                apple.transform.position.y + .5f,
                apple.transform.position.z
            );
            Vector3 direction = (applePosition - transform.position).normalized;
            direction = new Vector3(Camera.main.transform.forward.x, direction.y, direction.z);
            direction = direction.normalized;
            var finalForce = direction * force;

            arrowRB.AddForce(finalForce, ForceMode.Impulse);
            //arrowRB.useGravity = true;
            force = 1;
            arrowsLeft--;
            arrowsLeftText.SetText("Arrows: " + arrowsLeft);
            StartCoroutine(ArrowTimer());
        }
    }

    private IEnumerator ArrowTimer()
    {
        yield return new WaitForSecondsRealtime(2f);
        canShoot = true;

        if(arrowsLeft <= 0)
        {
            isGameOver = true;
            winningSound.Play();
            reloadButtonButton.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(true);
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            AddApple(face);
            continue;
        }
    }

    private void AddApple(ARFace face)
    {
        apple = Instantiate(redSpherePrefab, face.transform.position, Quaternion.identity, face.transform);
        apple.transform.localPosition = new Vector3(0f, face.transform.localScale.y * 0.225f, 0.5f);
        apple.tag = "Apple";
        
    }
}