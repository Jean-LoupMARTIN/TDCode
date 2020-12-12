

using System;
using TMPro;
using UnityEngine;



public class Error : Exception
{
    public int i, length;
    public string mess;

    public Error(int i, int length, string mess) {
        this.i = i;
        this.length = length;
        this.mess = mess;
    }
}



public class ErrorMess : MonoBehaviour
{
    public TMP_Text mess;
    public RectTransform rt, rtTrigger;
    public Vector2 pos;

    public void active(string mess, Vector2 pos, float length) {
        this.pos = pos;
        Vector2 size = new Vector3(length, 3), sizeTrigger = new Vector3(length+20, 50);
        rt.sizeDelta = size;
        rtTrigger.sizeDelta = sizeTrigger;
        this.mess.text = mess;
        gameObject.active = true;
    }

    public void desactive() { gameObject.active = false; }

    private void Update()
    {
        if (gameObject.active) {
            if (Tool.pointInRect(Input.mousePosition, rtTrigger)) mess.enabled = true;
            else mess.enabled = false;
        }
    }
}
