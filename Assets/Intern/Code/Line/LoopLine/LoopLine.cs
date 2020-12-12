

using System.Collections.Generic;

public abstract class LoopLine : Line
{
    protected Block block;

    public LoopLine(Block block, Block parent) : base(parent) { this.block = block; }

    public static LoopLine Read(List<Token> tokens, ref int i, Block parent)
    {
        try {
            if (i >= tokens.Count || !(tokens[i] is For || tokens[i] is While))
                throw new Error(Token.GetIChar(tokens, i), 1, "can't read loop");
            Loop loop = (Loop)tokens[i];

            if (loop is While) return WhileLine.Read(tokens, ref i, parent);

            else {
                if(i+2 >= tokens.Count) throw new Error(loop.i, loop.size, "incomplete for loop");

                if (tokens[i+2] is In)  return ForListLine .Read(tokens, ref i, parent);
                else                    return ForRangeLine.Read(tokens, ref i, parent);
            }
        }
        catch (Error err) { throw err; }
    }

    
}




