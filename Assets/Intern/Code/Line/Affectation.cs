

using System.Collections.Generic;

public class Affectation : Line
{
    private Variable var;
    private Value value;

    private Affectation(Block parent, Variable var, Value value) : base(parent) {
        this.var = var;
        this.value = value;
    }

    public static Affectation Read(List<Token> tokens, ref int i, Block parent) {
        if (i >= tokens.Count || !(tokens[i] is Variable))
            throw new Error(Token.GetIChar(tokens, i),1, "can't read affectation");
        Variable var = (Variable)tokens[i];
        i++;



        if (i >= tokens.Count || !(tokens[i] is Equal))
            throw new Error(Token.GetIChar(tokens, i), 1, "wait for =");
        i++;

        if (i >= tokens.Count || !(tokens[i] is Value))
            throw new Error(Token.GetIChar(tokens, i), 1, "wait for value");
        Value value = (Value)tokens[i];
        i++;

        if (i < tokens.Count && !(tokens[i] is EndLine))
            throw new Error(tokens[i-1].iNext, 1, "wait for end line");
        i++;

        return new Affectation(parent, var, value); 
    }

    public override void exe(float tStart) {
        try { parent.SetVar(var.name, value.getValue(parent)); }
        catch (Error err) { throw err; }
    }
}


