

using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public float sellCoef = 0.8f;
    public static TowerMenu inst;
    public Text name, upTxt, upDesableTxt, sellTxt;
    public GameObject up, upDesable;
    public RectTransform menuRT;
    [HideInInspector]public Tower tower;

    private void Awake() {
        inst = this;
        gameObject.active = false;
    }

    public void active(Tower tower) {
        this.tower = tower;
        name.text = tower.name;
        int upCost = UpCost();
        upTxt.text        = "UP " + upCost + "$";
        upDesableTxt.text = "UP " + upCost + "$";
        sellTxt.text = "SELL " + (int)(tower.cost * sellCoef) + "$";
        if (upCost <= Shop.inst.money) { up.active = true;  upDesable.active = false; }
        else                           { up.active = false; upDesable.active = true; }
        transform.position = Tool.world2screen(tower.transform);
        gameObject.active = true;
    }


    public void desactive() {
        tower = null;
        gameObject.active = false;
    }

    private int UpCost() { return (tower.bonus.Count + 1) * 100; }

    private void Update() {
        if (tower) {
            transform.position = Tool.world2screen(tower.transform);
            if (Input.GetMouseButtonDown(0) && !mouseOnMenu()) desactive();
        }
    }

    public bool mouseOnMenu() {
        return tower && Tool.pointInRect(Input.mousePosition, menuRT);
    }

    public void Up() {
        int upCost = UpCost();
        if (upCost <= Shop.inst.money) {
            tower.transform.localScale = 0.05f * Vector3.one + tower.transform.localScale;
            tower.bonus.Add(1);
            Shop.inst.addMoney(-upCost);
            tower.cost += upCost;
        }
        active(tower);
    }


    public void Sell() {
        Shop.inst.addMoney((int)(tower.cost * sellCoef));
        Tower.towers.Remove(tower);
        Destroy(tower.gameObject);
        desactive();
    }
}
