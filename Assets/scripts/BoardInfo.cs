using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInfo {

	public static string BASE_FILEPATH = "/boards/";

    //FORMAT "Heart,15,0,1,M"
    public string _name { get; }
    public int _size { get; }
    public bool _completed { get; }
    public bool _unlocked { get; }
    public string _difficulty { get; }

    public BoardInfo(string name, int size, bool completed, bool unlocked, string difficulty) {
        _name = name;
        _size = size;
        _completed = completed;
        _unlocked = unlocked;
        _difficulty = difficulty;
    }
}
