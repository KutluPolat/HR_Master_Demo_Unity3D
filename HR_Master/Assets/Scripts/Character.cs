using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Characters thisCharacter;

    private void Start()
    {
        thisCharacter = new Characters(); // Creates a new character specialities
    }
}
