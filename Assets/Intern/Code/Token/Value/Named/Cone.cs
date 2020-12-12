using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : Named
{
    public readonly static string COLOR = "FF9652";
    public Cone(int i) : base(i, 4, "name") { }
    public override object getValue(Block block) { return new MyVect(WaveManager.inst.target.position); }
}

public class End : Named {
    private MyVect v;
    public End(int i) : base(i, 3, "name") {
        v = new MyVect(WaveManager.inst.end.position);
    }
    public override object getValue(Block block) { return v; }
}
