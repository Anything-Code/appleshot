using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private AudioSource hittingSound;

    public static Action ArrowHit;

    private void OnTriggerEnter(Collider other)
    {
        ArrowHit?.Invoke();
        if (other.gameObject.CompareTag("Arrow"))
        {
            hittingSound.Play();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        hittingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
