using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Character> characters;
    public int active_character = 0;

    public Character GetCharacter()
    {
        return characters[active_character];
    }
}
