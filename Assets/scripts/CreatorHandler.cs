using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorHandler : MonoBehaviour {

    GameObject _setupPanel;
    GameObject _creatorPanel;
    GameObject _gridPanel;
    GameObject _rowsPanel;
    GameObject _columnsPanel;

    InputField nameText;

    //GameObject _colorPanel;
    CustomBoard board;
    Slider sizeSlider;
    Text sizeText;
    Button continueButton;
    Button finishButton;
    Button solveButton;
    int size;

    void Awake() {
        
        _setupPanel = GameObject.Find("SetupPanel");
        _creatorPanel = GameObject.Find("CreatorPanel");
        _gridPanel = GameObject.Find("GridPanel");
        _rowsPanel = GameObject.Find("RowsPanel");
        _columnsPanel = GameObject.Find("ColumnsPanel");

        nameText = GameObject.Find("BoardNameInputField").GetComponent<InputField>();
        

        solveButton = GameObject.Find("SolveButton").GetComponent<Button>();
        finishButton = GameObject.Find("FinishButton").GetComponent<Button>();

        solveButton.onClick.AddListener(delegate {
            if (board.isSolvable()) {
                finishButton.gameObject.SetActive(true);
                solveButton.transform.GetChild(0).GetComponent<Text>().text = "Solvable";
            }

            else {
                solveButton.transform.GetChild(0).GetComponent<Text>().text = "Not solvable";
            }
        });

        finishButton.onClick.AddListener( delegate {
            board.saveBoard();
            board.saveBoardInfo();
            
        });

        finishButton.gameObject.SetActive(false);
        _creatorPanel.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        /*_firstPanel = GameObject.Find("SetupPanel");
        _secondPanel = GameObject.Find("CreatorPanel");
        _gridPanel = GameObject.Find("GridPanel");*/
        sizeText = GameObject.Find("SizeText").GetComponent<Text>();
        sizeSlider = GameObject.Find("SizeSlider").GetComponent<Slider>();
        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        continueButton.onClick.AddListener(delegate { onContinueSelect(); });
        sizeSlider.minValue = 8;
        sizeSlider.maxValue = 25;
        sizeSlider.value = 8;
        sizeSlider.onValueChanged.AddListener(delegate { setSizeText(); });
        sizeSlider.wholeNumbers = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setSizeText() {
        sizeText.text = sizeSlider.value + "x" + sizeSlider.value;
    }

    void onContinueSelect() {
        size = (int)sizeSlider.value;
        board = new CustomBoard(size);
        board._name = nameText.text;
        board._size = size;
        _setupPanel.SetActive(false);
        _creatorPanel.SetActive(true);

        BoardSpace[,] spaces = new BoardSpace[size, size];

        for (int i = 0; i < size; i++) {
            VerticalLayoutGroup column = (Instantiate(Resources.Load("GridColumnPrefab")) as GameObject).GetComponent<VerticalLayoutGroup>();
            column.transform.SetParent(_gridPanel.transform, false);
            for (int j = 0; j < size; j++) {
                BoardSpace space = (Instantiate(Resources.Load("GridSpacePrefab")) as GameObject).GetComponent<BoardSpace>();
                                
                space._row = j;
                space._column = i;
                spaces[i, j] = space;
                space.board = this;
                space.transform.SetParent(column.transform, false);
                space.GetComponent<Button>().onClick.AddListener( delegate {

                    
                    
                    if (space._value) {
                        space.transform.GetChild(0).GetComponent<Text>().text = "";
                    }

                    else {
                        space.transform.GetChild(0).GetComponent<Text>().text = "X";
                    }
                    space.toggle();

                    updateRowAndColumnPanel(space._row, space._column);
                    finishButton.gameObject.SetActive(false);
                });
            }
        }

        board._squares = spaces;

        for (int i = 0; i < size; i++) {
            GameObject row = Instantiate(Resources.Load("RowNumbersPrefab")) as GameObject;
            row.transform.SetParent(_rowsPanel.transform, false);
            GameObject col = Instantiate(Resources.Load("ColumnNumbersPrefab")) as GameObject;
            col.transform.SetParent(_columnsPanel.transform, false);
        }

        /* Make previous panel invisible, and current panel visible.
        Get square prefab.
        Instantiate row and column markers.
        Instantiate NxN squares.
        */
    }

    public void updateRowAndColumnPanel(int row, int column) {
        _rowsPanel.transform.GetChild(row).GetChild(0).GetComponent<Text>().text = board.formatListHorizontal(board.getRowArray(row));
        _columnsPanel.transform.GetChild(column).GetChild(0).GetComponent<Text>().text = board.formatListVertical(board.getColumnArray(column));
    }
}
