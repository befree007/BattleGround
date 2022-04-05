using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum Players
{
    Player1,
    Player2,
    None
}

public enum GameState
{
    WaitForMove,
    Move,
    WaitForAttack,
    Attack
}

public class GameMaster : MonoBehaviour
{
    public BattleField battleField;
    public Character warrior;
    public Character archer;
    public Character wizard;
    public Character currentCharacter;
    private List<Vector2Int> selectedCells;

    public FieldMoveEvent fieldMoveEvent = new FieldMoveEvent();
    public ChangeTurnEvent changeTurnEvent = new ChangeTurnEvent();
    public WinOrLoseEvent winOrLoseEvent = new WinOrLoseEvent();

    public Text textTurnPlayer;

    public Color colorPlayer1;
    public Color colorPlayer2;

    public List<Character> player1Characters = new List<Character>();
    public List<Character> player2Characters = new List<Character>();

    public Players currentPlayer = Players.Player1;
    public GameState currentState = GameState.WaitForMove;


    void Start()
    {
        CreateCharacter(Instantiate(warrior), battleField.PlaceCharacter(Players.Player1), Players.Player1, colorPlayer1);
        CreateCharacter(Instantiate(archer), battleField.PlaceCharacter(Players.Player1), Players.Player1, colorPlayer1);
        CreateCharacter(Instantiate(wizard), battleField.PlaceCharacter(Players.Player1), Players.Player1, colorPlayer1);

        CreateCharacter(Instantiate(warrior), battleField.PlaceCharacter(Players.Player2), Players.Player2, colorPlayer2);
        CreateCharacter(Instantiate(archer), battleField.PlaceCharacter(Players.Player2), Players.Player2, colorPlayer2);
        CreateCharacter(Instantiate(wizard), battleField.PlaceCharacter(Players.Player2), Players.Player2, colorPlayer2);
    }

