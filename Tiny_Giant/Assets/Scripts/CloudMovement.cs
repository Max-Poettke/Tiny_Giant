using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] private Vector3 reappearPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float driftTime;
    private Vector3 _initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _initialPosition = transform.position;
        StartCoroutine(DriftCloud());
    }

    private IEnumerator DriftCloud()
    {
        var time = 0f;
        while (time < driftTime)
        {
            var t = time / driftTime;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(_initialPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        _initialPosition = reappearPosition;
        StartCoroutine(DriftCloud());
    }
}
