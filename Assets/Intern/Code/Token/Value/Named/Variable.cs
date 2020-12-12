using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : Named
{
    public Variable(int i, int size, string name) : base(i, size, name) { }
    public override object getValue(Block block) { return block.GetVar(name); }
}