

using System.Collections.Generic;

public abstract class Line
{
    public Block parent;

    public Line(Block parent) { this.parent = parent; }

    public static (Error, List<Line>) Translate(List<Token> tokens, Block parent) {
        List<Line> lines = new List<Line>();

        for (int i = 0; i < tokens.Count;) {
            Token token = tokens[i];
            try {
                if      (token is EndLine) { i++; }
                else if (token is Variable)                 lines.Add(Affectation  .Read(tokens, ref i, parent));
                else if (token is Function)                 lines.Add(FunctionLine .Read(tokens, ref i, parent));
                else if (token is If)                       lines.Add(ConditionLine.Read(tokens, ref i, parent));
                else if (token is For || token is While)    lines.Add(LoopLine     .Read(tokens, ref i, parent));
                else throw new Error(token.i, token.size, "can't read line");
            }
            catch (Error err) { return (err, null); }
        }
        return (null, lines);
    }


    public abstract void exe(float tStart);
}


