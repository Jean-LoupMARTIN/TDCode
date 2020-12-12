using System.Collections.Generic;


public class WhileLine : LoopLine {
    private Value condition;

    public WhileLine(Block block, Value condition, Block parent) : base(block, parent) {
        this.condition = condition;
    }

    public override void exe(float tStart) {
        try {
            while (true) {
                object res = condition.getValue(parent);
                if ((res is bool && (bool)res) || (!(res is bool) && res != null))
                    block.exe(tStart);
                else break;
            }
        }
        catch (Error err) { throw err; }
    }


    public static WhileLine Read(List<Token> tokens, ref int i, Block parent) {
        try {
            if (i >= tokens.Count || !(tokens[i] is While))
                throw new Error(Token.GetIChar(tokens, i), 1, "can't read while");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Value))
                throw new Error(tokens[i-1].iNext, 1, "wait for value");
            Value condition = (Value)tokens[i];
            i++;

            Block block = Block.Read(tokens, ref i, parent);

            return new WhileLine(block, condition, parent);
        }
        catch (Error err) { throw err; }
    }
}
