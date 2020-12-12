

public abstract class Condition : Token {
    public static readonly string COLOR = "5189FF";

    public Condition(int i, int size) : base(i, size) { }
}


public class If   : Condition { public If  (int i) : base(i, 2) { } }
public class Elif : Condition { public Elif(int i) : base(i, 4) { } }
public class Else : Condition { public Else(int i) : base(i, 4) { } }