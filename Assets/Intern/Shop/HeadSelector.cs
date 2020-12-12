

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadSelector : MonoBehaviour
{
    public List<TowerHead> heads;
    private int i = 0;
    public Text name, cost;

    private void Awake() {
        name.text = getHead().name;
        cost.text = getHead().cost+"$";
    }

    public TowerHead getHead() { return heads[i]; }

    public void move(int move)  {
        i = (i + move) % heads.Count;
        if (i < 0) i = heads.Count - 1;

        name.text = getHead().name;
        cost.text = getHead().cost+"$";

        Shop.inst.updateTowerView();
    }
}
