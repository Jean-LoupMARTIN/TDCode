


using System.Collections.Generic;
using UnityEngine;

public class MyVect : Obj {
    public Vector3 v;
    public Dictionary<string, Getter> attrs = new Dictionary<string, Getter>();
    public Dictionary<string, FuncExe> funcs = new Dictionary<string, FuncExe>();
    public Dictionary<string, Getter> getAttrs() { return attrs; }
    public Dictionary<string, FuncExe> getFuncs() { return funcs; }

    public MyVect(Vector3 v) {
        this.v = v;
        attrs.Add("x", new GetFloat(v.x));
        attrs.Add("y", new GetFloat(v.y));
        attrs.Add("z", new GetFloat(v.z));
        attrs.Add("norm", new GetFloat(v.magnitude));
        attrs.Add("normalized", new GetVect(v.normalized));
    }
}


public class GetFloat : Getter { float f; public GetFloat(float f) { this.f = f; } public object get() { return f; } }
public class GetVect : Getter { Vector3 v; public GetVect(Vector3 v) { this.v = v; } public object get() { return new MyVect(v); } }
