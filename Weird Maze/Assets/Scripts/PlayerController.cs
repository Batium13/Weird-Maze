using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2;
    private Animator anim;
    private Rigidbody myBody;
    float forceImpulse = 30f;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        FailCheck();
        SuccessCheck();
    }

    private void MoveForward()
    {
        if (!GameManager.gameManager.isGameOver && !GameManager.gameManager.isSuccess)
        {
            if (this.gameObject == GameManager.gameManager.playersList[0])
            {
                MoveDownQuick();
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                anim.SetBool("IsSuccess", false);
                anim.SetInteger("IsRunning", 1);
            }
            else
            {
                anim.SetInteger("IsRunning", 0);
            }
        }
        else if(GameManager.gameManager.isGameOver)
        {
            GameManager.gameManager.GameOver();
            anim.SetInteger("IsRunning", 0);
            anim.SetBool("IsCrush", true);
        }
        TransformCheck();
    }


    private void MoveDownQuick()
    {
        float downSpeed = 80f;
        RaycastHit hit;
        Ray downRay = new Ray(this.transform.position, Vector3.down);

        if (Physics.Raycast(downRay, out hit))
        {
            if (hit.distance > 1)
            {
                myBody.AddForce(Vector3.down * Time.deltaTime * downSpeed, ForceMode.Impulse);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube") && !GameManager.gameManager.isGameOver)
        {
            if(GameManager.gameManager.playersList[0].name == "Player" )
            {
                GameManager.gameManager.BecomePlayer();
                GameManager.gameManager.FollowerRemoveFromList();
            }
            else
            {
                GameManager.gameManager.currentPlayer = GameManager.gameManager.playersList[0];
                GameManager.gameManager.BecomePlayer();
                GameManager.gameManager.FollowerRemoveFromList();
            }
            StartCoroutine(FailedForce());
            StartCoroutine(StopForce());
            StartCoroutine(DestroyGameObject());
        }      
    }

    void FailCheck()
    {
        if (GameManager.gameManager.isGameOver)
        {
            anim.SetInteger("IsRunning", 0);
            anim.SetBool("IsCrush", true);
        }
    }
    void SuccessCheck()
    {
        if (GameManager.gameManager.isSuccess)
        {
            anim.SetInteger("IsRunning", 0);
            anim.SetBool("IsSuccess", true);
        }else
            anim.SetBool("IsSuccess", false);
    }

    void TransformCheck()
    {
        if(GameManager.gameManager.currentPlayer.transform.position.x > 0 || 
            GameManager.gameManager.currentPlayer.transform.position.x < 0)
        {
            GameManager.gameManager.currentPlayer.transform.position = new Vector3(0, 
                                                    GameManager.gameManager.currentPlayer.transform.position.y,
                                                    GameManager.gameManager.currentPlayer.transform.position.z);
        }
        if (this.gameObject.transform.position.z >= 300 && this.gameObject.transform.position.z <= -30)
        {
            GameManager.gameManager.isGameOver = true;
        }
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(2f);
        if(!GameManager.gameManager.isGameOver)
            this.gameObject.SetActive(false);
    }
    IEnumerator FailedForce()
    {
        yield return new WaitForSeconds(0);
        myBody.AddForce(new Vector3(0,3,-1) * forceImpulse * Time.deltaTime, ForceMode.Impulse);
        forceImpulse += 30;
    }
    IEnumerator StopForce()
    {
        yield return new WaitForSeconds(3f);
        forceImpulse = 0;
        myBody.GetComponent<Rigidbody>().isKinematic = true;
    }
}

    

