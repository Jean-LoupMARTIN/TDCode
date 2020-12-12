

using System.Collections.Generic;

public class FunctionLine : Line
{
    private Function function;

    private FunctionLine(Function function, Block parent) : base(parent) {
        this.function = function;
    }

    public static FunctionLine Read(List<Token> tokens, ref int i, Block parent)
    {
        if (i >= tokens.Count || !(tokens[i] is Function))
            throw new Error(Token.GetIChar(tokens, i), 1, "can't read function line");
        Function function = (Function)tokens[i];
        i++;

        if (i < tokens.Count && !(tokens[i] is EndLine))
            throw new Error(tokens[i - 1].iNext, 1, "wait for end line");
        i++;

        return new FunctionLine(function, parent);
    }

    public override void exe(float tStart) {
        try { function.getValue(parent); }
        catch (Error err) { throw err; }
    }
}
