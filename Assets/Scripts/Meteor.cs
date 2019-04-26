using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : CharacterAttribute
{
    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        position -= new Vector3(speed, 0);
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
        }
    }
}
