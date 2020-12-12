


using System.Collections.Generic;

public abstract class Value : Token {
    public Value(int i, int size) : base(i, size) { }
    public abstract object getValue(Block block);

    public static void Read(ref List<Token> tokens, int i) {
        try {
            if (i >= tokens.Count) throw new Error(GetLastI(tokens), 1, "wait for value");

            Token token = tokens[i];

            if (token is Float || token is Bool || token is String) return;

            else if (token is BracketOpen) {
                int iBracketOpen = token.i;
                tokens.RemoveAt(i);

                Operation.Read(ref tokens, i); i++;
                if (i >= tokens.Count || !(tokens[i] is BracketClose))
                    throw new Error(tokens[i-1].iNext, 1, "wait for )");
                tokens.RemoveAt(i);
            }

            else if (token is Named) {
                Named left = (Named)token;
                if (left is Function) Function.Read(ref tokens, i);
                tokens.RemoveAt(i);

                while (i < tokens.Count && tokens[i] is Point) {
                    tokens.RemoveAt(i);

                    if (i >= tokens.Count || !(tokens[i] is Attr || tokens[i] is Function)) 
                        throw new Error(left.iNext+1, 1, "wait for attribute / function");

                    if (tokens[i] is Function) {
                        Function.Read(ref tokens, i);
                        Function func = (Function)tokens[i];
                        func.leftName = left;
                    } else {
                        Attr attr = (Attr)tokens[i];
                        attr.leftName = left;
                    }
                    left = (Named)tokens[i];
                    tokens.RemoveAt(i);
                }

                tokens.Insert(i, left);
            }

            else throw new Error(token.i, 1, "wait for value");
        }
        catch (Error err) { throw err; }
    }



    
}



