


using System.Collections.Generic;

public interface Obj
{
    Dictionary<string, Getter>  getAttrs();
    Dictionary<string, FuncExe> getFuncs();
}


public interface Getter { object get(); }
public interface FuncExe {
    bool verifArgs(List<object> args);
    object exe(List<object> args);
}


