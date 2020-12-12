


public class String : Value
{
    private static readonly string COLOR = "FF959C";

    private string value;
    private String(int i, int size, string value) : base(i, size) { this.value = value; }

    public override object getValue(Block block) { return value; }

    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        int iMem = i;
        token = null;
        txtToken = "\"";
        color = COLOR;

        if (txt[i] != '\"') return false;
        string value = "";
        i++;

        for (; i < txt.Length; i++) {
            txtToken += txt[i];
            if (txt[i] == '\"') { token = new String(iMem, txtToken.Length, value); return true; }
            value += txt[i];
        }
        return false;
    }
}


