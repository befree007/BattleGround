using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct FieldMove
{
    public Character character;
    public Vector2Int fromPosition;
    public Vector2Int toPosition;
    public Players player;
    public GameState state;

    //public DamageText damageText;
    

    public FieldMove(Character aCharacter, Vector2Int firstPosition, Vector2Int secondPosition, Players newPlayer, GameState gameState)
    {
        character = aCharacter;
        fromPosition = firstPosition;
        toPosition = secondPosition;
        player = newPlayer;
        state = gameState;
    }

    public override string ToString()
    {
        return $"{character}, {fromPosition}, {toPosition}, {player}, {state}";
    }
}

public class FieldMoveEvent : UnityEvent<FieldMove>
{

}

public class ChangeTurnEvent: UnityEvent<Players>
{

}

public class WinOrLoseEvent: UnityEvent<Players>
{

}

public class DamageTaken: UnityEvent<int>
{

}

public class GameEvent : MonoBehaviour
{
    public GameMaster gameMaster;

    public void Start()
    {
        gameMaster.fieldMoveEvent.AddListener(GetEvent);
        gameMaster.changeTurnEvent.AddListener(ChangeTurn);
        gameMaster.winOrLoseEvent.AddListener(CheckWinOrLose);
    }

    public void GetEvent(FieldMove fieldMove)
    {
        Debug.Log(fieldMove);
    }

    public void ChangeTurn(Players player)
    {
        Debug.Log($"Now turn player - {player}");
    }

    public void CheckWinOrLose(Players player)
    {
        Debug.Log($"Выиграл игрок - {player}");
    }

    //public void DamageText(int damage)
    //{
    //    damageText.SetDamage(damage);
    //}

}

