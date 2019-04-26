using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : CharacterAttribute
{
    private int verticalDirection = 0;

    private new void Awake()
    {
        base.Awake();
        StartCoroutine(Attack());
        StartCoroutine(RandomVerticalMove(Random.Range(0f, 1.5f)));
    }

    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        position -= new Vector3(speed, speed * 0.5f * verticalDirection);
        transform.position = position;
        if (transform.position.x < -2)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.gameObject.GetComponent<CharacterAttribute>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        Shoot(leftShootPoint, Vector3.left);
        StartCoroutine(Attack());
    }

    IEnumerator RandomVerticalMove(float time)
    {
        yield return new WaitForSeconds(time);
        verticalDirection = Random.Range(-1, 2);
        StartCoroutine(RandomVerticalMove(Random.Range(0f, 1.5f)));
    }
}
