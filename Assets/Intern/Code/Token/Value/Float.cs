


public class Float : Value
{
    private static readonly string COLOR = "80FFCC";

    private float value;
    public Float(int i, int size, float value) : base(i, size) { this.value = value; }

    public override object getValue(Block block) { return value; }

    public static bool read(

            string  txt,
            int     i,

            out Token   token,
            out string  txtToken,
            out string  color) {

        int iMem = i;
        token = null;
        txtToken = "";
        color = COLOR;

        if (txt[i] == '-') { txtToken += '-'; i++; }

        if (i >= txt.Length || !char.IsDigit(txt[i])) return false;
        txtToken += txt[i];
        i++;


        bool point = false;
        for (; i < txt.Length; i++) {
            if (!point && txt[i] == '.') point = true;
            else if (!char.IsDigit(txt[i])) break;
            txtToken += txt[i];
        }

        token = new Float(iMem, txtToken.Length, float.Parse(txtToken));
        return true;
    }
}
