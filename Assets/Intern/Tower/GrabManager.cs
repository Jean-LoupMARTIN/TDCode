

using UnityEngine;
using UnityEngine.AI;

public class GrabManager : MonoBehaviour
{
    public static GrabManager inst;
    private void Awake() { inst = this; }

    public CamMove camMove;
    private Tower towerGrabed;
    private bool forBuy;
    public GameObject WaitWaveMenu;

    public void grab(Tower tower, bool forBuy) {
        towerGrabed = tower;
        this.forBuy = forBuy;
        if(!forBuy) camMove.enabled = false;
        TowerSelector.inst.enabled = false;
        WaitWaveMenu.active = false;
    }

    private void Update() {

        if (towerGrabed) {

            if (Tool.mouseHit(out RaycastHit hit, LayersManager.grabTowerLayer)) 
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit hit2, 100, towerGrabed.agent.nva.areaMask))
                    towerGrabed.agent.setPos(hit2.position);

            if (forBuy) {
                if (Input.GetMouseButtonDown(0)) {
                    Shop.inst.finishBuy();
                    endGrab();
                }
                else if (Input.GetMouseButtonDown(1)) {
                    Destroy(towerGrabed.gameObject);
                    endGrab();
                }
            }

            else if (Input.GetMouseButtonUp(0)) {
                camMove.enabled = true;
                TowerMenu.inst.active(towerGrabed);
                endGrab();
            }
        }
    }

    private void endGrab() {
        towerGrabed = null;
        TowerSelector.inst.enabled = true;
        WaitWaveMenu.active = true;
    }
}
