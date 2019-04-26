using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAttribute : MonoBehaviour
{
    // Easier balancing of attributes using the editor
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float maxLife;
    [SerializeField]
    public int spellRange; // In world units
    // Life is set to maxLife at the beginning of the game
    protected float life;
    private Color characterColor;
    protected readonly float spellSpeed = 0.145f; // This sets the corret value to calculate the range

    public Animator animator;
    public GameObject spell;
    public GameObject rightShootPoint;
    public GameObject leftShootPoint;
    public GameObject upShootPoint;
    public GameObject downShootPoint;
    public AudioClip Damage1;
    public AudioClip Damage2;
    public AudioClip DestroySound;
    public AudioClip Fire1;
    public AudioClip Fire2;
    public AudioClip FireMultiple1;
    public AudioClip FireMultiple2;
    public int audiosource;

    protected void Awake()
    {
        characterColor = gameObject.GetComponent<SpriteRenderer>().color;
        life = maxLife;
    }

    // Player dies in a different way
    protected virtual void Die()
    {
        Destroy(gameObject);
    }


    public void ChangeLife(float increment)
    {
        life += increment;
        maxLife += increment;
    }

    public void Heal(float restored) => life += restored;
    public void DealDamage(GameObject target) => target.GetComponent<CharacterAttribute>().TakeDamage(damage);

    public void TakeDamage(float damage)
    {
        StartCoroutine(RestoreColor());
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        life -= damage;
        if (life <= 0) Die();
    }

    public IEnumerator RestoreColor()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = characterColor;
    }

    public void IsBehindWall()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
    }

    protected void Shoot(GameObject shootPoint, params Vector2[] directions)
    {

        List<GameObject> previousShots = new List<GameObject>();

        foreach (Vector2 direction in directions)
        {
            GameObject shot = Instantiate(spell, shootPoint.GetComponent<Transform>().position, Quaternion.Euler(new Vector3(0, 0, -90f))) as GameObject;
            shot.GetComponent<Spell>().SetSource(gameObject.tag);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            foreach (GameObject shot2 in previousShots)
            {
                Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(), shot2.GetComponent<Collider2D>());
            }
            previousShots.Add(shot);
            shot.GetComponent<Spell>().SetSpeed(StandarizeDirection(direction.x), StandarizeDirection(direction.y), this.spellSpeed, this.spellRange, this.damage);   
        }
    }

    protected void Shoot(params Vector2[] directions)
    {
        // These define spell rate between shots


        List<GameObject> previousShots = new List<GameObject>();

        foreach (Vector2 direction in directions)
        {
            GameObject shot = (GameObject)Instantiate(spell, SelectShotOrigin(direction).GetComponent<Transform>().position, Quaternion.identity);
            shot.GetComponent<Spell>().SetSource(gameObject.tag);
            Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            foreach (GameObject shot2 in previousShots)
            {
                Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(), shot2.GetComponent<Collider2D>());
            }
            previousShots.Add(shot);
            shot.GetComponent<Spell>().SetSpeed(StandarizeDirection(direction.x), StandarizeDirection(direction.y), this.spellSpeed, this.spellRange, this.damage);
        }
    }

    protected float StandarizeDirection(float direction)
    {
        return direction > 0 ? 1 : direction < 0 ? -1 : 0;
    }

    private GameObject SelectShotOrigin(Vector2 direction)
    {
        if ((direction.x != 0.0f && direction.y != 0.0f) || (direction.x != 0.0f && direction.y == 0.0f))
        {
            return direction.x > 0.0f ? rightShootPoint : leftShootPoint;
        }
        else if (direction.x == 0.0f && direction.y != 0.0f)
        {
            return direction.y > 0.0f ? upShootPoint : downShootPoint;
        }
        else
        {
            return upShootPoint;
        }
    }

    protected Vector2 NormalizeSpeed(float x, float y)
    {
        float xSpeed;
        float ySpeed;
        if (x != 0 && y != 0)
        {
            xSpeed = Mathf.Sin(Mathf.PI / 4) * speed * x;
            ySpeed = Mathf.Sin(Mathf.PI / 4) * speed * y;
        }
        else
        {
            xSpeed = x != 0 ? speed * x : 0;
            ySpeed = y != 0 ? speed * y : 0;
        }

        return new Vector2(xSpeed, ySpeed);
    }


}
