using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Actor"))
        {
            GameManager.instance.suicides++;
            if (collision.gameObject.GetComponent<AI>().state == AI.State.Thief)
            {
                AI.thiefExists = false;
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Lose();
        }
    }
}
