

public abstract class CharToken : Token
{
    public CharToken(int i) : base(i, 1) { }

    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        token = null;
        txtToken = txt[i].ToString();
        color = "";

        if      (txt[i] == '\n') { token = new EndLine(i);}
        else if (txt[i] == '(')  { token = new BracketOpen(i);   color = "FFFFFF"; }
        else if (txt[i] == ')')  { token = new BracketClose(i);  color = "FFFFFF"; }
        else if (txt[i] == '{')  { token = new BraceOpen(i);     color = "FFFFFF"; }
        else if (txt[i] == '}')  { token = new BraceClose(i);    color = "FFFFFF"; }
        else if (txt[i] == '[')  { token = new CrochetOpen(i);   color = "FFC1FF"; }
        else if (txt[i] == ']')  { token = new CrochetClose(i);  color = "FFC1FF"; }
        else if (txt[i] == '.')  { token = new Point(i);         color = "FFFFFF"; }
        else if (txt[i] == ',')  { token = new Comma(i);         color = "FFFFFF"; }
        else if (txt[i] == '=')  { token = new Equal(i);         color = "FF6100"; }

        return token != null;
    }
}


public class EndLine :      CharToken { public EndLine      (int i) : base(i) { } }
public class BracketOpen :  CharToken { public BracketOpen  (int i) : base(i) { } }
public class BracketClose : CharToken { public BracketClose (int i) : base(i) { } }
public class BraceOpen :    CharToken { public BraceOpen    (int i) : base(i) { } }
public class BraceClose :   CharToken { public BraceClose   (int i) : base(i) { } }
public class CrochetOpen :  CharToken { public CrochetOpen  (int i) : base(i) { } }
public class CrochetClose : CharToken { public CrochetClose (int i) : base(i) { } }
public class Point :        CharToken { public Point        (int i) : base(i) { } }
public class Comma :        CharToken { public Comma        (int i) : base(i) { } }
public class Equal :        CharToken { public Equal        (int i) : base(i) { } }
