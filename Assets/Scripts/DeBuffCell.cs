using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffCell : Cell
{
    public override void SetCharacter(Character newCharacter)
    {
        newCharacter.transform.SetParent(this.transform);
        newCharacter.transform.localPosition = new Vector3(0, 0, -5f);
        newCharacter.currentAttack = newCharacter.baseAttack - 1;
        character = newCharacter;
    }
}
