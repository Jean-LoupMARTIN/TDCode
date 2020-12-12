

using UnityEngine;

public abstract class Comparator : Operator {
    private static readonly string COLOR = "FFFF00";
    public Comparator(int i, int size) : base(i, size, 1) {}


    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        token = null;
        txtToken = "";
        color = COLOR;

        char c = txt[i];

        if (c == '<') {
            if (i+1 < txt.Length && txt[i+1] == '=') { token = new InfEq(i); txtToken = "<="; }
            else                                     { token = new Inf(i);   txtToken = "<";  }
        }
        else if (c == '>') {
            if (i+1 < txt.Length && txt[i+1] == '=') { token = new SupEq(i); txtToken = ">="; }
            else                                     { token = new Sup(i);   txtToken = ">";  }
        }

        else if (c == '=' && i+1 < txt.Length && txt[i+1] == '=') { token = new EqEq(i); txtToken = "=="; }
        else if (c == '!' && i+1 < txt.Length && txt[i+1] == '=') { token = new Diff(i); txtToken = "!="; }

        return token != null;
    }

    public override bool verifType(object a, object b) { return a is float && b is float; }
}




public class EqEq : Comparator {
    public EqEq (int i) : base(i, 2) { }

    public override bool verifType(object a, object b) {
        return
            (a is bool && b is bool) ||
            (a is float && b is float) ||
            (a is string && b is string) ||
            (a is Vector3 && b is Vector3) ||
            (a is Tower && b is Tower) ||
            (a is Enemy && b is Enemy);
    }

    public override object operate(object a, object b) {
        if (a is bool && b is bool) return (bool)a == (bool)b;
        if (a is string && b is string) return (string)a == (string)b;
        if (a is float && b is float) return (float)a == (float)b;
        if (a is Vector3 && b is Vector3) {
            Vector3 va = (Vector3)a, vb = (Vector3)b;
            return va.x == vb.x && va.y == vb.y && va.z == vb.z;
        }
        return a == b;
    }
}

public class Diff : Comparator {
    public Diff(int i) : base(i, 2) { }

    public override bool verifType(object a, object b) {
        return
            (a is bool && b is bool) ||
            (a is float && b is float) ||
            (a is string && b is string) ||
            (a is Vector3 && b is Vector3) ||
            (a is Tower && b is Tower) ||
            (a is Enemy && b is Enemy);
    }

    public override object operate(object a, object b) {
        if (a is bool && b is bool) return (bool)a != (bool)b;
        if (a is string && b is string) return (string)a != (string)b;
        if (a is float && b is float) return (float)a != (float)b;
        if (a is Vector3 && b is Vector3) {
            Vector3 va = (Vector3)a, vb = (Vector3)b;
            return va.x != vb.x || va.y != vb.y || va.z != vb.z;
        }
        return a != b;
    }
}

public class Inf   : Comparator { public Inf  (int i) : base(i, 1) { } public override object operate(object a, object b) { return (float)a <  (float)b; } }
public class Sup   : Comparator { public Sup  (int i) : base(i, 1) { } public override object operate(object a, object b) { return (float)a >  (float)b; } }
public class InfEq : Comparator { public InfEq(int i) : base(i, 2) { } public override object operate(object a, object b) { return (float)a <= (float)b; } }
public class SupEq : Comparator { public SupEq(int i) : base(i, 2) { } public override object operate(object a, object b) { return (float)a >= (float)b; } }
