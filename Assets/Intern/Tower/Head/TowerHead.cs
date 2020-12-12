
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerHead : MonoBehaviour
{
    private static readonly float ROT_SPEED = 20;

    public string name;
    public int cost;
    public Animator animator;
    protected float counterAction = 0;
    public string defaultCode;

    [HideInInspector] public Dictionary<string, Getter> attrs = new Dictionary<string, Getter>();
    [HideInInspector] public Dictionary<string, FuncExe> funcs = new Dictionary<string, FuncExe>();

    protected virtual void Awake() {
        funcs.Add("look", new Look(this));
    }

    public void look(Enemy     target)  { if (target == null) return; look(target.gravity); }
    public void look(Transform target)  { if (target == null) return; look(target.position); }
    public void look(Vector3   target) {
        Vector3 dir = Tool.dir(transform, target, false);

        float alpha = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg; 

        float dAlpha = (alpha - transform.eulerAngles.y)  % 360;
        
        if      (dAlpha < -180) dAlpha += 360;
        else if (dAlpha >  180) dAlpha -= 360;

        
        if (dAlpha != 0) {
            float rotAlpha;
            if (dAlpha > 0) rotAlpha = Mathf.Min(ROT_SPEED * Time.deltaTime * Mathf.Max(dAlpha / 180, 0.1f), dAlpha * Mathf.Deg2Rad);
            else            rotAlpha = Mathf.Max(ROT_SPEED * Time.deltaTime * Mathf.Min(dAlpha / 180, 0.1f), dAlpha * Mathf.Deg2Rad);
            transform.RotateAround(Vector3.up, rotAlpha);
        }


        float beta = Mathf.Atan2(dir.y, Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z)) * Mathf.Rad2Deg; // [-90;90]
        float xEuler = -transform.eulerAngles.x;
        if      (xEuler < -180) xEuler += 360;
        else if (xEuler >  180) xEuler -= 360;
        float dBeta = beta - xEuler;

        if (dBeta != 0) {
            float rotBeta;
            if (dBeta > 0) rotBeta = Mathf.Min(-ROT_SPEED * Time.deltaTime * Mathf.Max(dBeta / 180, 0.1f), dBeta * Mathf.Deg2Rad);
            else           rotBeta = Mathf.Max(-ROT_SPEED * Time.deltaTime * Mathf.Min(dBeta / 180, 0.1f), dBeta * Mathf.Deg2Rad);
            transform.RotateAround(transform.right, rotBeta);
        }
    }

    protected virtual void Update() {
        counterAction = Mathf.Max(counterAction - Time.deltaTime, 0);
    }
}

public class Look : FuncExe {
    TowerHead th;
    public Look(TowerHead th) { this.th = th; }
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is MyVect; }
    public object exe(List<object> args) {
        if (WaveManager.inWave) th.look(((MyVect)args[0]).v);
        return null;
    }
}