using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public Character mage;

    public float speed = 0f;
    public float damage = 0f;

    public abstract void Init(Character character);

    public abstract void Activate(PathNode target);
}
