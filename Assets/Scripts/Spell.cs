using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    private float xSpeed = 0f;
    private float ySpeed = 0f;
    private float lifetime = 0f;
    private float damage;
    private string sourceTag; // Character's type that created the bullet


    public void SetSpeed(float x, float y, float speed, int range, float damage)
    {
        this.lifetime = (float)range / 7.5f; // 1 / 7.5 seconds is almost a unit in the world
        this.damage = damage;
        StartCoroutine(DestroyBullet());

        if (x != 0 && y != 0)
        {
            this.xSpeed = Mathf.Sin(Mathf.PI / 4) * speed * x;
            this.ySpeed = Mathf.Sin(Mathf.PI / 4) * speed * y;
        } else
        {
            this.xSpeed = x != 0 ? speed * x : 0;
            this.ySpeed = y != 0 ? speed * y : 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector2 position = transform.position;
        position.x += xSpeed;
        position.y += ySpeed;
        transform.position = position;
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        // A bullet cannot deny another
        if (!collision.gameObject.CompareTag("bullet") &&
            !collision.gameObject.CompareTag(sourceTag)) {
            Destroy(gameObject);
                collision.transform.gameObject.GetComponent<CharacterAttribute>().TakeDamage(damage);
        }
    }

    public void SetSource(string tag) => this.sourceTag = tag;

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
