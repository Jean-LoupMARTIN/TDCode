

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class InputCode : MonoBehaviour
{
    private TMP_InputField input;
    public TMP_Text txtHidden, txtDisplay, lines;
    private Block block;
    private string txtChanged = null;
    public ErrorMess errMess;

    private void Awake() {
        input = GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener(TxtChanged);
    }

    public void Open() {
        input.text = TowerMenu.inst.tower.block.txtMem;
    }

    private void TxtChanged(string txt) { txtChanged = txt; }

    private void Update() {
        if (txtChanged != null) { UpdateBlock(txtChanged); txtChanged = null; }
        UpdatePos();
    }

    private void UpdateBlock(string txt) {
        Error err;
        Tower tower = TowerMenu.inst.tower;
        (txtDisplay.text, err, block) = Block.Read(txt, tower);
        tower.block = block;
        UpdateErr(err);
        UpdateLines();
    }



    private void UpdateLines() {
        int line = 1;
        lines.text = "1 |";
        string txt = input.text;
        for (int i = 0; i < txt.Length; i++) {
            if (txt[i] == '\n') {
                line++;
                lines.text += "\n" + line + " |";
            }
        }
        for (int i = line; i < 50; i++) lines.text += "\n|";
    }

    private void UpdatePos() {
        txtDisplay.transform.position = txtHidden.transform.position;
        Vector2 linePos = lines.transform.position;
        linePos.y = txtHidden.transform.position.y;
        lines.transform.position = linePos;
        Vector2 errMessPos = txtHidden.transform.position;
        errMessPos += errMess.pos;
        errMess.transform.position = errMessPos;
    }


    private void UpdateErr(Error err) {
        if (err == null) errMess.desactive();
        else {
            float wChar = 15.4f;
            float hChar = wChar * 2.26f;

            int line = 1, xChar = 0;
            string txt = input.text;
            for (int i = 0; i < txt.Length && i < err.i; i++) {
                if (txt[i] == '\n') { line++; xChar = 0; }
                else if (txt[i] == '\t') xChar += 10 - xChar % 10;
                else xChar++;
            }
            Vector2 pos = new Vector2(xChar * wChar, -line * hChar);
            errMess.active(err.mess, pos, err.length * wChar);
        }
    }
}
