using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CellEvent : UnityEvent <Cell>
{

}

public class Cell : MonoBehaviour
{
    public TextMesh debagTextIndex;
    public Character character;
    public GameObject highLight;
    public CellEvent cellClicked;
    public CellEvent characterClicked;
    public bool selected;    

    public Vector2Int fieldPosition;

    void Awake()
    {
        if (cellClicked == null)
        {
            cellClicked = new CellEvent();
        }

        if (characterClicked == null)
        {
            characterClicked = new CellEvent();
        }
            
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Click");

            if (character != null)
            {
                characterClicked.Invoke(this);
            }
            else
            {
                cellClicked.Invoke(this);
            }
        }
    }

    public virtual bool SelectUp(Color color, bool selectCharacter = true)
    {
        if (character == null && selectCharacter == false)
        {
            selected = true;
            GetComponent<SpriteRenderer>().color = color;
            return true;
        }
        else if(selectCharacter == true)
        {
            selected = true;
            GetComponent<SpriteRenderer>().color = color;
            return true;
        }

        return false;
    }

    public virtual void SelectDown()
    {
        selected = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void SetCharacter(Character newCharacter)
    {        
        newCharacter.transform.SetParent(this.transform);
        newCharacter.transform.localPosition = new Vector3(0,0,-5f);
        newCharacter.currentAttack = newCharacter.baseAttack;
        newCharacter.baseHealth = newCharacter.currentHealth;
        character = newCharacter;
    }

    public void OnMouseEnter()
    {
        highLight.SetActive(true);
    }

    public void OnMouseExit()
    {
        highLight.SetActive(false);
    }
}
