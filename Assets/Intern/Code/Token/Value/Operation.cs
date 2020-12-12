using System.Collections.Generic;



public class Operation : Value
{
    List<Value> values;
    List<Operator> operators;

    public Operation(int i, List<Value> values, List<Operator> operators) : base(i, 0) {
        this.values = values;
        this.operators = operators;
        setINext(Tool.last(values).iNext);
    }



    public override object getValue(Block block) {
        try {
            object left = values[0].getValue(block);
            for (int i = 0; i < operators.Count; i++) {
                Operator op = operators[i];
                object right = values[i+1].getValue(block);
                if (!op.verifType(left, right)) throw new Error(op.i, op.size, "can't operate those two types");
                left = op.operate(left, right);
            }
            return left;
        } catch (Error err) { throw err; }
    }





    public static void Read(ref List<Token> tokens, int i) {
        if (i >= tokens.Count) throw new Error(GetLastI(tokens), 1, "wait for value");

        try {
            List<Value> values = new List<Value>();
            List<Operator> operators = new List<Operator>();

            Value.Read(ref tokens, i);
            values.Add((Value)tokens[i]);
            tokens.RemoveAt(i);

            while (i < tokens.Count && tokens[i] is Operator) {

                operators.Add((Operator)tokens[i]);
                tokens.RemoveAt(i);

                Value.Read(ref tokens, i);
                values.Add((Value)tokens[i]);
                tokens.RemoveAt(i);
            }

            if (values.Count == 1)  tokens.Insert(i, values[0]);
            else                    tokens.Insert(i, new Operation(values[0].i, values, operators));
        }
        catch (Error err) { throw err; }
    }
}


