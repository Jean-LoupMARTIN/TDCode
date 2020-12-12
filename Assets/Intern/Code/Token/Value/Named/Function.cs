using System.Collections.Generic;


public class Function : Named {
    public Named leftName;
    public List<Value> args = new List<Value>();
    public Function(int i, int size, string name) : base(i, size, name) { }

    public override object getValue(Block block) {
        Obj left;
        if (leftName == null) left = block.tower;
        else {
            try {
                object obj = leftName.getValue(block);
                if (obj is Obj) left = (Obj)obj;
                else throw new Error(leftName.i, leftName.size, leftName.name + " is not an object");
            }
            catch (Error err) { throw err; }
        }

        if(!left.getFuncs().ContainsKey(name))
            throw new Error(i, size, "no function named : " + name);

        FuncExe funcExe = left.getFuncs()[name];
        List<object> argsObj = new List<object>();
        foreach (Value val in args) {
            try { argsObj.Add(val.getValue(block)); }
            catch (Error err) { throw err; }
        }

        if (!funcExe.verifArgs(argsObj))
            throw new Error(i, size, "wrong arguments");

        return funcExe.exe(argsObj);
    }



    public static void Read(ref List<Token> tokens, int i) {
        try {
            if (i >= tokens.Count || !(tokens[i] is Function))
                throw new Error(GetIChar(tokens, i), 1, "wait for function name");

            Function function = (Function)tokens[i];

            i++;
            if (i >= tokens.Count || !(tokens[i] is BracketOpen))
                throw new Error(GetIChar(tokens, i), 1, "wait for (");
            tokens.RemoveAt(i);

            if(i >= tokens.Count)
                throw new Error(GetIChar(tokens, i), 1, "wait for )");


            if (tokens[i] is BracketClose) {
                function.setINext(tokens[i].iNext);
                tokens.RemoveAt(i);
                return;
            }

            Operation.Read(ref tokens, i);
            function.args.Add((Value)tokens[i]);
            tokens.RemoveAt(i);

            while (i < tokens.Count && tokens[i] is Comma) {
                tokens.RemoveAt(i);
                Operation.Read(ref tokens, i);
                function.args.Add((Value)tokens[i]);
                tokens.RemoveAt(i);
            }
            if (i >= tokens.Count || !(tokens[i] is BracketClose))
                throw new Error(tokens[i-1].iNext, 1, "wait for )");

            function.setINext(tokens[i].iNext);
            tokens.RemoveAt(i);
        }
        catch (Error err) { throw err; }
    }
}
