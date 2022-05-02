using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCastle : MonoBehaviour
{
    bool Ended = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Ended)
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<Firework>().Explode();
            Ended = true;
        }
    }
}
