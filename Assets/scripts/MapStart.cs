using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStart : MonoBehaviour {

    public Map map;
    public GameObject mapPanel;

    void Awake() {
        map = new Map(10);
        DontDestroyOnLoad(GameObject.Find("MapBundle"));

        mapPanel = GameObject.Find("MapPanel");
    }

	//Loads BoardInfos from the Map save file and instantiates prefabs on the map
	void Start () {
        for (int c = 0; c < map._size; c++) {
            GameObject column = Instantiate(Resources.Load("GridColumnPrefab")) as GameObject;
            column.transform.SetParent(mapPanel.transform);
            for (int r = 0; r < map._size; r++) {
                
            }
        }
	}
}
