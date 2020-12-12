

public abstract class Operator: Token {
    public int priority;
    public Operator(int i, int size, int priority) : base(i, size) { this.priority = priority; }
    public abstract bool verifType(object a, object b);
    public abstract object operate(object a, object b);

    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        return
            Comparator   .read(txt, i, out token, out txtToken, out color) ||
            OperatorBool .read(txt, i, out token, out txtToken, out color) ||
            OperatorFloat.read(txt, i, out token, out txtToken, out color);
    }
}


