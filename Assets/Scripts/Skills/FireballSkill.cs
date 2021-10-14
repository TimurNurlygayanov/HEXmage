using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Fireball")]
public class FireballSkill: Skill
{
    public Fireball fireball_prefab;

    public override void Init(Character character)
    {
        this.mage = character;
    }

    public override void Activate(PathNode target)
    {
        this.mage.TurnTo(target.gameObject);

        Fireball fireball = Instantiate(fireball_prefab, mage.transform);
        fireball.transform.parent = null;

        Vector3 magic_force = mage.transform.forward * speed;
        fireball.GetComponent<Rigidbody>().velocity = magic_force;
    }
}
