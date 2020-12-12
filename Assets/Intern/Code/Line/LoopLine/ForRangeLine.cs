


using System.Collections.Generic;

public class ForRangeLine : LoopLine {
    private Variable var;
    private Value start, end;

    public ForRangeLine(Block block, Variable var, Value start, Value end, Block parent) : base(block, parent) {
        this.var = var;
        this.start = start;
        this.end = end;
    }

    public override void exe(float tStart) {
        try {
            object startRes = start.getValue(parent);
            object endRes   = end.getValue(parent);
            if (!(startRes is float)) throw new Error(start.i, start.size, "start need to be an integer");
            if (!(endRes   is float)) throw new Error(start.i, start.size, "end need to be an integer");
            float startf = (float)startRes;
            float endf   = (float)endRes;
            for (float i = startf; i < endf; i++) {
                parent.SetVar(var.name, i);
                block.exe(tStart);
            }
        }
        catch (Error err) { throw err; }
    }

    public static ForRangeLine Read(List<Token> tokens, ref int i, Block parent) {
        try {
            if (i >= tokens.Count || !(tokens[i] is For))
                throw new Error(Token.GetIChar(tokens, i), 1, "can't read for");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Variable))
                throw new Error(tokens[i-1].iNext, 1, "wait for variable");
            Variable var = (Variable)tokens[i];
            i++;

            if (i >= tokens.Count || !(tokens[i] is From))
                throw new Error(tokens[i-1].iNext, 1, "wait for from");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Value))
                throw new Error(tokens[i-1].iNext, 1, "wait for value");
            Value start = (Value)tokens[i];
            i++;

            if (i >= tokens.Count || !(tokens[i] is To))
                throw new Error(tokens[i-1].iNext, 1, "wait for to");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Value))
                throw new Error(tokens[i-1].iNext, 1, "wait for value");
            Value end = (Value)tokens[i];
            i++;

            Block block = Block.Read(tokens, ref i, parent);

            return new ForRangeLine(block, var, start, end, parent);
        }
        catch (Error err) { throw err; }
    }
}
