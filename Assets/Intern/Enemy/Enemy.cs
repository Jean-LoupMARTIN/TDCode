

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Obj
{
    public readonly static float DIST_GRAB = 2;
    public static List<Enemy> enemies = new List<Enemy>();
    public static Enemy enemyTarget;

    public string name;
    public int cost, life, weight;
    [HideInInspector]public float crtLife;
    public bool metallic = false, fly = false;

    public BrokableCube body;
    public Agent agent;
    public Transform targetPos, gravity, lifeBar;
    private Transform cam;

    [HideInInspector] public Dictionary<string, Getter> attrs = new Dictionary<string, Getter>();
    [HideInInspector] public Dictionary<string, FuncExe> funcs = new Dictionary<string, FuncExe>();
    public Dictionary<string, Getter> getAttrs() { return attrs; }
    public Dictionary<string, FuncExe> getFuncs() { return funcs; }

    private void Awake() {
        attrs.Add("name",     new GetEnemyName(this));
        attrs.Add("position", new GetTransPos(transform));
        attrs.Add("rotation", new GetTransRot(transform));
        attrs.Add("speed",    new GetEnemySpeed(this));
        attrs.Add("metallic", new GetEnemyMetallic(this));
        attrs.Add("fly",      new GetEnemyFly(this));
        attrs.Add("life",     new GetEnemyLife(this));

        cam = GameObject.Find("Main Camera").transform;
        crtLife = life;
        lifeBar = Instantiate(lifeBar);
        WaveManager.inst.startWaveEvent.AddListener(go);
        enemies.Add(this);
    }

    private void go() {
        agent.enable();
        agent.moveTo(WaveManager.inst.target);
    }

    

    protected virtual void Update() {

        updateLifeBar();

        if (WaveManager.inWave) {

            if(enemyTarget == null) {
                if (Tool.dist(WaveManager.inst.target, gravity) < DIST_GRAB)
                    grab();
            }
            else if(enemyTarget != this)
                agent.moveTo(enemyTarget.transform.position - 2 * enemyTarget.transform.forward);

            else {
                WaveManager.inst.target.position = targetPos.position;
                WaveManager.inst.target.rotation = targetPos.rotation;
            }
        }
    }

    private void updateLifeBar() {
        if (Tool.dist(cam, transform) < 30) {
            lifeBar.gameObject.active = true;
            Vector3 lifeBarPos = targetPos.position + Vector3.up * 0.5f;
            if (enemyTarget == this) lifeBarPos += Vector3.up * 2;
            lifeBar.position = lifeBarPos;

            Vector3 dir = Tool.dir(lifeBar, cam, false);
            float targetO = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float crtO = lifeBar.transform.eulerAngles.y;
            float deltaO = (targetO - crtO) % 360;
            if (deltaO > 180) deltaO -= 360;
            else if (deltaO < -180) deltaO += 360;

            if (deltaO != 0) lifeBar.transform.Rotate(0, deltaO, 0);
        }
        else lifeBar.gameObject.active = false;
    }

    private void grab() {
        enemyTarget = this;
        WaveManager.inst.target.GetComponent<NavMeshObstacle>().enabled = false;        
        agent.moveTo(WaveManager.inst.end);
    }



    public static Enemy getClosest(Vector3 target) {
        Enemy output = null;
        float dist2 = -1;
        foreach (Enemy enemy in enemies) {
            float dx = target.x - enemy.gravity.position.x;
            float dy = target.z - enemy.gravity.position.z;
            float crtDist2 = dx*dx + dy*dy;
            if (output == null || crtDist2 < dist2) {
                output = enemy;
                dist2 = crtDist2;
            }
        }
        return output;
    }

    
    public void takeDamage(float damage, Vector3 dir, float knockBackCoef=1) {

        crtLife -= damage;
        knockBackCoef *= damage / weight;

        if (crtLife < 0) { die(knockBackCoef, dir); return; }

        dir.y = 0;
        transform.position += dir * knockBackCoef;

        Vector3 scale = lifeBar.localScale;
        scale.x = crtLife / life;
        lifeBar.localScale = scale;
    }

    private void die(float strong, Vector3 dir) {

        Destroy(lifeBar.gameObject);

        if (enemyTarget == this) {
            enemyTarget = null;
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 100, LayersManager.enemyMaskArea)) {
                WaveManager.inst.target.position = hit.position;
                WaveManager.inst.target.rotation = Quaternion.identity;
                WaveManager.inst.target.GetComponent<NavMeshObstacle>().enabled = true;
                foreach (Enemy enemy in enemies) enemy.agent.moveTo(WaveManager.inst.target);
            }
        }
        Shop.inst.addMoney(cost);
        if(body) body.explose(strong, dir, transform.parent);
        Destroy(gameObject);
        enemies.Remove(this);
    }
}


public class GetEnemyName    : Getter { Enemy e;  public GetEnemyName    (Enemy e)  { this.e = e; } public object get() { return e.name; } }
public class GetEnemySpeed   : Getter { Enemy e;  public GetEnemySpeed   (Enemy e)  { this.e = e; } public object get() { return e.agent.speed; } }
public class GetEnemyLife    : Getter { Enemy e;  public GetEnemyLife    (Enemy e)  { this.e = e; } public object get() { return e.crtLife; } }
public class GetEnemyFly     : Getter { Enemy e;  public GetEnemyFly     (Enemy e)  { this.e = e; } public object get() { return e.fly; } }
public class GetEnemyMetallic: Getter { Enemy e;  public GetEnemyMetallic(Enemy e)  { this.e = e; } public object get() { return e.metallic; } }
public class GetTransPos : Getter { Transform t; public GetTransPos(Transform t) { this.t = t; } public object get() { return new MyVect(t.position); } }
public class GetTransRot : Getter { Transform t; public GetTransRot(Transform t) { this.t = t; } public object get() { return new MyVect(t.rotation.eulerAngles); } }
