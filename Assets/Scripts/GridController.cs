using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GridController : MonoBehaviour {
    
    public int rows = 4;
    public int columns = 14;
    public int gameToPlay;
    public float xSeparation = 1.11f;
    public float ySeparation = 1.51f;
    public float xStart = -5f;
    public float yStart = -0.2f;
    public AudioClip incorrectGuessSound;
    public AudioClip correctGuessSound;
    public AudioClip puzzleRevealedSound;
    public AudioClip puzzleSolvedSound;
    public GameObject coverTilePrefab;
    public GameObject letterTilePrefab;
    public GameObject categoryText;
    public GameObject[] letterDisplays;
    public GameLayout[] gameLayouts;

    private Vector3[] gridPositions;
    private List<GameObject> coverTiles;
    private List<GameObject> letterTiles;

    [Serializable]
    public class GameLayout
    {
        public String category;
        public String[] layout;
    }

    public void ResetGame()
    {
        GameLayout game = gameLayouts[gameToPlay];

        for (int i = 0; i < game.layout.Length; i++)
        {
            Destroy(letterTiles[i]);
        }

        for (int x = 0; x < letterDisplays.Length; x++)
        {
            letterDisplays[x].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }

        letterTiles.Clear();

        if (gameToPlay < gameLayouts.Length - 1)
        {
            gameToPlay++;
        }
        else
        {
            gameToPlay = 0;
        }

        StartCoroutine(PlaySound(puzzleRevealedSound, 0.0f));
        SetCategory();
        CreateGame(letterTilePrefab);
    }

    void Start () {

        coverTiles = new List<GameObject>();
        letterTiles = new List<GameObject>();
        SetGridPositions();
        SetCategory();
        CreateGrid(coverTilePrefab);
        CreateGame(letterTilePrefab);
    }

    void SetGridPositions()
    {
        gridPositions = new Vector3[(rows * columns)];

        float xPos = xStart;
        float yPos = yStart;

        int count = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((x == 0 && y == 0) || (x == 13 && y == 0) || (x == 0 && y == 3) || (x == 13 && y == 3))
                {
                    gridPositions[count] = Vector3.zero;
                }
                else
                {
                    gridPositions[count] = new Vector3(xPos, yPos, 0.0f);
                }

                yPos += ySeparation;
                count++;
            }

            yPos = yStart;
            xPos += xSeparation;
        }
    }

    void SetCategory()
    {
        GameLayout game = gameLayouts[gameToPlay];

        categoryText.GetComponent<Text>().text = game.category;
    }

    void CreateGrid(GameObject obj)
    {
        int count = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Debug.Log("Tile " + count + ": " + gridPositions[count]);

                if (gridPositions[count] != Vector3.zero)
                {
                    Debug.Log("Initialized Tile " + count);
                    CreateObject(obj, gridPositions[count]);
                }

                count++;
            }
        }
    }

    void CreateObject(GameObject obj, Vector3 pos)
    {
        GameObject tile = Instantiate(obj, pos, Quaternion.identity) as GameObject;
        coverTiles.Add(tile);
    }

    void CreateGame(GameObject obj)
    {
        GameLayout game = gameLayouts[gameToPlay];

        for (int i = 0; i < game.layout.Length; i++)
        {
            if (!game.layout[i].Equals(""))
            {
                Vector3 pos = gridPositions[i];
                pos += new Vector3(0, 0, -1);
                GameObject tile = Instantiate(obj, pos, Quaternion.identity) as GameObject;
                letterTiles.Add(tile);
                tile.GetComponentInChildren<MeshRenderer>().enabled = false;
                tile.GetComponentInChildren<TextMesh>().text = game.layout[i];
            }
            else
            {
                letterTiles.Add(null);
            }
        }
    }

    void CheckLetters()
    {
        if (IsSolved())
        {
            return;
        }

        GameLayout game = gameLayouts[gameToPlay];
        
        for (int i = 0; i < game.layout.Length; i++)
        {
            if (!game.layout[i].Equals(""))
            {
                if (Input.GetButtonDown(game.layout[i]))
                {
                    letterTiles[i].GetComponentInChildren<MeshRenderer>().enabled = true;
                    
                    if (IsSolved())
                    {
                        StartCoroutine(PlaySound(correctGuessSound, 0.0f));
                        StartCoroutine(PlaySound(puzzleSolvedSound, 1.0f));
                    }
                }
            }
        }
    }

    void CheckLetterDisplays()
    {
        for (int i = 0; i < letterDisplays.Length; i++)
        {
            if (Input.GetKeyDown(letterDisplays[i].GetComponentInChildren<Text>().text.ToLower()))
            {
                letterDisplays[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

                if (!IsSolved())
                {
                    if (IsGuessCorrect(i))
                    {
                        StartCoroutine(PlaySound(correctGuessSound, 0.0f));
                    }
                    else
                    {
                        StartCoroutine(PlaySound(incorrectGuessSound, 0.0f));
                    }
                }
            }
        }
    }

    bool IsGuessCorrect(int index)
    {
        GameLayout game = gameLayouts[gameToPlay];

        for (int i = 0; i < game.layout.Length; i++)
        {
            if (game.layout[i].Equals(letterDisplays[index].GetComponentInChildren<Text>().text))
            {
                return true;
            }
        }

        return false;
    }

    void ShowBoard()
    {
        GameLayout game = gameLayouts[gameToPlay];

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            for (int i = 0; i < game.layout.Length; i++)
            {
                if (!game.layout[i].Equals(""))
                {
                    letterTiles[i].GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }

            StartCoroutine(PlaySound(puzzleSolvedSound, 0.0f));
        }
    }

    bool IsSolved()
    {
        GameLayout game = gameLayouts[gameToPlay];

        for (int i = 0; i < letterTiles.Count; i++)
        {
            if (letterTiles[i] != null &&
                !letterTiles[i].GetComponentInChildren<MeshRenderer>().enabled)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator PlaySound(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

	void Update () {
        CheckLetters();
        CheckLetterDisplays();
        ShowBoard();
    }
}
