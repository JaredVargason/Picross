using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("World Map Button").GetComponent<Button>().onClick.AddListener( delegate {
            SceneManager.LoadScene("map_scene");
        });

        GameObject.Find("My Puzzles Button").GetComponent<Button>().onClick.AddListener(delegate {
            SceneManager.LoadScene("my_puzzles_scene");
        });

        GameObject.Find("Creator Button").GetComponent<Button>().onClick.AddListener(delegate {
            SceneManager.LoadScene("creator_scene");
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
