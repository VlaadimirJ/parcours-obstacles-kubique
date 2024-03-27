using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce;
    bool canJump = true;
    public bool hasTriggeredScore = false;
    public bool hasTriggeredNuke = false;
    public bool hasLost = false;
    GameManager gameManager;

    private Vector2 touchStartPos;
    private float touchStartTime;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float touchDuration = Time.time - touchStartTime;
                float swipeDeltaX = touch.position.x - touchStartPos.x;

                if (swipeDeltaX <= 0.2f && swipeDeltaX>=-0.2f)
                {
                    if (canJump)
                    {
                        Jump();
                    }
                }
                else if (swipeDeltaX > 0)
                {
                    MoveRight();
                }
                else if (swipeDeltaX < 0)
                {
                    MoveLeft();
                }
            }
        }
    }
    private void MoveLeft()
    {
        if (transform.position.x == 0f)
        {

            Vector3 newPosition = new Vector3(-1.5f, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }

        if (transform.position.x == 1.5f)
        {

            Vector3 newPosition = new Vector3(0f, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void MoveRight()
    {
        if (transform.position.x == 0f)
        {

            Vector3 newPosition = new Vector3(1.5f, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }

        if (transform.position.x == -1.5f)
        {

            Vector3 newPosition = new Vector3(0f, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

 
    public IEnumerator ResetFlagAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        hasTriggeredScore = false;
        hasTriggeredNuke = false;
        hasLost = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            hasLost = true;
            StartCoroutine(ResetFlagAfterDelay());
        }

        if (other.gameObject.tag == "Score")
        {
            hasTriggeredScore = true;
            StartCoroutine(ResetFlagAfterDelay());
        }
        if (other.gameObject.tag == "Nuke")
        {
            hasTriggeredNuke = true;
            StartCoroutine(ResetFlagAfterDelay());
        }
    }
}
