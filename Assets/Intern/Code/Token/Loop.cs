

public abstract class Loop : Token
{
    public static readonly string COLOR = "5189FF";
    public Loop(int i, int size) : base(i, size) { }
}

public class While : Loop { public While(int i) : base(i, 5) { } }
public class For   : Loop { public For  (int i) : base(i, 3) { } }
public class In    : Loop { public In   (int i) : base(i, 2) { } }
public class From  : Loop { public From (int i) : base(i, 4) { } }
public class To    : Loop { public To   (int i) : base(i, 2) { } }