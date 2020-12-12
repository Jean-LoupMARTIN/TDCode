

using System.Collections.Generic;
using UnityEngine;

public class Demon : TowerHead
{
    public float distCurse, damage;
    public GameObject curseGO;
    private ParticleSystem cursePS;
    private Enemy enemyCursed;

    protected override void Awake() {
        base.Awake();
        funcs.Add("curse", new DemonCurse(this));
    }

    private void Start() {
        cursePS = Instantiate(curseGO).GetComponent<ParticleSystem>();
    }

    public void curse(Enemy enemy) {

        if (!enemy || enemy == enemyCursed) return;

        if (Tool.dist(enemy.gravity, transform) < distCurse)
            startCurse(enemy);

        else if (enemyCursed) stopCurse();
    }

    private void startCurse(Enemy enemy) {
        enemyCursed = enemy;
        cursePS.Play();
    }

    private void stopCurse() {
        enemyCursed = null;
        cursePS.Stop();
    }

    protected override void Update() {
        base.Update();

        if (enemyCursed && Tool.dist(enemyCursed.gravity, transform) < distCurse) {
            cursePS.transform.position = enemyCursed.gravity.position;
            enemyCursed.takeDamage(damage, new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), 3);
        }
        else stopCurse();
    }

    public void OnDestroy() { if (cursePS) Destroy(cursePS.gameObject);}
}


public class DemonCurse : FuncExe {
    Demon d;
    public DemonCurse(Demon d) { this.d = d; }
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is Enemy; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) d.curse((Enemy)args[0]);
        return null;
    }
}