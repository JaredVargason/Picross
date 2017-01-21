using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CustomBoard {

    public string _name;
    public int _size;
    public BoardSpace[,] _squares;
    
    public CustomBoard(int size) {
        _size = size;
        _squares = new BoardSpace[_size,_size];
    }

    public void markSquare(int column, int row) {
        _squares[column,row]._value = !_squares[column, row]._value;
    }

    public List<int> getRowArray(int row) {
        List<int> rowArray = new List<int>();
        int count = 0;
        for (int i = 0; i < _size; i++) {
            if (_squares[i, row]._value) {
                count++;
            }

            else if (count > 0) {
                rowArray.Add(count);
                count = 0;
            }
        }

        if (count > 0) {
            rowArray.Add(count);
        }

        if (rowArray.Count == 0) {
            rowArray.Add(0);
        }
        
        return rowArray;
    }

    public List<int> getColumnArray(int column) {
        List<int> columnArray = new List<int>();
        int count = 0;
        for (int i = 0; i < _size; i++) {
            if (_squares[column, i]._value) {
                count++;
            }

            else if (count > 0) {
                columnArray.Add(count);
                count = 0;
            }
        }

        if (count > 0) {
            columnArray.Add(count);
        }

        if (columnArray.Count == 0) {
            columnArray.Add(0);
        }

        return columnArray;
    }

    public List<List<int>> getAllRowArrays() {
        List<List<int>> rows = new List<List<int>>();
        for (int i = 0; i < _size; i++) {
            rows.Add(getRowArray(i));
        }

        return rows;
    }

    public List<List<int>> getAllColumnArrays() {
        List<List<int>> cols = new List<List<int>>();
        for (int i = 0; i < _size; i++) {
            cols.Add(getColumnArray(i));
        }

        return cols;
    }

    public string formatListHorizontal(List<int> list) {
       
        if (list.Count == 0) {
            return "0";
        }
        
        else if (list.Count == 1) {
            return "" + list[0];
        }
        
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Count - 1; i++) {
            sb.Append(list[i] + " ");
        }

        sb.Append(list[list.Count - 1]);
        
        return sb.ToString();
    }

    public string formatListVertical(List<int> list) {
        if (list.Count == 0) {
            return "0";
        }

        else if (list.Count == 1) {
            return "" + list[0];
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Count - 1; i++) {
            sb.Append(list[i] + "\n\n");
        }

        sb.Append(list[list.Count - 1]);

        return sb.ToString();
    }

    private static List<List<int>> rows;
    private static List<List<int>> cols;
    private static int[,] colVal, colIx;
    private static long[] mask, val;
    private static long[] grid;
    private static long[][] rowPerms;

    public bool isSolvable() {
        rowPerms = new long[_size][];
        grid = new long[_size];
        rows = getAllRowArrays();
        cols = getAllColumnArrays();

        for (int r = 0; r < _size; r++) {
            LinkedList<long> res = new LinkedList<long>();
            int spaces = _size - (rows[r].Count - 1);

            for (int i = 0; i < rows[r].Count; i++) {
                spaces -= rows[r][i];
            }

            calcPerms(r, 0, spaces, 0, 0, res);

            if (res.Count == 0) {
                return false;
            }

            rowPerms[r] = new long[res.Count];
            while (res.Count > 0) {
                rowPerms[r][res.Count - 1] = res.Last.Value;
                res.RemoveLast();
            }
        }

		//Calculate
		colVal = new int[_size,_size];
		colIx = new int[_size, _size];
		mask = new long[_size];
		val = new long[_size];

		if (dfs(0)) {
			return true;
		}

		else {
			Debug.Log("No solution found");
			return false;
		}
    }

    private void calcPerms(int r, int cur, int spaces, long perm, int shift, LinkedList<long> res) {
        if (cur == rows[r].Count) {
            if ((grid[r] & perm) == grid[r]) {
                res.AddLast(perm);
            }
            return;
        }
        while (spaces >= 0) {
            calcPerms(r, cur + 1, spaces, perm | (bits(rows[r][cur]) << shift), shift + rows[r][cur] + 1, res);
            shift++;
            spaces--;
        }
    }        

    private bool dfs(int row) {
        if (row == _size) {
            return true;
        }
        rowMask(row); // calculate mask to stay valid in the next row
        for (int i = 0; i < rowPerms[row].Length; i++) {
            if ((rowPerms[row][i] & mask[row]) != val[row]) {
                continue;
            }
            grid[row] = rowPerms[row][i];
            updateCols(row);
            if (dfs(row + 1)) {
                return true;
            }
        }
        return false;
    }

    private void rowMask(int row) {
        mask[row] = val[row] = 0;
        if (row == 0) {
            return;
        }
        long ixc = 1L;
        for (int c = 0; c < _size; c++, ixc <<= 1) {
            if (colVal[row - 1,c] > 0) {
                // when column at previous row is set, we know for sure what has to be the next bit according to the current size and the expected size
                mask[row] |= ixc;
                Debug.Log(colIx[row - 1, c]);
                Debug.Log(cols[c].Count);
                if (cols[c][colIx[row - 1,c]] > colVal[row - 1,c]) {
                    val[row] |= ixc; // must set
                }
            }
            else if (colVal[row - 1,c] == 0 && colIx[row - 1,c] == cols[c].Count) {
                // can not add anymore since out of indices
                mask[row] |= ixc;
            }
        }
    }

    private void updateCols(int row) {
        long ixc = 1L;
        for (int c = 0; c < _size; c++, ixc <<= 1) {
            // copy from previous
            colVal[row,c] = row == 0 ? 0 : colVal[row - 1,c];
            colIx[row,c] = row == 0 ? 0 : colIx[row - 1,c];
            if ((grid[row] & ixc) == 0) {
                if (row > 0 && colVal[row - 1,c] > 0) {
                    // bit not set and col is not empty at previous row => close blocksize
                    colVal[row,c] = 0;
                    colIx[row,c]++;
                }
            }
            else {
                colVal[row,c]++; // increase value for set bit
            }
        }
    }

    private long bits(int b) {
        return (1L << b) - 1;
    }
    override
    public string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(_name + "\n");
        sb.Append(_size + "\n");

        
        sb.Append(formatListForSave(getAllRowArrays()));
        sb.Append(formatListForSave(getAllColumnArrays()));
        
        /*for (int i = 0; i < _squares.Length; i++) {
            for (int j = 0; j < _squares.Length; j++) {
                sb.Append(_squares[j, i]._value ? "1" : "0");
            }
            sb.Append('\n');
        }*/

        return sb.ToString();
    }

    public string getBoardInfoString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(_name + "\n");
        sb.Append(_size + "\n");
        sb.Append(0 + "\n");
        sb.Append(0 + "\n");
        return sb.ToString();
    }

    public string formatListForSave(List<List<int>> bigList) {
        StringBuilder sb = new StringBuilder();
        foreach (List<int> list in bigList) {
            foreach (int i in list) {
                sb.Append(i + " ");
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append('\n');
        }
        sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }

    public void saveBoard() {
        if (!Directory.Exists(Application.persistentDataPath + "/boards/")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/boards/");
        }

        Debug.Log("nice");
        string fileName = Application.persistentDataPath + "/" + _name + ".txt";
        Debug.Log(fileName);
        StreamWriter fileWriter = File.CreateText(fileName);
        fileWriter.Write(ToString());
        fileWriter.Close();
        Debug.Log("cool");
        
        //if (!File.Exists(_name)) {
        //File.WriteAllText(Application.persistentDataPath + "/boards/" + _name + ".txt", ToString());
        //
    }

    public void saveBoardInfo() {
        /*string fileName = Application.persistentDataPath + "/boardinfo/" + _name;
        StreamWriter fileWriter = File.CreateText(fileName);
        fileWriter.WriteLine(getBoardInfoString());
        fileWriter.Close();*/
    }

    /*BOARD INFO IS SAVED IN THE FOLLOWING FORMAT:
    name
    size
    completed (1 or 0)
    unlocked (1 or 0)
    difficulty (put in manually)
    
    /*BOARDS ARE SAVED IN THE FOLLOWING FORMAT:
    name
    size
    (size) row nums
    (size) column nums
    */
    
}
