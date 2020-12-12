


public abstract class Named : Value
{
    public string name;

    public Named(int i, int size, string name) : base(i, size) { this.name = name; }

    public static bool read(
            
            string  txt,
            int     i,
            Tower   tower,

            out Token   token,
            out string  txtToken,
            out string  color) {

        int iMem = i;
        token = null;
        txtToken = "";
        color = "";
        bool attr = i > 0 && txt[i-1] == '.';

        if (!char.IsLetter(txt[i])) return false;

        for (; i < txt.Length && (char.IsLetter(txt[i]) ||
                                  char.IsDigit(txt[i])  ||
                                  txt[i] == '_' ||
                                  txt[i] == '-');
                                  i++)
            txtToken += txt[i];

        attr |= tower.attrs.ContainsKey(txtToken);

        if (i < txt.Length && txt[i] == '(') { token = new Function(iMem, txtToken.Length, txtToken); color = "FFF5C5"; }
        else if (attr)                  { token = new Attr    (iMem, txtToken.Length, txtToken); color = "FFDEC7"; }
        else if (txtToken == "enemies") { token = new Enemies (iMem);                            color = MyList.COLOR; }
        else if (txtToken == "towers")  { token = new Towers  (iMem);                            color = MyList.COLOR; }
        else if (txtToken == "cone")    { token = new Cone    (iMem);                            color = Cone.COLOR; }
        else if (txtToken == "end")     { token = new End     (iMem);                            color = Cone.COLOR; }
        else if (txtToken == "true")    { token = new Bool    (iMem, 4, true);                   color = Bool.COLOR; }
        else if (txtToken == "false")   { token = new Bool    (iMem, 5, false);                  color = Bool.COLOR; }
        else if (txtToken == "if")      { token = new If      (iMem);                            color = Condition.COLOR; }
        else if (txtToken == "elif")    { token = new Elif    (iMem);                            color = Condition.COLOR; }
        else if (txtToken == "else")    { token = new Else    (iMem);                            color = Condition.COLOR; }
        else if (txtToken == "while")   { token = new While   (iMem);                            color = Loop.COLOR; }
        else if (txtToken == "for")     { token = new For     (iMem);                            color = Loop.COLOR; }
        else if (txtToken == "in")      { token = new In      (iMem);                            color = Loop.COLOR; }
        else if (txtToken == "from")    { token = new From    (iMem);                            color = Loop.COLOR; }
        else if (txtToken == "to")      { token = new To      (iMem);                            color = Loop.COLOR; }
        else                            { token = new Variable(iMem, txtToken.Length, txtToken); color = "FFFFFF"; }

        return true;
    }
}



















