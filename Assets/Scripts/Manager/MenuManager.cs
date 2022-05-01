using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    private bool FirstLevel = false;



    void Start()
    {

        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
        playerAnimator.SetFloat("speed", moveInput.sqrMagnitude);

        if (Input.GetButtonDown("Horizontal"))
        {
            //playerAnimator.SetFloat("speed", 1);
        }

        if (Input.GetButtonDown("Vertical"))
        {
            //playerAnimator.SetFloat("speed", 1);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            //playerAnimator.SetFloat("speed", 0);
        }

        if (Input.GetButtonUp("Vertical"))
        {
            //playerAnimator.SetFloat("speed", 0);
        }
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + moveInput * speed * Time.fixedDeltaTime);

        if (Input.GetKey(KeyCode.E) && FirstLevel)
        {
            SceneManager.LoadScene(1);

        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level1"))
        {
            FirstLevel = true;
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level1"))
        {
            FirstLevel = false;
        }
    }
}
