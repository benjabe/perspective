using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Camera[] cameras;
    [SerializeField] GameObject youreFiredGO;
    [SerializeField] GameObject promotedGO;
    [SerializeField] GameObject sun;
    [SerializeField] Text thanksText;
    [SerializeField] Text altF4Text;
    int currentCameraIndex = 0;

    public int score = 0;
    public int suicides = 0;

    bool end = false;
    float timeSinceEnd = 0.0f;

    private void Awake()
    {
        youreFiredGO.SetActive(false);
        SetActiveCamera(cameras[currentCameraIndex]);
        if (instance == null)
        {
            instance = this;
        }
        sun.SetActive(false);
        promotedGO.SetActive(false);
    }

    void Update()
    {
        if (score >= 5)
        {
            print("you won");
            promotedGO.SetActive(true);
        }
        if (end)
        {
            timeSinceEnd += Time.deltaTime;
            if (timeSinceEnd >= 23.1f)
            {
                thanksText.color = new Color(
                   thanksText.color.r,
                   thanksText.color.g,
                   thanksText.color.b,
                   Mathf.Clamp(thanksText.color.a + 0.125f * Time.deltaTime, 0.0f, 1.0f)
               );
           }
           if (timeSinceEnd >= 38.0f)
           {
                altF4Text.color = new Color(
                        thanksText.color.r,
                        thanksText.color.g,
                        thanksText.color.b,
                        Mathf.Clamp(altF4Text.color.a + 0.125f * Time.deltaTime, 0.0f, 1.0f)
                    );
            }
        }
    }

    public void Lose()
    {
        print("you lose");
        youreFiredGO.SetActive(true);
    }

    public void SetActiveCameraToNext()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Length)
        {
            currentCameraIndex = 0;
        }
        SetActiveCamera(cameras[currentCameraIndex]);

    }

    public void SetActiveCamera(Camera camera)
    {
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        camera.gameObject.SetActive(true);
    }

    public Camera GetActiveCamera()
    {
        return cameras[currentCameraIndex];
    }

    public void ZoomOut()
    {
        end = true;
        Camera.main.transform.parent = null;
        Camera.main.GetComponent<Animator>().SetBool("ZoomOut", true);
        sun.SetActive(true);
        GetComponent<AudioSource>().Play();
    }
}
