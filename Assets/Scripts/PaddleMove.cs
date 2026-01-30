using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PaddleMove : MonoBehaviour
{
    public float minX = -7.5f;
    public float maxX = 7.5f;

    Vector3 startPos;
    Camera cam;

    public bool canMove = false;

    PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        startPos = transform.position;
        cam = Camera.main;

        Ball ball = FindObjectOfType<Ball>();
        if (ball != null)
            ball.paddle = this.transform;
    }

    //좌우로 움직이는 패들
    private void Update()
    {
        if (!pv.IsMine)
            return;

        if (!canMove)
            return;
        Vector3 paddlePos = transform.position;

#if UNITY_EDITOR || UNITY_STANDALONE
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        paddlePos.x = mousePos.x;
#else
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = cam.ScreenToWorldPoint(touch.position);
            paddlePos.x = touchPos.x;
        }
#endif
        paddlePos.x = Mathf.Clamp(paddlePos.x, minX, maxX);
        transform.position = paddlePos;
    }

    public void ResetPaddle()
    {
        transform.position = startPos;
    }
}
