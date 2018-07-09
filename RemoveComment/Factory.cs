using System;

namespace RemoveComment
{
    public class Factory
    {
        public static Remover Creator(string type)
        {
            switch(type.ToLower())
            {
                case "lua":
                    return new Lua();
                case "c":
                case "c++":
                case "c#":
                case "csharp":
                case "cs":
                case "java":
                case "oc":
                case "objective-c":
                case "php":
                case "javascript":
                case "js":
                    return new CChildren();
                default:
                    Console.WriteLine($"[{type}] is not supported");
                    return null;
            }
        }
    }
}
