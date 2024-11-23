using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public GameObject[] tilePrefabs;
    public GameObject[,] tiles;
    public GameObject selectedTile;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        tiles = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateTile(x, y);
            }
        }
        while (CheckForMatchesAndRemove().Count > 0) { }
    }

    void CreateTile(int x, int y)
    {
        int randomIndex;
        do {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        } while (x > 1 && y > 1 && tiles[x-1,y].name == tiles[x-2,y].name && tiles[x-1,y].name == tilePrefabs[randomIndex].name && tiles[x,y-1].name == tiles[x,y-2].name && tiles[x,y-1].name == tilePrefabs[randomIndex].name);

        GameObject tile = Instantiate(tilePrefabs[randomIndex], new Vector2(x, y), Quaternion.identity);
        tile.name = $"Tile {x} {y}";
        tiles[x, y] = tile;
    }

    List<GameObject> CheckForMatchesAndRemove()
    {
        List<GameObject> matches = FindMatches();
        foreach (GameObject match in matches)
        {
            Destroy(match);
        }
        RefillBoard();
        return matches;
    }

    List<GameObject> FindMatches()
    {
        List<GameObject> matches = new List<GameObject>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                if (tiles[x, y] && tiles[x + 1, y] && tiles[x + 2, y] &&
                    tiles[x, y].name == tiles[x + 1, y].name && tiles[x, y].name == tiles[x + 2, y].name)
                {
                    matches.Add(tiles[x, y]);
                    matches.Add(tiles[x + 1, y]);
                    matches.Add(tiles[x + 2, y]);
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                if (tiles[x, y] && tiles[x, y + 1] && tiles[x, y + 2] &&
                    tiles[x, y].name == tiles[x, y + 1].name && tiles[x, y].name == tiles[x, y + 2].name)
                {
                    matches.Add(tiles[x, y]);
                    matches.Add(tiles[x, y + 1]);
                    matches.Add(tiles[x, y + 2]);
                }
            }
        }
        return matches;
    }

    void RefillBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (tiles[x, y] == null) {
                    CreateTile(x,y);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (selectedTile == null)
                {
                    selectedTile = hit.collider.gameObject;
                }
                else
                {
                    SwapTiles(selectedTile, hit.collider.gameObject);
                    selectedTile = null;
                }
            }
        }
    }

    void SwapTiles(GameObject tileA, GameObject tileB)
    {
        int xA = (int)tileA.transform.position.x;
        int yA = (int)tileA.transform.position.y;
        int xB = (int)tileB.transform.position.x;
        int yB = (int)tileB.transform.position.y;
        if (Mathf.Abs(xA - xB) + Mathf.Abs(yA - yB) != 1) {
            return;
        };

        GameObject temp = tiles[xA, yA];
        tiles[xA, yA] = tiles[xB, yB];
        tiles[xB, yB] = temp;

        tileA.transform.position = new Vector2(xB, yB);
        tileB.transform.position = new Vector2(xA, yA);

        while (CheckForMatchesAndRemove().Count > 0) { }
    }
}