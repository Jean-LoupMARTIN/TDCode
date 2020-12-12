


public class Comment
{
    public static readonly string COLOR = "777777";

    public static bool read(string  txt, int i, out string comment) {

        comment = "";

        if (i+1 >= txt.Length) return false;
        comment = txt.Substring(i, 2);
        i += 2;

        if(comment == "//") {
            for (; i < txt.Length && txt[i] != '\n'; i++)
                comment += txt[i];
            return true;
        }

        else if(comment == "/*") {
            for (; i < txt.Length; i++) {
                comment += txt[i];
                if (txt[i-1] == '*' && txt[i] == '/') break;
            }
            return true;
        }

        return false;
    }
}


