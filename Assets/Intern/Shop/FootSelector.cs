

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootSelector : MonoBehaviour
{
    public List<Tower> foot;
    private int i = 0;
    public Text name, cost;

    private void Awake() {
        name.text = getFoot().name;
        cost.text = getFoot().cost+"$";
    }

    public Tower getFoot() { return foot[i]; }

    public void move(int move)  {
        i = (i + move) % foot.Count;
        if (i < 0) i = foot.Count - 1;

        name.text = getFoot().name;
        cost.text = getFoot().cost+"$";

        Shop.inst.updateTowerView();
    }
}
