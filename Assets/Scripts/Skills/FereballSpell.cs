using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FereballSpell : Skill
{
    public Fireball fireball_prefab;
    public float speed = 100;

    public override void Activate()
    {
        Fireball fireball = Instantiate(fireball_prefab, mage.transform);
        fireball.GetComponent<Rigidbody>().AddForce(mage.transform.forward * speed);
    }
}
