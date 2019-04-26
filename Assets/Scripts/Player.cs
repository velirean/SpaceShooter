using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : CharacterAttribute
{
    public Text healtText;
    
    public void Stop() => GetComponent<Rigidbody2D>().velocity = Vector3.zero;

    protected override void Die()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
    }

    private void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (horizontalMove != 0 || verticalMove != 0)
        {
            GetComponent<Rigidbody2D>().velocity = NormalizeSpeed(horizontalMove, verticalMove);
        }

        Vector3 position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, 0f, 16f), Mathf.Clamp(transform.position.y, 1f, 9f));
        gameObject.transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(rightShootPoint, Vector2.right);
        }

        healtText.text = "Vida " + life;

    }

}
