using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Board {

    public static string BOARD_SAVE_PATH = "/boards/";
    public static string BOARD_INFO_PATH = "/boardinfo/";

    public enum troolean {
        no, yes, maybe
    };

    private String _name;
    public int _size;
    private troolean[,] _playerBoard;
    private bool[] _correctRows;
    private bool[] _correctColumns;
    //private bool[,] _correctBoard;
    public int[][] _columnNums;
    public int[][] _rowNums;
    

    public Board(String name, int size, bool completed, bool unlocked) {
        _size = size;
        _playerBoard = new troolean[size, size];
        _correctRows = new bool[size];
        _correctColumns = new bool[size];
        _columnNums = new int[_size][];
        _rowNums = new int[_size][];
        
        for (int i = 0; i < _playerBoard.GetLength(0); i++) {
            for (int j = 0; j < _playerBoard.GetLength(1); j++) {
                _playerBoard[i, j] = troolean.maybe;
            }
        }
    }

    /*public bool isCorrectBoard() {
        for (int i = 0; i < _playerBoard.GetLength(0); i++) {
            for (int j = 0; j < _playerBoard.GetLength(1); j++) {
                if (!compareBoolToTrool(_playerBoard[i,j],_correctBoard[i,j])) {
                    return false;
                }
            }
        }

        return true;
    }*/

    public bool compareBoolToTrool(troolean t, bool b) {
        troolean i = b ? troolean.yes : troolean.maybe;
        if (t == troolean.no) t = troolean.maybe;
        return i == t;
    }

    public bool checkColumnFilled(int column) {
        _correctColumns[column] = Enumerable.SequenceEqual(getColumnArray(column), _columnNums[column]);
        return _correctColumns[column];
    }

    public bool checkRowFilled(int row) {
        _correctRows[row] = Enumerable.SequenceEqual(getRowArray(row), _rowNums[row]);
        return _correctRows[row];
    }

    public bool checkWin() {
        for (int i = 0; i < _size; i++) {
            if (!(_correctColumns[i] && _correctRows[i])) {
                return false;
            }
        }

        return true;
    }

    public List<int> getRowArray(int row) {
        List<int> rowArray = new List<int>();
        int count = 0;
        for (int i = 0; i < _size; i++) {
            if (_playerBoard[i, row] == troolean.yes) {
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
            if (_playerBoard[column, i] == troolean.yes) {
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

    override
    public String ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(_name + '\n');
        sb.Append(_size + '\n');
        //int completed = _completed ? 0 : 1;
        //sb.Append(completed + '\n');
        
        for (int i = 0; i < _size; i++) {
            for (int j = 0; j < _size; j++) {
                sb.Append(_playerBoard[i, j]);
            }
            sb.Append('\n');
        }

        /*for (int i = 0; i < _size; i++) {
            for (int j = 0; j < _size; j++) {
                sb.Append(_correctBoard[i, j] ? 1 : 0);
            }
            sb.Append('\n');
        }*/

        for (int i = 0; i < _columnNums.GetLength(0); i++) {
            int[] column = _columnNums[i];
            for (int j = 0; j < column.Length; j++) {
                sb.Append(column[j]);
            }
            sb.Append('\n');
        }

        for (int i = 0; i < _rowNums.GetLength(0); i++) {
            int[] row = _rowNums[i];
            for (int j = 0; j < row.Length; j++) {
                sb.Append(row[j]);
            }
            sb.Append('\n');
        }

        return sb.ToString();
    }

    public void saveBoard() {
        System.IO.File.WriteAllText(BOARD_SAVE_PATH + _name + ".txt",this.ToString());
    }

    public Board loadBoard() {
        return null;
    }
    
    public void loadBoardInfo() {
        string[] lines = System.IO.File.ReadAllLines(@"/boardinfo/" + _name);
        _name = lines[0];
        _size = Convert.ToInt32(lines[1]);

        int count = 2;

        int size = count + _size;
        for (int i = count; i < size; i++) {
            _columnNums[i-count] = Array.ConvertAll(lines[i].Split(' '), int.Parse);
        }

        size = size + _size;
        for (int i = count + _size; i < size; i++) {
            _rowNums[i - count + _size] = Array.ConvertAll(lines[i].Split(' '), int.Parse);
        }

        size = size + _size;
        /*for (int i = count + _size * 2; i < size; i++) {
            int index = i - count + _size * 2;
            string line = lines[index];
            for (int j = 0; j < line.Length; j++) {
                _correctBoard[index, j] = line[j] == '1' ? true : false;
            }
        }*/
    }

    public string formatListHorizontal(int[] list) {

        if (list.Length == 0) {
            return "0";
        }

        else if (list.Length == 1) {
            return "" + list[0];
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Length - 1; i++) {
            sb.Append(list[i] + " ");
        }

        sb.Append(list[list.Length - 1]);

        return sb.ToString();
    }

    public string formatListVertical(int[] list) {
        if (list.Length == 0) {
            return "0";
        }

        else if (list.Length == 1) {
            return "" + list[0];
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Length - 1; i++) {
            sb.Append(list[i] + "\n\n");
        }

        sb.Append(list[list.Length - 1]);

        return sb.ToString();
    }
}
