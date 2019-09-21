using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public enum State
    {
        Normal,
        Thief
    }
    [SerializeField] GameObject suspiciousGO;
    [SerializeField] Transform playerTransform;
    public State state = State.Normal;
    float speed = 8.22f;
    float moveDir;
    float timeToMoveInCurrentDir;
    Rigidbody rb;
    public static bool thiefExists = false;

    private void Awake()
    {
        suspiciousGO.SetActive(false);
        SelectMoveDir();
        rb = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().material.color = new Color(
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            Random.Range(0.0f, 1.0f),
            1.0f
        );
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                timeToMoveInCurrentDir -= Time.deltaTime;
                if (timeToMoveInCurrentDir <= 0.0f)
                {
                    SelectMoveDir();
                }
                
                break;
            case State.Thief:
                if (playerTransform.position.y > -3)
                {
                    suspiciousGO.SetActive(true);
                    suspiciousGO.transform.LookAt(GameManager.instance.GetActiveCamera().transform);
                    suspiciousGO.transform.Rotate(Vector3.up, 180f);
                }
                else
                {
                    suspiciousGO.SetActive(false);
                }
                break;
            default:
                break;
        }

        Vector3 movement = new Vector3(Mathf.Cos(moveDir), 0, Mathf.Sin(moveDir));
        rb.MovePosition(transform.position + movement * Time.deltaTime * speed);
    }

    void SelectMoveDir()
    {
        moveDir = Random.Range(0f, 2 * Mathf.PI);
        timeToMoveInCurrentDir = Random.Range(0.5f, 8.0f);
        if (!thiefExists && Random.Range(0, 40) == 0)
        {
            state = State.Thief;
            thiefExists = true;
        }
    }
}
