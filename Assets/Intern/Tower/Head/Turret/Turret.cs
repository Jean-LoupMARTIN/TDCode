

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : TowerHead
{
    public Transform firePoint;
    public GameObject bullet, BulletShot;

    public float bulletDamage, delayAction, bulletSpeed, deltaRot = 2;

    [HideInInspector] public int loaded = 1;
    public int maxLoaded = 5;
    public float delayShotLoaded = 0.1f;

    protected override void Awake() {
        base.Awake();
        animator.SetFloat("shotSpeed", 1 / delayAction);
        funcs.Add("shot", new TurretShot(this));
        funcs.Add("load", new TurretLoad(this));
        attrs.Add("delayAction",    new GetTurretDelayAction(this));
        attrs.Add("bulletSpeed",    new GetTurretBulletSpeed(this));
        attrs.Add("bulletDamage",   new GetTurretBulletDamage(this));
        attrs.Add("loaded",         new GetTurretLoaded(this));
        attrs.Add("maxLoaded",      new GetTurretMaxLoaded(this));
    }

    public void shot() {
        if (counterAction > 0) return;
        counterAction += delayAction;
        StartCoroutine("ShotLoaded");
    }

    public void load() {
        if (counterAction > 0) return;
        counterAction += delayAction;
        loaded = Mathf.Min(maxLoaded, loaded + 1);
    }

    IEnumerator ShotLoaded() {
        for (int i = 0; i < loaded; i++) {
            GameObject bulletGO = Instantiate(bullet, firePoint.position, firePoint.rotation);
            bulletGO.transform.Rotate(
                Random.Range(-deltaRot, deltaRot),
                Random.Range(-deltaRot, deltaRot),
                Random.Range(-deltaRot, deltaRot));
            Bullet b = bulletGO.GetComponent<Bullet>();
            b.speed = bulletSpeed;
            b.damage = bulletDamage;
            GameObject bs = Instantiate(BulletShot, firePoint.position, firePoint.rotation);
            Destroy(bs, 1);

            animator.SetTrigger("shot");

            Tool.shakeSphere(firePoint, 0.7f);

            yield return new WaitForSeconds(delayShotLoaded);
        }
        loaded = 1;
    }
}

public class TurretShot : FuncExe {
    Turret t;
    public TurretShot(Turret t) { this.t = t; }
    public bool verifArgs(List<object> args) { return args.Count == 0; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) t.shot();
        return null;
    }
}

public class TurretLoad : FuncExe {
    Turret t;
    public TurretLoad(Turret t) { this.t = t; }
    public bool verifArgs(List<object> args) { return args.Count == 0; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) t.load();
        return null;
    }
}

public class GetTurretDelayAction  : Getter { Turret t; public GetTurretDelayAction (Turret t) { this.t = t; } public object get() { return t.delayAction; } }
public class GetTurretBulletSpeed  : Getter { Turret t; public GetTurretBulletSpeed (Turret t) { this.t = t; } public object get() { return t.bulletSpeed; } }
public class GetTurretBulletDamage : Getter { Turret t; public GetTurretBulletDamage(Turret t) { this.t = t; } public object get() { return t.bulletDamage; } }
public class GetTurretLoaded       : Getter { Turret t; public GetTurretLoaded      (Turret t) { this.t = t; } public object get() { return (float)t.loaded; } }
public class GetTurretMaxLoaded    : Getter { Turret t; public GetTurretMaxLoaded   (Turret t) { this.t = t; } public object get() { return (float)t.maxLoaded; } }