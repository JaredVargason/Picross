using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Map {

    private static string MAP_FILEPATH = "/map/map.txt";

    public int _size;
    public BoardInfo[,] _map;
    
    public Map(int size) {
        _size = size;
        _map = new BoardInfo[size, size];
    }

    public BoardInfo getBoardInfo(int x, int y) {
        return _map[x, y];
    }

    override
    public string ToString() {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _size; i++) {
            for (int j = 0; j < _size; j++) {
                BoardInfo bi = _map[i, j];
                sb.Append(bi._name + ',' + bi._size + ',' + bi._difficulty + ',' + bi._completed + ',' + bi._unlocked);
            }
            sb.Append('\n');
        }

        return sb.ToString();
    }

    public void loadMap() {
        string[] lines = System.IO.File.ReadAllLines(MAP_FILEPATH);
        for (int i = 0; i < lines.Length; i++) {
            string[] strings = lines[i].Split(',');
            BoardInfo info = new BoardInfo(strings[0],
                int.Parse(strings[1]),
                strings[2] == "1",
                strings[3] == "1",
                strings[4]               
                );
            _map[i / _size, i % _size] = info;
        }
    }

    public void saveMap() {
        System.IO.File.WriteAllText(MAP_FILEPATH, this.ToString());
    }
}
