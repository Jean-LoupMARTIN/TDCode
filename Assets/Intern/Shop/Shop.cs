

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop inst;

    public int money;

    public Transform world;
    public HeadSelector headSelector;
    public FootSelector footSelector;
    public Transform towerViewPlace;
    private GameObject towerGO;
    private Tower tower;

    public Text moneyTxt, cost;
    public GameObject buyButt;

    private void Awake() {
        inst = this;
        moneyTxt.text = money + " $";
    }

    private void Start() { updateTowerView(); }


    public void updateTowerView() {
        if (towerGO) Destroy(towerGO);

        towerGO = Instantiate(
            footSelector.getFoot().gameObject,
            towerViewPlace.position,
            towerViewPlace.rotation,
            towerViewPlace);

        Tower tower = towerGO.GetComponent<Tower>();

        GameObject headGO = Instantiate(
            headSelector.getHead().gameObject,
            tower.headPos.position,
            tower.headPos.rotation,
            tower.headPos);

        TowerHead head = headGO.GetComponent<TowerHead>();

        tower.attachHead(head);

        cost.text = tower.cost+"$";

        if (tower.cost > money) buyButt.active = false;
        else                    buyButt.active = true;
    }

    public void buy() {
        int areaMask = towerGO.GetComponent<Tower>().agent.nva.areaMask;
        if (NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 100, areaMask)) {
            tower = Instantiate(towerGO, hit.position, Quaternion.identity, world).GetComponent<Tower>();
            tower.agent.enable();
            GrabManager.inst.grab(tower, true);
        }
    }

    public void finishBuy() {
        addMoney(-tower.cost);
        Tower.towers.Add(tower);
        tower.enabled = true;
        tower.head.enabled = true;
    }


    public void addMoney(int nb) {
        money += nb;
        moneyTxt.text = money + " $";
        if (towerGO.GetComponent<Tower>().cost > money) buyButt.active = false;
        else                                            buyButt.active = true;
    }


    /*

    public Camera cam;
    private Tower towerGrab;

    public static int layerPut;
    public static int layerBuy;


    private void Awake() {
        inst = this;
        layerPut =  ~LayerMask.GetMask("Tower") &
                    ~LayerMask.GetMask("GhostObj") &
                    ~LayerMask.GetMask("Object") &
                    ~LayerMask.GetMask("Enemy") &
                    ~LayerMask.GetMask("Projectil");

        layerBuy = LayerMask.GetMask("BuyPlane");
    }


    


    

    private void Update() {
        if (towerGrab) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500, layerPut))
                if (Physics.Raycast(hit.point + Vector3.up, Vector3.down, out _, 500, layerBuy))
                    if (Physics.Raycast(hit.point + Vector3.up, Vector3.down, out hit, 500, layerPut))
                        towerGrab.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0)) buy();
            else if (Input.GetMouseButtonDown(1)) cancelBuy();
        }
    }

    private void buy() {
        addMoney(-towerGrab.cost);
        towerGrab.GetComponent<NavMeshAgent>().enabled = true;
        towerGrab.GetComponent<Agent>().enabled = true;
        towerGrab.enabled = true;

        Tower.towers.Add(towerGrab);
        endBuy();
    }

    private void cancelBuy() {
        Destroy(towerGrab.gameObject);
        endBuy();
    }

    

    public void addMoney(int nb) {
        money += nb;
        if (towerGO.GetComponent<Tower>().cost > money) buyButt.active = false;
        else buyButt.active = true;
        moneyTxt.text = money + " $";
    }
    */
}
