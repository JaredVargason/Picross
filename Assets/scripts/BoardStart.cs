using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardStart : MonoBehaviour {

    public Board currentBoard;
    public GameObject rowPanel;
    public GameObject columnPanel;
    public GameObject gridPanel;

    void Awake () {
        currentBoard = GameObject.Find("MapBundle").GetComponent<MapBundle>().board;
        rowPanel = GameObject.Find("RowsPanel");
        columnPanel = GameObject.Find("ColumnsPanel");
        gridPanel = GameObject.Find("GridPanel");
    }

	// Use this for initialization
	void Start () {
		for (int i = 0; i < currentBoard._size; i++) {
            GameObject column = Instantiate(Resources.Load("GridColumnPrefab")) as GameObject;
            column.transform.SetParent(gridPanel.transform, false);
            for (int j = 0; j < currentBoard._size; j++) {
                BoardSpace space = (Instantiate(Resources.Load("GridSpacePrefab")) as GameObject).GetComponent<BoardSpace>();
                space._row = j;
                space._column = i;
                space.transform.SetParent(column.transform, false);

                space.GetComponent<Button>().onClick.AddListener(delegate {
                    if (currentBoard.checkColumnFilled(space._column) &&
                    currentBoard.checkRowFilled(space._row)) {
                        currentBoard.checkWin();
                    }
                });
            }

        }

        for (int i = 0; i < currentBoard._size; i++) {
            GameObject row = Instantiate(Resources.Load("RowNumbersPrefab")) as GameObject;
            row.transform.SetParent(rowPanel.transform);
            row.transform.GetChild(0).GetComponent<Text>().text = currentBoard.formatListHorizontal(currentBoard._rowNums[i]);
            
            GameObject column = Instantiate(Resources.Load("ColumnNumbersPrefab")) as GameObject;
            column.transform.SetParent(columnPanel.transform);
            column.transform.GetChild(0).GetComponent<Text>().text = currentBoard.formatListVertical(currentBoard._columnNums[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
