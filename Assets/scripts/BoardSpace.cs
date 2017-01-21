using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSpace : MonoBehaviour {

    public CreatorHandler board;
    public bool _value;
    public int _row;
    public int _column;

    public EventTrigger trigger;

    /*public BoardSpace(int row, int column) {
        _row = row;
        _column = column;
        _value = false;
    }*/

    void Start() {
        _value = false;

        trigger = GetComponent<EventTrigger>();

        /*EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => OnPointerDownDelegate((PointerEventData)(data)));
        trigger.triggers.Add(pointerDownEntry);*/

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => OnPointerEnterDelegate((PointerEventData)(data)));
        trigger.triggers.Add(entry);

        /*EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => OnPointerUpDelegate((PointerEventData)(data)));
        trigger.triggers.Add(pointerUpEntry);*/
    }
    
    public void toggle() {
        _value = !_value;
    }

    

    public void OnPointerEnterDelegate(PointerEventData data) {

        if (InputHandler.direction == InputHandler.directionDecided.no) {
            if (_row == InputHandler.currentRow) {
                InputHandler.direction = InputHandler.directionDecided.row;
            }

            else if (_column == InputHandler.currentColumn) {
                InputHandler.direction = InputHandler.directionDecided.column;
            }
        }

        if (!InputHandler.down) {
			InputHandler.down = true;
			InputHandler.fillingIn = !_value;
		    InputHandler.currentColumn = _column;
		    InputHandler.currentRow = _row;
            InputHandler.direction = InputHandler.directionDecided.no;
		}

        if (InputHandler.direction == InputHandler.directionDecided.no || (InputHandler.direction == InputHandler.directionDecided.row && InputHandler.currentRow == _row) || 
            (InputHandler.direction == InputHandler.directionDecided.column && InputHandler.currentColumn == _column)) {
            if (!_value && InputHandler.fillingIn) {
			    transform.GetComponent<Image>().color = Color.black;
                toggle();
            }

		    else if (_value && !InputHandler.fillingIn) {
			    transform.GetComponent<Image>().color = Color.white;
                toggle();
            }
        }

            

        board.updateRowAndColumnPanel(_row, _column);
    }
}