    public void OnCharacterClicked(Cell cell)
    {
        if (currentState == GameState.WaitForMove && cell.character.owner == currentPlayer && cell.character.moveEnd == false && currentCharacter == null)
        {
            currentCharacter = cell.character;

            List<Vector2Int> moves = currentCharacter.PossibleMoves(cell.fieldPosition, battleField.fieldSize, currentCharacter.moveDistance);

            selectedCells = battleField.SelectCells(moves, Color.gray, false);

            if (selectedCells.Count == 0)
            {
                currentState = GameState.WaitForAttack;
            }
            else
            {
                currentState = GameState.Move;
            }
        }

        if (currentState == GameState.WaitForAttack && currentCharacter == cell.character)
        {
            List<Vector2Int> attacks = currentCharacter.PossibleAttacks(cell.fieldPosition, battleField.fieldSize, currentCharacter.rangeAttack);

            List<Vector2Int> possibleAttacks = new List<Vector2Int>();

            for (int i = 0; i < attacks.Count; i++)
            {
                if (battleField.cells[attacks[i].x, attacks[i].y].character != null)
                {
                    if (battleField.cells[attacks[i].x, attacks[i].y].character.owner != currentPlayer)
                    {
                        possibleAttacks.Add(attacks[i]);
                    }                    
                }
            }

            selectedCells = battleField.SelectCells(possibleAttacks, Color.red, true);

            if (selectedCells.Count == 0)
            {
                currentCharacter.moveEnd = true;

                if (CanPlayerMove(currentPlayer) == false)
                {
                    ChangePlayer();                    
                }

                selectedCells = null;
                currentCharacter = null;
                currentState = GameState.WaitForMove;
            }
            else
            {
                currentState = GameState.Attack;
            }
        }

        if (currentState == GameState.Attack)
        {
            if (cell.selected == true)
            {
                if (cell.character != null)
                {
                    int health = cell.character.TakeDamage(currentCharacter.GetDamage());
                    fieldMoveEvent.Invoke(new FieldMove(cell.character, currentCharacter.transform.parent.GetComponent<Cell>().fieldPosition, cell.fieldPosition, currentPlayer, currentState));
                }

                battleField.DeSelectCells(selectedCells);
                currentCharacter.moveEnd = true;                

                if (CanPlayerMove(currentPlayer) == false)
                {
                    ChangePlayer();
                }

                selectedCells = null;
                currentCharacter = null;
                currentState = GameState.WaitForMove;                
            }

            winOrLoseEvent.Invoke(CheckWin());

            //Debug.Log($"Выиграл игрок - {CheckWin()}");
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (currentState == GameState.Move)
        {
            if (cell.selected == true)
            {
                fieldMoveEvent.Invoke(new FieldMove(currentCharacter, currentCharacter.transform.parent.GetComponent<Cell>().fieldPosition, cell.fieldPosition, currentPlayer, currentState));
                MoveCharacter(cell, currentCharacter);
                battleField.DeSelectCells(selectedCells);
                selectedCells = null;                

                List<Vector2Int> attacks = currentCharacter.PossibleAttacks(cell.fieldPosition, battleField.fieldSize, currentCharacter.rangeAttack);

                List<Vector2Int> possibleAttacks = new List<Vector2Int>();

                for (int i = 0; i < attacks.Count; i++)
                {
                    if (battleField.cells[attacks[i].x, attacks[i].y].character != null)
                    {
                        if (battleField.cells[attacks[i].x, attacks[i].y].character.owner != currentPlayer)
                        {
                            possibleAttacks.Add(attacks[i]);
                        }
                    }
                }

                if (possibleAttacks.Count == 0)
                {
                    currentCharacter.moveEnd = true;

                    if (CanPlayerMove(currentPlayer) == false)
                    {
                        ChangePlayer();
                    }

                    selectedCells = null;
                    currentCharacter = null;
                    currentState = GameState.WaitForMove;
                }
                else
                {
                    currentState = GameState.WaitForAttack;
                    OnCharacterClicked(cell);
                }
            }
        }

        Debug.Log($"CellsClicked {cell.fieldPosition.x}, {cell.fieldPosition.y}");
    }

    public Players CheckWin()
    {
        int counter1 = 0;

        for (int i = 0; i < player1Characters.Count; i++)
        {
            if (player1Characters[i].alive == false)
            {
                counter1++;
            }
        }

        if (counter1 == player1Characters.Count)
        {
            return Players.Player2;
        }

        int counter2 = 0;

        for (int i = 0; i < player2Characters.Count; i++)
        {
            if (player2Characters[i].alive == false)
            {
                counter2++;
            }
        }

        if (counter2 == player2Characters.Count)
        {
            return Players.Player1;
        }

        return Players.None;
    }

    public void MoveCharacter(Cell cell, Character character)
    {
        character.transform.parent.GetComponent<Cell>().character = null;
        cell.SetCharacter(character);
    }

    public void PositionCell(int i, int j)
    {
        Debug.Log($"{i}, {j}");
    }

    public void InitCellsEvents()
    {
        for (int i = 1; i < battleField.cells.GetLength(0); i++)
        {
            for (int j = 1; j < battleField.cells.GetLength(1); j++)
            {
                battleField.cells[i, j].cellClicked.AddListener(OnCellClicked);
                battleField.cells[i, j].characterClicked.AddListener(OnCharacterClicked);
            }            
        }
    }

    public void CreateCharacter(Character character, Vector2Int position, Players playerSide, Color color)
    {
        character.owner = playerSide;

        if (battleField.cells[position.x, position.y].character == null)
        {
            battleField.cells[position.x, position.y].SetCharacter(character);

            character.GetComponent<SpriteRenderer>().color = color;

            if (playerSide == Players.Player1)
            {
                player1Characters.Add(character);
            }
            else
            {
                player2Characters.Add(character);
            }
        }
        else
        {
            CreateCharacter(character, battleField.PlaceCharacter(playerSide), playerSide, color);
        }
    }

    public bool CanPlayerMove(Players player)
    {
        List<Character> listCharacter;
        int counter = 0;

        if (player == Players.Player1)
        {
            listCharacter = player1Characters;
        }
        else
        {
            listCharacter = player2Characters;
        }

        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (listCharacter[i].moveEnd == true || listCharacter[i].alive == false)
            {
                counter++;
            }
        }

        if (counter == listCharacter.Count)
        {
            return false;
        }

        return true;
    }

    public void ChangePlayer()
    {
        if (currentPlayer == Players.Player1)
        {
            currentPlayer = Players.Player2;
            SetCharactersCanMove(player2Characters);
            textTurnPlayer.text = "Turn: BluePlayer";
        }
        else
        {
            currentPlayer = Players.Player1;
            SetCharactersCanMove(player1Characters);
            textTurnPlayer.text = "Turn: WhitePlayer";
        }

        changeTurnEvent.Invoke(currentPlayer);
    }

    public void SetCharactersCanMove(List<Character> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].moveEnd = false;
        }
    }

}
