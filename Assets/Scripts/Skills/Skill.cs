using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public Character mage;

    public float speed = 0f;
    public int damage = 0;

    public abstract void Init(Character character);

    public abstract void Activate(GameObject target);
}
