


using System.Collections.Generic;
using UnityEngine;

public abstract class MyList : Named
{
    public static readonly string COLOR = "FFC1FF";
    public MyList(int i, int size, string name) : base(i, size, name) {}
}

public class Enemies : MyList, Obj {

    public static Enemies inst;

    static Enemies(){
        inst = new Enemies(-1);
        inst.attrs = new Dictionary<string, Getter>();
        inst.funcs = new Dictionary<string, FuncExe>();

        inst.attrs.Add("size", new GetSizeEnemies());

        inst.funcs.Add("get", new GetEnemy());
        inst.funcs.Add("getClosest", new GetClosestEnemy());
    }

    public Dictionary<string, Getter>  attrs;
    public Dictionary<string, FuncExe> funcs;

    public Enemies(int i) : base(i, 7, "enemies") {}

    public Dictionary<string, Getter>  getAttrs() { return attrs; }
    public Dictionary<string, FuncExe> getFuncs() { return funcs; }

    public override object getValue(Block block) { return inst; }
}


public class Towers : MyList, Obj {

    public static Towers inst;

    static Towers(){
        inst = new Towers(-1);
        inst.attrs = new Dictionary<string, Getter>();
        inst.funcs = new Dictionary<string, FuncExe>();

        inst.attrs.Add("size", new GetSizeTowers());

        inst.funcs.Add("get", new GetTower());
        inst.funcs.Add("getClosest", new GetClosestTower());
    }

    public Dictionary<string, Getter>  attrs;
    public Dictionary<string, FuncExe> funcs;

    public Towers(int i) : base(i, 6, "towers") {}

    public Dictionary<string, Getter>  getAttrs() { return attrs; }
    public Dictionary<string, FuncExe> getFuncs() { return funcs; }

    public override object getValue(Block block) { return inst; }
}

public class GetSizeEnemies : Getter { public object get() { return (float)Enemy.enemies.Count; } }
public class GetSizeTowers  : Getter { public object get() { return (float)Tower.towers .Count; } }

public class GetEnemy : FuncExe {
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is float; }
    public object exe(List<object> args) {
        int i = (int)(float)args[0];
        if (i < Enemy.enemies.Count)
            return Enemy.enemies[i];
        return null;
    }
}

public class GetTower : FuncExe {
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is float; }
    public object exe(List<object> args) {
        int i = (int)(float)args[0];
        if(i < Tower.towers.Count)
            return Tower.towers[i];
        return null;
    }
}

public class GetClosestEnemy : FuncExe {
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is MyVect; }
    public object exe(List<object> args) {
        MyVect target = (MyVect)args[0];
        return Enemy.getClosest(target.v);
    }
}

public class GetClosestTower : FuncExe {
    public bool verifArgs(List<object> args) { return args.Count == 1 && args[0] is MyVect; }
    public object exe(List<object> args) {
        MyVect target = (MyVect)args[0];
        return Tower.getClosest(target.v);
    }
}