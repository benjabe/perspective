using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] Transform sunTransform;
    float distanceFromSun;

    private void Start()
    {
        distanceFromSun = (sunTransform.position - transform.position).magnitude;
    }

    private void Update()
    {
        transform.position = sunTransform.position + new Vector3(
            Mathf.Cos(Time.time / distanceFromSun * 6.0f),
            0,
            Mathf.Sin(Time.time / distanceFromSun * 6.0f)
        ) * distanceFromSun;
    }
}
