using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour {

    public static bool down = false;
    public static int currentRow;
    public static int currentColumn;
    public static directionDecided direction; //makes it so once you go in a column or row you can only fill those in
    public static bool fillingIn; //whether you are filling things in or taking them out
    public static bool mode; //X's or blocks

    public enum directionDecided {
        no, column, row
    }

    void Update() {
        if (Input.touchCount == 0) {
            InputHandler.down = false;
            direction = directionDecided.no;
        }
    }
}
