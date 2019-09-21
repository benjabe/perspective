using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivityX = 250f;
    [SerializeField] private float mouseSensitivityY = 250f;
    [SerializeField] private float speed = 6.0f;
    [SerializeField] LayerMask interactionMask;
    [SerializeField] float interactionDistance = 3.4f;
    [SerializeField] Lift lift;
    [SerializeField] Text debugText;

    Rigidbody rb;
    [SerializeField] Camera cam;
    Transform cameraTransform;

    Vector3 movementDirection = Vector3.zero;
    float verticalLookRotation;

    bool canMove = true;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        cameraTransform = cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ?
                CursorLockMode.Locked :
                CursorLockMode.None;
        }
        
        movementDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            if (Physics.Raycast(
                cameraTransform.position,
                cameraTransform.TransformDirection(Vector3.forward),
                out hit,
                interactionDistance))
            {
                //debugText.text = hit.transform.tag;
                switch(hit.transform.tag)
                {
                    case "Door":
                        Destroy(hit.transform.gameObject);
                        break;
                    case "LiftButton":
                        lift.GoToStop(int.Parse(hit.transform.name));
                        lift.PlayLiftSound();
                        break;
                    case "Monitor":
                        GameManager.instance.SetActiveCameraToNext();
                        break;
                    case "Actor":
                        if (hit.transform.GetComponent<AI>().state == AI.State.Thief)
                        {
                            GameManager.instance.score++;
                            Destroy(hit.transform.gameObject);
                            AI.thiefExists = false;
                        }
                        else
                        {
                            GameManager.instance.Lose();
                            Destroy(hit.transform.gameObject);
                        }
                        GetComponent<AudioSource>().Play();
                        break;
                    case "Notice":
                        canMove = false;
                        GameManager.instance.ZoomOut();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!canMove) return;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
            verticalLookRotation +=+ Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
            //verticalLookRotation = Mathf.Clamp(verticalLookRotation, -89.0f, 89.0f);
            cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
            if (cameraTransform.localEulerAngles.x - 360.0f > 91.0f) cameraTransform.localEulerAngles = Vector3.right * 89.0f;
            if (cameraTransform.localEulerAngles.x < -89.0f) cameraTransform.localEulerAngles = Vector3.right * -89.0f;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.TransformDirection(movementDirection) * speed * Time.fixedDeltaTime);
    }
}
