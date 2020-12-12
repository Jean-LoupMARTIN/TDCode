


using System.Collections.Generic;

public class ForListLine : LoopLine {
    private Variable var;
    private MyList list;

    public ForListLine(Block block, Variable var, MyList list, Block parent) : base(block, parent) {
        this.var = var;
        this.list = list;
    }

    public override void exe(float tStart) {
        try {
            List<Obj> listObjs;
            if (list is Enemies) {
                foreach (Enemy enemy in Enemy.enemies) {
                    parent.SetVar(var.name, enemy);
                    block.exe(tStart);
                }
            }
            else if (list is Towers) {
                foreach (Tower tower in Tower.towers) {
                    parent.SetVar(var.name, tower);
                    block.exe(tStart);
                }
            }
            else new Error(list.i, list.size, "unknown list");
        }
        catch (Error err) { throw err; }
    }

    public static ForListLine Read(List<Token> tokens, ref int i, Block parent) {
        try {
            if (i >= tokens.Count || !(tokens[i] is For))
                throw new Error(Token.GetIChar(tokens, i), 1, "can't read for");
            i++;

            if (i >= tokens.Count || !(tokens[i] is Variable))
                throw new Error(tokens[i-1].iNext, 1, "wait for variable");
            Variable var = (Variable)tokens[i];
            i++;

            if (i >= tokens.Count || !(tokens[i] is In))
                throw new Error(tokens[i-1].iNext, 1, "wait for in");
            i++;

            if (i >= tokens.Count || !(tokens[i] is MyList))
                throw new Error(tokens[i-1].iNext, 1, "wait for list");
            MyList list = (MyList)tokens[i];
            i++;

            Block block = Block.Read(tokens, ref i, parent);

            return new ForListLine(block, var, list, parent);
        }
        catch (Error err) { throw err; }
    }
}
