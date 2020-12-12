

public class Bool : Value
{
    public static readonly string COLOR = "73D7FF";

    private bool value;
    public Bool(int i, int size, bool value) : base(i, size) { this.value = value; }

    public override object getValue(Block block) { return value; }
}


