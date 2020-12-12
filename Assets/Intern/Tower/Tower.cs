

using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, Obj
{
    public static void PRINT(object obj) { print(obj); }

    public static List<Tower> towers = new List<Tower>();

    public readonly static (float w, Color c) OUTLINE_OVER   = (2, Color.white);
    public readonly static (float w, Color c) OUTLINE_SELECT = (3, Color.yellow);

    public string name;
    public int cost;
    public Transform headPos;
    public Agent agent;
    public Outline outline;
    public string defaultCode;

    [HideInInspector] public TowerHead head;
    [HideInInspector] public Vector3 posMem;
    [HideInInspector] public List<int> bonus = new List<int>();
    [HideInInspector] public Block block;

    [HideInInspector] public Dictionary<string, Getter> attrs = new Dictionary<string, Getter>();
    [HideInInspector] public Dictionary<string, FuncExe> funcs = new Dictionary<string, FuncExe>();
    public Dictionary<string, Getter> getAttrs() { return attrs; }
    public Dictionary<string, FuncExe> getFuncs() { return funcs; }

    protected virtual void Awake() {
        attrs.Add("name",     new GetTowerName(this));
        attrs.Add("position", new GetTransPos(transform));
        attrs.Add("rotation", new GetTransRot(transform));
        attrs.Add("speed",    new GetTowerSpeed(this));
        attrs.Add("cost",     new GetTowerCost(this));

        funcs.Add("moveTo", new MoveTo(this));
        funcs.Add("print", new Print(this));
        funcs.Add("dist", new Dist());
    }

    protected virtual void Start() {
        WaveManager.inst.startWaveEvent.AddListener(savePos);
        WaveManager.inst.endWaveEvent.AddListener(replace);

        if (head != null) {
            foreach (KeyValuePair<string, Getter> attr in head.attrs) attrs[attr.Key] = attr.Value;
            foreach (KeyValuePair<string, FuncExe> func in head.funcs) funcs[func.Key] = func.Value;
            if (defaultCode != "") defaultCode += '\n';
            string code = "\n\n" +
                "target = enemies.getClosest(position)\n" +
                "look(target.position)\n" +
                defaultCode +
                head.defaultCode +
                "\n\n";
            (_, _, block) = Block.Read(code, this);
        }
    }

    private void savePos() { posMem = transform.position; }
    
    public void replace(){
        transform.rotation      = Quaternion.identity;
        head.transform.rotation = Quaternion.identity;
        agent.nva.path.ClearCorners();
        agent.setPos(posMem);
    }

    protected virtual void Update() {
        if (WaveManager.inWave) {
            try { block.exe(System.DateTime.Now.Millisecond); }
            catch (Error err) { print(err.mess); }
        }
    }

    public void moveTo(Transform  target) { moveTo(target.position); }
    public void moveTo(GameObject target) { moveTo(target.transform.position); }
    public void moveTo(Enemy      target) { moveTo(target.transform.position); }
    public virtual void moveTo(Vector3 target) { agent.moveTo(target); }

    public void attachHead(TowerHead head) {
        this.head = head;
        name += " " + head.name;
        cost += head.cost;
    }

    // 0 default   1 over   2 select
    public void setOutline(int type) {
        if (type == 0) { outline.enabled = false; return; }

        outline.enabled = true;

        float w;
        Color c;

        if (type == 1) { w = OUTLINE_OVER.w;   c = OUTLINE_OVER.c; }
        else           { w = OUTLINE_SELECT.w; c = OUTLINE_SELECT.c; }

        outline.OutlineWidth = w;
        outline.OutlineColor = c;
    }

    public static Tower getClosest(Vector3 target) {
        Tower output = null;
        float dist2 = -1;
        foreach (Tower tower in towers) {
            float dx = target.x - tower.headPos.position.x;
            float dy = target.z - tower.headPos.position.z;
            float crtDist2 = dx*dx + dy*dy;
            if (output == null || crtDist2 < dist2) {
                output = tower;
                dist2 = crtDist2;
            }
        }
        return output;
    }
}


public class GetTowerName  : Getter { Tower t;  public GetTowerName (Tower t)  { this.t = t; } public object get() { return t.name; } }
public class GetTowerSpeed : Getter { Tower t;  public GetTowerSpeed(Tower t)  { this.t = t; } public object get() { return t.agent.speed; } }
public class GetTowerCost  : Getter { Tower t;  public GetTowerCost (Tower t)  { this.t = t; } public object get() { return t.cost; } }

public class MoveTo : FuncExe {
    Tower t;
    public MoveTo(Tower t) { this.t = t; }
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is MyVect; }
    public object exe(List<object> args) {
        if(WaveManager.inWave) t.moveTo(((MyVect)args[0]).v);
        return null;
    }
}

public class Print : FuncExe {
    Tower t;
    public Print(Tower t) { this.t = t; }
    public bool verifArgs(List<object> args) { return args.Count == 1; }
    public object exe(List<object> args) {
        Tower.PRINT(t.name + " : " + args[0]);
        return null;
    }
}

public class Dist : FuncExe {
    public bool verifArgs(List<object> args) { return args.Count == 2 && args[0] is MyVect && args[1] is MyVect; }
    public object exe(List<object> args) {
        Vector3 a = ((MyVect)args[0]).v, b = ((MyVect)args[1]).v;
        return (b-a).magnitude;
    }
}