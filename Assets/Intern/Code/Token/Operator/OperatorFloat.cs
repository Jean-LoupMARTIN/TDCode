

public abstract class OperatorFloat : Operator {

    protected static readonly string COLOR = "FFFF00";

    public OperatorFloat(int i, int priority) : base(i, 1, priority) { }


    public override bool verifType(object a, object b) { return a is float && b is float; }


    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        string c = txt[i].ToString();

        token = null;
        txtToken = c;
        color = COLOR;

        if      (c == "+") token = new Plus(i);
        else if (c == "-") token = new Minus(i);
        else if (c == "*") token = new Times(i);
        else if (c == "/") token = new Divided(i);
        else if (c == "%") token = new Modulo(i);

        return token != null;
    }
}



public class Plus : OperatorFloat {
    public Plus(int i):base(i, 2) {}
    public override bool verifType(object a, object b) { return (a is float && b is float) || (a is MyVect && b is MyVect); }
    public override object operate(object a, object b) {
        if (a is float) return (float)a + (float)b;
        else return new MyVect(((MyVect)a).v + ((MyVect)b).v);
    }
}
public class Minus : OperatorFloat {
    public Minus(int i):base(i, 2) {}
    public override bool verifType(object a, object b) { return (a is float && b is float) || (a is MyVect && b is MyVect); }
    public override object operate(object a, object b) {
        if (a is float) return (float)a - (float)b;
        else return new MyVect(((MyVect)a).v - ((MyVect)b).v);
    }
}


public class Times  :OperatorFloat {public Times  (int i):base(i, 3) {} public override object operate(object a, object b) { return (float)a * (float)b;}}
public class Divided:OperatorFloat {public Divided(int i):base(i, 3) {} public override object operate(object a, object b) { return (float)a / (float)b;}}
public class Modulo :OperatorFloat {public Modulo (int i):base(i, 3) {} public override object operate(object a, object b) { return (float)a % (float)b;}}

