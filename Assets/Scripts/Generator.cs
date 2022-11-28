using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    // Block Prefab
    private GameObject block = null;
    public void LoadBlock() => block = Resources.Load<GameObject>("Block");
    // Buttons
    [Header("Buttons")]
    [SerializeField] private Button suffleButton = null;
    [SerializeField] private Button suffleHardButton = null;
    [SerializeField] private Button blankButton = null;
    [SerializeField] private Button answerButton = null;
    [SerializeField] private Button suffleMix = null;
    private void Awake()
    {
        // Load Resource
        LoadBlock();
        // Init Buttons
        suffleButton.onClick.AddListener(()=>ShuffleLine(10));
        suffleHardButton.onClick.AddListener(() => ShuffleHard(10));
        suffleMix.onClick.AddListener(() => ShuffleLine(10));
        suffleMix.onClick.AddListener(() => ShuffleHard(10));
        blankButton.onClick.AddListener(()=>MakeBlank(40));
        answerButton.onClick.AddListener(() => ShowAnswer());
        // Make Grid
        InitGrid();
        Win.SetActive(false);
    }
    private Color gray = new Color(0.65f, 0.65f, 0.65f);
    // 3 * 3 MAX
    private const int SQUARE_MAX = 3;
    // Row MAX
    private const int GRID_MAX = 9;
    // Temp variable
    private GameObject temp = null;
    // My gameobject blocks
    private TextMeshPro[,] blocks = new TextMeshPro[9, 9];
    // Tuple data
    private Tuple<int, int, int> first = new Tuple<int, int, int>(1, 2, 3);
    private Tuple<int, int, int> second = new Tuple<int, int, int>(4, 5, 6);
    private Tuple<int, int, int> last = new Tuple<int, int, int>(7, 8, 9);
    private Tuple<int, int, int>[,] grid = new Tuple<int, int, int>[GRID_MAX, SQUARE_MAX]; // Y, X
    // Make tuple data
    private void InitGrid()
    {
        for (int i = 0; i < GRID_MAX; i++) // Y
        {
            for (int j = 0; j < SQUARE_MAX; j++) // X
            {
                // Switch by 0, 1, 2 -> row(X) index
                switch (j)
                {
                    case 0:
                        {
                            grid[i, j] = first;
                            break;
                        }
                    case 1:
                        {
                            grid[i, j] = second;
                            break;
                        }
                    case 2:
                        {
                            grid[i, j] = last;
                            break;
                        }
                }
            }
            // Shuffle tuple data by current column line index
            SwitchTuple(i);
        }

        // Create block object and set number as text
        for (int i = 0; i < GRID_MAX; i++) // Y
        {
            for (int j = 0; j < GRID_MAX; j++) // X
            {
                temp = Instantiate(block, new Vector3(j, -i, 0), Quaternion.identity);
                if (IsGray(i, j))
                {
                    temp.GetComponent<Renderer>().material.color = gray;
                }
                // Rotate randomly
                temp.transform.Rotate(new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), Random.Range(-4f, 4f)));
                blocks[j, i] = temp.GetComponentInChildren<TextMeshPro>();
                // Set number from grid tuple
                // Calculate what is my number in my tuple
                if ((j + 1) % 3 == 1)
                    blocks[j, i].text = grid[i, j / 3].Item1.ToString();
                else if ((j + 1) % 3 == 2)
                    blocks[j, i].text = grid[i, j / 3].Item2.ToString();
                else if ((j + 1) % 3 == 0)
                    blocks[j, i].text = grid[i, j / 3].Item3.ToString();
                else Debug.Log("ERROR");
            }
        }
    }
    private bool IsGray(int i, int j)
    {
        if (j >= 3 && j < 6 && i / 3 == 0)
        {
            return true;
        }
        else if (j >= 0 && j < 3 && i / 3 == 1)
        {
            return true;
        }
        else if (j >= 6 && j < 9 && i / 3 == 1)
        {
            return true;
        }
        else if (j >= 3 && j < 6 && i / 3 == 2)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Shuffle tuple data
    /// </summary>
    /// <param name="line">Current line index</param>
    private void SwitchTuple(int line)
    {
        var temp1 = first;
        var temp2 = second;
        var temp3 = last;
        // Switch left to right
        first = temp2;
        second = temp3;
        last = temp1;
        // If current line number is 3 or 6, then change each tuple data to reorder number
        // (1, 2, 3) => (2, 3, 4)
        if (line + 1 == 3 || line + 1 == 6)
        {
            temp1 = first;
            temp2 = second;
            temp3 = last;
            // Re-order
            first = new Tuple<int, int, int>(temp1.Item2, temp1.Item3, temp2.Item1);
            second = new Tuple<int, int, int>(temp2.Item2, temp2.Item3, temp3.Item1);
            last = new Tuple<int, int, int>(temp3.Item2, temp3.Item3, temp1.Item1);
        }
    }
    /// <summary>
    /// Suffle number by reorder line
    /// </summary>
    /// <param name="count">How many times do I shuffle?</param>
    public void ShuffleLine(int count)
    {
        if (count == 0) return;

        int randomLine = Random.Range(0, 9);

        int switchingLine = 0;
        switch (randomLine)
        {
            case < 3:
                {
                    switchingLine = Random.Range(0, 3);
                    break;
                }
            case < 6:
                {
                    switchingLine = Random.Range(3, 6);
                    break;
                }
            case < 9:
                {
                    switchingLine = Random.Range(6, 9);
                    break;
                }
        }

        string temp;
        // Set shuffle method. By row or column?
        int rowOrCol = Random.Range(0, 2);

        for (int i = 0; i < GRID_MAX; i++)
        {
            // Shuffle by Row
            if (rowOrCol == 0)
            {
                // Origin <-> Selected : Row
                temp = blocks[i, randomLine].text;
                blocks[i, randomLine].text = blocks[i, switchingLine].text;
                blocks[i, switchingLine].text = temp;
            }
            // Shuffle by Column
            else
            {
                // Origin <-> Selected : Col
                temp = blocks[randomLine, i].text;
                blocks[randomLine, i].text = blocks[switchingLine, i].text;
                blocks[switchingLine, i].text = temp;
            }
        }
        // Recursive
        ShuffleLine(count - 1);
    }
    // Will switching list
    private List<TextMeshPro> originNum = new List<TextMeshPro>();
    private List<TextMeshPro> switchNum = new List<TextMeshPro>();
    // Will switching line index
    private int origin = 0;
    private int switching;
    // Pair data
    private (int, int)[] pairs = { (1, 9), (2, 8), (3, 7), (4, 6), (6, 4), (7, 3), (8, 2), (9, 1) };
    /// <summary>
    /// Shuffle number by switching each number pair.
    /// </summary>
    /// <param name="difficulty">How many times do I shuffle?</param>
    public void ShuffleHard(int difficulty)
    {
        originNum.Clear();
        switchNum.Clear();
        if (difficulty == 0) return;
        // Choose Number
        origin = Random.Range(1, 10);
        while (origin == 5)
        {
            origin = Random.Range(1, 10);
        }
        // Find Pair Number
        for (int i = 0; i < pairs.Length; i++)
        {
            if (pairs[i].Item1 == origin)
            {
                switching = pairs[i].Item2;
            }
        }
        // Find Objects which i want to shuffle
        for (int i = 0; i < GRID_MAX; i++)
        {
            for (int j = 0; j < GRID_MAX; j++)
            {
                // Find origin number
                if (string.Compare(blocks[i, j].text, origin.ToString()) == 0)
                {
                    originNum.Add(blocks[i, j]);
                }
                // Find target number from pair value by origin
                else if (string.Compare(blocks[i, j].text, switching.ToString()) == 0)
                {
                    switchNum.Add(blocks[i, j]);
                }
            }
        }
        // Switch
        string temp;
        for(int i = 0; i < originNum.Count; i++)
        {
            temp = originNum[i].text;
            originNum[i].text = switchNum[i].text;
            switchNum[i].text = temp;
        }
        // Recursive
        ShuffleHard(difficulty - 1);
    }
    // Coordinates
    private int rndX;
    private int rndY;
    private Stack<TextMeshPro> solutions = new Stack<TextMeshPro>();
    /// <summary>
    /// Create blank to play game
    /// </summary>
    /// <param name="difficulty">How many blanks do I create?</param>
    public void MakeBlank(int difficulty)
    {
        if (difficulty == 0)
        {
            ShuffleLine(3);
            ShuffleHard(3);
            return;
        }

        rndX = Random.Range(0, 9);
        rndY = Random.Range(0, 9);
        blocks[rndX, rndY].alpha = 0f;
        solutions.Push(blocks[rndX, rndY]);
        MakeBlank(difficulty - 1);
    }
    [SerializeField] private GameObject Win = null;
    /// <summary>
    /// Check answer and show.
    /// </summary>
    public void ShowAnswer()
    {
        if (solutions.Count == 0) return;

        bool flag = true;
        TextMeshPro temp;
        TextMeshPro input;

        while (solutions.Count != 0)
        {
            temp = solutions.Pop();
            temp.alpha = 1f;
            input = temp.transform.GetChild(0).GetComponent<TextMeshPro>();

            // If wrong answer input
            if (string.Compare(temp.text, input.text) != 0)
            {
                Debug.LogError("Wrong");
                flag = false;
                temp.color = Color.red;
                input.alpha = 0f;
            }
        }

        if (flag)
        {
            Win.SetActive(true);
        }
    }
}
