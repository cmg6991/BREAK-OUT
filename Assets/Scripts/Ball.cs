using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;

    public float launchSpeed = 15f;
    public float minY = -5.5f;
    public float maxVelocity = 15f;

    public Transform paddle;

    bool isLaunched = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    private void Update()
    {
        if (paddle == null)
            return;

        if (!isLaunched)
        {
            Vector3 pos = paddle.position;
            pos.y += 0.5f;
            transform.position = pos;

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
            {
                LaunchBall();
            }
#else
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                LaunchBall();
#endif
        }

        if (transform.position.y < minY)
        {
            StageManager.Instance.hpCount();
            ResetBall();
        }

        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void ResetBall()
    {
        isLaunched = false;
        rb.velocity = Vector3.zero;
    }

    void LaunchBall()
    {
        isLaunched = true;
        rb.velocity = Vector2.up * launchSpeed;
    }
}
