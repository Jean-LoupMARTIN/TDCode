

using System.Collections.Generic;
using UnityEngine;

public class Canon : TowerHead
{
    public Transform firePoint;
    public GameObject ball, ballShot;
    public Animator animator;

    public float damage;
    public float explosionRadius, delayShot, ballSpeed;

    protected override void Awake() {
        base.Awake();
        animator.SetFloat("shotSpeed", 1 / delayShot);
        funcs.Add("shot", new CanonShot(this));
    }

    public void shot() {
        
        if (counterAction > 0) return;
        counterAction += delayShot;

        GameObject ballGO = Instantiate(ball, firePoint.position, firePoint.rotation);
        CanonBall b = ballGO.GetComponent<CanonBall>();
        b.damage = damage;
        b.radius = explosionRadius;
        b.speed = ballSpeed;
        GameObject bs = Instantiate(ballShot, firePoint.position, firePoint.rotation);
        Destroy(bs, 1);
        
        animator.SetTrigger("shot");
        Tool.shakeSphere(firePoint, 1f);
    }
}

public class CanonShot : FuncExe {
    Canon c;
    public CanonShot(Canon c) { this.c = c; }
    public bool verifArgs(List<object> args) { return args.Count == 0; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) c.shot();
        return null;
    }
}