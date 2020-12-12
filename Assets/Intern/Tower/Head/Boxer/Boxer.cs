

using System.Collections.Generic;
using UnityEngine;

public class Boxer : TowerHead
{
    public float delayHit, damage, radius;
    public Transform left, right;
    public GameObject shockWave, debrit;

    protected override void Awake() {
        base.Awake();
        animator.SetFloat("hitSpeed", 1 / delayHit);
        funcs.Add("hit", new BoxerHit(this));
    }

    public void hit() {
        if (counterAction > 0) return;
        counterAction += delayHit;
        animator.SetTrigger("hit");
    }


    public void hitLeft()   { hitSphere(left); }
    public void hitRight()  { hitSphere(right); }
    public void hitSphere(Transform point) {

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        Material mat = null;
        foreach (Collider collider in colliders) {
            Enemy enemy = Tool.searchEnemy(collider.transform);
            if (enemy) {
                enemy.takeDamage(damage, point.forward);
                if (!mat) mat = collider.GetComponent<MeshRenderer>().material;
            }
        }
        if (mat) {
            GameObject d = Instantiate(debrit, point.position, point.rotation);
            ParticleSystemRenderer dr = d.GetComponent<ParticleSystemRenderer>();
            dr.material = mat;
            Destroy(d, 3);
        }
        GameObject sw = Instantiate(shockWave, point.position, Quaternion.identity);
        sw.transform.localScale = radius * Vector3.one;
        Destroy(sw, 1);
    }
}

public class BoxerHit : FuncExe {
    Boxer b;
    public BoxerHit(Boxer b) { this.b = b; }
    public bool verifArgs(List<object> args) { return args.Count == 0; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) b.hit();
        return null;
    }
}