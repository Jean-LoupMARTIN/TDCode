

using System.Collections.Generic;

public class ConditionLine : Line
{
    private (Value condition, Block block) ifCond;
    private List<(Value condition, Block block)> elifConds;
    private Block elseBlock;

    private ConditionLine(
        Block parent,
        (Value, Block) ifCond,
        List<(Value, Block)>
        elifConds,
        Block elseBlock) : base(parent) {

        this.ifCond    = ifCond;
        this.elifConds = elifConds;
        this.elseBlock = elseBlock;
    }

    public static ConditionLine Read(List<Token> tokens, ref int i, Block parent) {
        try {
            if (i >= tokens.Count || !(tokens[i] is If))
                throw new Error(Token.GetIChar(tokens, i),1, "can't read if condition");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Value))
                throw new Error(tokens[i-1].iNext, 1, "wait for value");
            (Value condition, Block block) ifCond;
            ifCond.condition = (Value)tokens[i];
            i++;

            ifCond.block = Block.Read(tokens, ref i, parent);
            for (; i < tokens.Count && tokens[i] is EndLine; i++) { }

            List<(Value condition, Block block)> elifConds = new List<(Value condition, Block block)>();
            while (i < tokens.Count && tokens[i] is Elif) {
                i++;
                if (i >= tokens.Count || !(tokens[i] is Value))
                    throw new Error(tokens[i-1].iNext, 1, "wait for value");
                (Value condition, Block block) elifCond;
                elifCond.condition = (Value)tokens[i];
                i++;

                ifCond.block = Block.Read(tokens, ref i, parent);
            }

            for (; i < tokens.Count && tokens[i] is EndLine; i++) { }

            Block elseBlock = null;
            if (i < tokens.Count && tokens[i] is Else) {
                i++;
                elseBlock = Block.Read(tokens, ref i, parent);
            }
            return new ConditionLine(parent, ifCond, elifConds, elseBlock);
        }
        catch(Error err) { throw err; }
    }

    public override void exe(float tStart) {
        try {
            object ifRes = ifCond.condition.getValue(parent);

            if ((ifRes is bool && (bool)ifRes) || (!(ifRes is bool) && ifRes != null))
                ifCond.block.exe(tStart);

            else {
                foreach ((Value condition, Block block) elifCond in elifConds) {
                    object elifRes = elifCond.condition.getValue(parent);
                    if ((ifRes is bool && (bool)ifRes) || (!(ifRes is bool) && ifRes != null)) {
                        elifCond.block.exe(tStart);
                        return;
                    }
                }
                if (elseBlock != null) elseBlock.exe(tStart);
            }
        }
        catch (Error err) { throw err; }
    }
}
