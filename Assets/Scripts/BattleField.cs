using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public Cell[,] cells;

    public float cellOffset;
    public Vector2Int fieldSize;

    public List<GameObject> cellPrefabs;
    public Transform camera;
    public GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[fieldSize.x + 1, fieldSize.y + 1];
        CreateBattleField();
        gameMaster.InitCellsEvents();
    }

    public void CreateBattleField()
    {
        Vector3 currentCellPosition = new Vector3(0,0, transform.position.z);        

        for (int i = 1; i < fieldSize.x + 1; i++)
        {
            for (int j = 1; j < fieldSize.y + 1; j++)
            {
                int rndIndexCellsLandScape = Random.Range(1, 3);
                int rndIndexCellsBuffDeBuff = Random.Range(3, cellPrefabs.Count);
                int rndCounerAnotherCells = Random.Range(1, 11);

                if (rndCounerAnotherCells == 1)
                {                    
                    CreateCell(new Vector2Int(i, j), currentCellPosition, cellPrefabs[rndIndexCellsLandScape]);
                }
                else if (rndCounerAnotherCells == 2 && i >= 3 && i <= 6)
                {
                    CreateCell(new Vector2Int(i, j), currentCellPosition, cellPrefabs[rndIndexCellsBuffDeBuff]);
                }
                else 
                {
                    CreateCell(new Vector2Int(i, j), currentCellPosition, cellPrefabs[0]);
                }
                currentCellPosition.Set(currentCellPosition.x + cellOffset, currentCellPosition.y, transform.position.z);
            }
            currentCellPosition.Set(0, currentCellPosition.y - cellOffset, transform.position.z);
        }

        camera.transform.position = new Vector3((float)fieldSize.y / 2* cellOffset - cellOffset/2, -((float)fieldSize.x / 2* cellOffset) + cellOffset/2, -10f);
        camera.GetComponent<Camera>().orthographicSize = fieldSize.y * cellOffset/2 + cellOffset;
    }

    public void CreateCell(Vector2Int fieldPosition, Vector3 cellPosition, GameObject cell)
    {
        GameObject instance = Instantiate(cell, cellPosition, new Quaternion(0, 0, 0, 0));
        instance.name = $"{fieldPosition.x}, {fieldPosition.y}";
        cells[fieldPosition.x, fieldPosition.y] = instance.GetComponent<Cell>();
        cells[fieldPosition.x, fieldPosition.y].debagTextIndex.text = $"{fieldPosition.x},{fieldPosition.y}";
        cells[fieldPosition.x, fieldPosition.y].fieldPosition = fieldPosition;
        instance.transform.SetParent(this.transform);
        cellPosition.Set(cellPosition.x + cellOffset, cellPosition.y, transform.position.z);
    }

    public List<Vector2Int> SelectCells(List<Vector2Int> positions, Color color, bool selectCharacter)
    {
        List<Vector2Int> selected = new List<Vector2Int>(); 

        for (int i = 0; i < positions.Count; i++)
        {
            if(cells[positions[i].x, positions[i].y].SelectUp(color, selectCharacter) == true)
            {
                selected.Add(new Vector2Int(positions[i].x, positions[i].y));
            }            
        }

        return selected;
    }

    public void DeSelectCells(List<Vector2Int> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            cells[positions[i].x, positions[i].y].SelectDown();
        }
    }

    public Vector2Int PlaceCharacter(Players players)
    {
        int xPosition;
        int yPosition;

        if (players == Players.Player1)
        {
            xPosition = Random.Range(1, 3);
            yPosition = Random.Range(1, 9);

            Vector2Int newPlace = new Vector2Int(xPosition, yPosition);

            if (cells[newPlace.x, newPlace.y].SelectUp(Color.white) == false)
            {
                return PlaceCharacter(players);
            }

            return newPlace;
        }
        else if (players == Players.Player2)
        {
            xPosition = Random.Range(7, 9);
            yPosition = Random.Range(1, 9);

            Vector2Int newPlace = new Vector2Int(xPosition, yPosition);

            if (cells[newPlace.x, newPlace.y].SelectUp(Color.white) == false)
            {
                return PlaceCharacter(players);
            }

            return newPlace;
        }

        Vector2Int nullPlace = new Vector2Int(0, 0);

        return nullPlace;
    }
}
