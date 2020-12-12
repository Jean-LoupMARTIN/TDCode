using System.Collections.Generic;

public class Block
{
    public readonly static float T_MAX = 5;
    public Tower tower;
    public Dictionary<string, object> vars = new Dictionary<string, object>();
    public List<Line> lines = null;
    public Block parent = null;
    public string txtMem;

    public Block(Tower tower) {
        this.tower = tower;
    }
    public static (string txtColor, Error, Block) Read(string txt, Tower tower) {
        string txtColor;
        List<Token> tokens;
        Error err;
        Block block = new Block(tower);
        block.txtMem = txt;

        (txtColor, err, tokens) = Token.Translate(txt, tower);
        if (err != null) return (txtColor, err, block);

        List<Line> lines;
        (err, lines) = Line.Translate(tokens, block);
        if (err != null) return (txtColor, err, block);

        block.lines = lines;

        try { block.exe(System.DateTime.Now.Millisecond); }
        catch (Error e) {
            block.lines = null;
            return (txtColor, e, block);
        }

        return (txtColor, null, block);
    }

    public static Block Read(List<Token> tokens, ref int i, Block parent)
    {
        for (; i < tokens.Count && tokens[i] is EndLine; i++) { } 

        if (i >= tokens.Count || !(tokens[i] is BraceOpen))
            throw new Error(Token.GetIChar(tokens, i), 1, "wait for {");
        int iBraceOpen = tokens[i].i;
        i++;


        List<Token> subTokens = new List<Token>();
        int braceCount = 1;
        for (; i < tokens.Count; i++) {
            if (tokens[i] is BraceClose) {
                braceCount--;
                if (braceCount == 0) break;
            }
            else if(tokens[i] is BraceOpen) braceCount++;
            subTokens.Add(tokens[i]);
        }

        if (i >= tokens.Count)
            throw new Error(iBraceOpen, 1, "wait for }");
        i++;

        Block block = new Block(parent.tower);
        Error err;
        List<Line> lines;
        (err, lines) = Line.Translate(subTokens, block);
        if (err != null) throw err;

        block.parent = parent;
        block.lines = lines;

        return block;
    }

    public object GetVar(string name) {
        for (Block block = this; block != null; block = block.parent)
            if (block.vars.ContainsKey(name)) return block.vars[name];
        return null;
    }

    public void SetVar(string name, object value) {
        for (Block block = this; block != null; block = block.parent)
            if (block.vars.ContainsKey(name)) { block.vars[name] = value; return; }
        vars[name] = value;
    }

    public void exe(float tStart) {
        if (lines == null) return;
        if (System.DateTime.Now.Millisecond - tStart > T_MAX) throw new Error(0, 1, "to long...");

        vars.Clear();
        try { foreach (Line line in lines) line.exe(tStart); }
        catch (Error err) { throw err; }
    }
}
