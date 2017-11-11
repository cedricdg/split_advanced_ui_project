using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChargeOnHandAngle : MonoBehaviour
{

    public string TagWithCalculateYVelocity = "Handpalm";
    const int NUMBER_OF_HANDS = 1;

    public float ScaleGrowth = 0.01f;
    public float MinScale = 0.1f;
    public float MaxScale = 1f;
    public Transform ball;
    public Transform Bullet;
    public bool IsCharging;
    public float CooldownTime = 5f;

    private float LastShotTime;

    public bool IsFullyCharged
    {
        get
        {
            return ball.localScale.y >= MaxScale - 0.01f;
        }
        set
        {
            var newScale = value ? MaxScale : MinScale;
            ball.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("Fire1") && IsFullyCharged)
        {
            ReleaseBall();
            IsFullyCharged = false;
        }
    }

    private void ReleaseBall()
    {
        var newBullet = Instantiate(
            Bullet,
            ball.position,
            ball.rotation
        );
        var hands = GameObject.FindGameObjectsWithTag(TagWithCalculateYVelocity);
        newBullet.position = Vector3.Lerp(hands.First().transform.position, hands.Last().transform.position, 0.5f);
        newBullet.position += new Vector3(0f, 0.1f, 0f);
        newBullet.gameObject.SetActive(true);

        LastShotTime = Time.time;
    }

    private void FixedUpdate()
    {
        var hands = GameObject.FindGameObjectsWithTag(TagWithCalculateYVelocity);
        SetObjectCharging(hands);

        if (IsCharging && Time.time > LastShotTime + CooldownTime)
        {
            ball.localScale += new Vector3(ScaleGrowth, ScaleGrowth, ScaleGrowth);
            if (!ball.gameObject.activeSelf)
                ball.gameObject.SetActive(true);

            ball.position = Vector3.Lerp(hands.First().transform.position, hands.Last().transform.position, 0.5f);
            ball.position += new Vector3(0f, ball.localScale.y + 0.1f, 0f);
        }
        else if (ball.gameObject.activeSelf)
        {
            ball.localScale -= new Vector3(ScaleGrowth * 2, ScaleGrowth * 2, ScaleGrowth * 2);
        }

        if (ball.localScale.y < MinScale)
        {
            ball.gameObject.SetActive(false);
        }

        if (ball.localScale.y > MaxScale)
        {
            ball.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
        }
    }

    private void SetObjectCharging(GameObject[] hands)
    {
        var horizontalHands = hands.Where((h) => h.GetComponent<HandIsHorizontal>().IsHorizontal);
        if (horizontalHands.Count() == NUMBER_OF_HANDS)
        {
            Debug.Log("Valid Number of Hands");
            IsCharging = true;
        }
        else
        {
            IsCharging = false;
        }
    }
}
