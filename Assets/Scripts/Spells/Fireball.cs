using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject effect;
    public Character mage;
    public int damage;

    public void Awake()
    {
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != mage.gameObject)
        {
            Debug.Log(collision.gameObject + "  " + collision.gameObject.tag);

            if (collision.gameObject.tag == "Character")
            {
                Character enemy = collision.gameObject.GetComponent<Character>();
                enemy.GetDamage(damage);
            }

            GameObject after_effect = Instantiate(effect, collision.transform);
            after_effect.transform.parent = collision.gameObject.transform;
            Destroy(after_effect, 120);

            Destroy(gameObject, 0.2f);  // add effect of hit the target

            after_effect = Instantiate(effect, collision.transform);
            after_effect.transform.parent = collision.gameObject.transform;
            after_effect.transform.position += new Vector3(0f, 1f, 0f);
            Destroy(after_effect, 120);
        }
    }
}
