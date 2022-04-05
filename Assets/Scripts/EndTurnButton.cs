using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public GameMaster gameMaster;

    public void EndOfTurn()
    {
        gameMaster.ChangePlayer();
    }
}
