using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] float speed = 13.2f;
    [SerializeField] List<Vector3> stops;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip lift;
    [SerializeField] AudioClip pling;

    Vector3 targetStop;

    bool playedPling = true;

    private void Awake()
    {
        stops.Insert(0, transform.position);
        for (int i = 1; i < stops.Count; i++)
        {
            stops[i] += transform.position;
        }
        targetStop = stops[0];
        audioSource.Stop();
    }

    private void Update()
    {
        Vector3 distance = targetStop - transform.position;
        float distanceToMove = Time.deltaTime * speed;
        if (distanceToMove > distance.magnitude)
        {
            distanceToMove = distance.magnitude;
            if (!playedPling)
            {
                audioSource.clip = pling;
                audioSource.loop = false;
                audioSource.Play();
                playedPling = true;
            }   
        }
        transform.Translate(distance.normalized * distanceToMove);
    }

    public void PlayLiftSound()
    {
        audioSource.clip = lift;
        audioSource.loop = true;
        audioSource.Play();
        playedPling = false;
    }

    public void GoToStop(int stopIndex)
    {
        targetStop = stops[stopIndex];
    }

    private void OnDrawGizmos()
    {
        Vector3 from = stops[0];
        foreach (Vector3 stop in stops)
        {
            Vector3 totalStopPosition = stop;
            Gizmos.color = Color.red;
            Vector3 direction = totalStopPosition - from;
            Gizmos.DrawRay(from, direction);
            direction = Vector3.right;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector3(
                totalStopPosition.x - 0.5f,
                totalStopPosition.y,
                totalStopPosition.z
            ), direction);
            direction = Vector3.forward;
            Gizmos.DrawRay(new Vector3(
                totalStopPosition.x,
                totalStopPosition.y,
                totalStopPosition.z - 0.5f
            ), direction);
            from = totalStopPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
