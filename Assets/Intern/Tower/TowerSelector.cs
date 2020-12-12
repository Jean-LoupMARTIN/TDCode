

using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    
    public static TowerSelector inst;
    private void Awake() {
        inst = this;
        WaveManager.inst.endWaveEvent.AddListener(active);
    }

    private void active() { gameObject.active = true; }
    private Tower towerOver, towerSelected;

    
    private void Update() {

        // select
        if (Input.GetMouseButtonDown(0) && !TowerMenu.inst.mouseOnMenu()) {

            if (towerOver == null) {
                if(towerSelected != null) {
                    towerSelected.setOutline(0);
                    towerSelected = null;
                }
            }
            else {
                if(towerOver != towerSelected) {
                    if(towerSelected != null) towerSelected.setOutline(0);
                    towerSelected = towerOver;
                    towerSelected.setOutline(2);
                }
                GrabManager.inst.grab(towerSelected, false);
            }
        }

        // over
        if (Tool.mouseHit(out RaycastHit hit) && !TowerMenu.inst.mouseOnMenu()) {
            Transform trans = hit.transform;
            Tower t = Tool.searchTower(trans);
            if (t) {
                if (t != towerOver) {
                    if (towerOver != null && towerOver != towerSelected)
                        towerOver.setOutline(0);
                    towerOver = t;
                    if (t != towerSelected) 
                        t.setOutline(1);
                }
            }
            else {
                if (towerOver != null && towerOver != towerSelected)
                    towerOver.setOutline(0);
                towerOver = null;
            }
        }else {
            if (towerOver != null && towerOver != towerSelected)
                towerOver.setOutline(0);
            towerOver = null;
        }
    }
}
