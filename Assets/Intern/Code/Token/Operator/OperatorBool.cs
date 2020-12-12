

public abstract class OperatorBool : Operator {
    protected static readonly string COLOR = "5189FF";
    public OperatorBool(int i, int size) : base(i, size, 0) { }

    public override bool verifType(object a, object b) { return a is bool && b is bool; }

    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        token = null;
        txtToken = "";
        color = COLOR;

        if (i+1 < txt.Length) {
            txtToken = txt.Substring(i, 2);

            if      (txtToken == "&&") token = new And(i, 2);
            else if (txtToken == "||") token = new Or(i);
            else if (txtToken == "or") token = new Or(i);

            else if (i+2 < txt.Length) {
                txtToken = txt.Substring(i, 3);
                if (txtToken == "and") token = new And(i, 3);
            }
        }

        return token != null;
    }
}



public class And : OperatorBool { public And(int i, int size) : base(i, size) { } public override object operate(object a, object b) { return (bool)a && (bool)b; } }
public class Or  : OperatorBool { public Or (int i)           : base(i, 2)    { } public override object operate(object a, object b) { return (bool)a || (bool)b; } }

