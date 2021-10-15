using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireballSkill: Skill
{
    public Fireball fireball_prefab;
    public GameObject after_effect;

    public override void Init(Character character)
    {
        this.mage = character;
    }

    public override void Activate(GameObject target)
    {
        this.mage.TurnTo(target.gameObject);

        Fireball fireball = Instantiate(fireball_prefab, mage.transform);
        fireball.transform.parent = null;
        fireball.effect = after_effect;
        fireball.mage = this.mage;
        fireball.damage = damage;

        Vector3 magic_force = mage.transform.forward * speed;
        fireball.GetComponent<Rigidbody>().velocity = magic_force;
    }
}
