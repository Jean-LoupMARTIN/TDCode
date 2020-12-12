

using System.Collections.Generic;

public class Token
{
    public int i, size, iEnd, iNext;

    public Token(int i, int size) {
        this.i = i;
        this.size = size;
        iNext = i + size;
        iEnd = iNext - 1;
    }

    public void setINext(int iNext) {
        this.iNext = iNext;
        iEnd = iNext - 1;
        size = iNext - i;
    }

    public static (string txtColor, Error, List<Token>) Translate(string txt, Tower tower) {
        string txtColor;
        List<Token> tokens;
        (txtColor, tokens) = FirstRead(txt, tower);
        try { SecondRead(ref tokens); } catch (Error err) { return (txtColor, err, null); }
        return (txtColor, null, tokens);
    }

    public static (string txtColor, List<Token>) FirstRead(string txt, Tower tower) {
        string txtColor = "";
        List<Token> tokens = new List<Token>();

        for (int i = 0;  i < txt.Length;) {

            Token token;
            string txtToken, color;

            // space
            if (txt[i] == ' ' || txt[i] == '\t') { txtColor += txt[i]; i++; }

            // comment
            else if (Comment.read(txt, i, out txtToken)) {
                txtColor += "<color=#"+Comment.COLOR+">" + txtToken + "</color>";
                i += txtToken.Length;
            }

            // token
            else if (
                String   .read(txt, i, out token, out txtToken, out color) ||
                Float    .read(txt, i, out token, out txtToken, out color) ||
                Operator .read(txt, i, out token, out txtToken, out color) ||
                CharToken.read(txt, i, out token, out txtToken, out color) ||
                Named    .read(txt, i, tower, out token, out txtToken, out color)) {

                tokens.Add(token);
                if(color != "") txtColor += "<color=#"+color+">" + txtToken + "</color>";
                else txtColor += txtToken;
                i += txtToken.Length;
            }

            // unknow
            else { txtColor += "<color=#"+Comment.COLOR+">" + txt[i] + "</color>"; i++; }
        }

        return (txtColor, tokens);
    }



    
    public static void SecondRead(ref List<Token> tokens) {
        try {
            for (int i=0; i < tokens.Count; i++) {
                Token token = tokens[i];
                if (token is Value || token is BracketOpen)
                    Operation.Read(ref tokens, i);
            }
        }
        catch(Error err) { throw err; }
    }




    public static int GetLastI(List<Token> tokens) {
        if (tokens.Count > 0) return Tool.last(tokens).iNext;
        return 1;
    }

    public static int GetIChar(List<Token> tokens, int iToken) {
        if (iToken >= tokens.Count) return GetLastI(tokens);
        return tokens[iToken].i;
    }
}

