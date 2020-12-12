


public class Attr : Named {
    public Named leftName;
    public Attr(int i, int size, string name) : base(i, size, name) { }
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
        if(!left.getAttrs().ContainsKey(name))
            throw new Error(i, size, "no attribute named : " + name);
        return left.getAttrs()[name].get();
    }
}

