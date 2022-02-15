using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int health;
    public int attack;
    public int rangeAttack;
    public int moveDistance;
    public bool moveEnd = false;
    public bool alive = true;
    public List<Vector2Int> patternMoves;
    public List<Vector2Int> patternAttacks;

    public GameMaster gameMaster;

    public Players owner;

    public void Awake()
    {
        InitPatternMoves();
    }

    public virtual int GetDamage()
    {
        return attack;
    }

    public virtual int TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.parent.GetComponent<Cell>().character = null;
            alive = false;
        }

        return health;
    }

    public virtual void InitPatternMoves()
    {
        patternMoves = new List<Vector2Int>();

        patternMoves.Add(new Vector2Int(-1, 1));
        patternMoves.Add(new Vector2Int(0, 1));
        patternMoves.Add(new Vector2Int(1, 1));
        patternMoves.Add(new Vector2Int(1, 0));
        patternMoves.Add(new Vector2Int(1, -1));
        patternMoves.Add(new Vector2Int(0, -1));
        patternMoves.Add(new Vector2Int(-1, -1));
        patternMoves.Add(new Vector2Int(-1, 0));
    }

    public bool CheckInsideField(Vector2Int position, Vector2Int sizeField)
    {
        if (position.x > 0 && position.x < sizeField.x + 1)
        {
            if (position.y > 0 && position.y < sizeField.y + 1)
            {
                return true;
            }
        }

        return false;
    }

    public virtual List<Vector2Int> PossibleMoves(Vector2Int position, Vector2Int sizeField)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        for (int i = 0; i < patternMoves.Count; i++)
        {
            for (int n = 0; n < moveDistance + 1; n++)
            {
                Vector2Int newMove = position + patternMoves[i] + patternMoves[i] * n;

                if (CheckInsideField(newMove, sizeField) == true)
                {
                    moves.Add(newMove);
                }
            }
        }

        return moves;
    }

    public virtual List<Vector2Int> PossibleAttacks(Vector2Int position, Vector2Int sizeField)
    {
        return PossibleMoves(position, sizeField);
    }

}
