using System.Text.RegularExpressions;

namespace RemoveComment
{
    /// <summary>
    /// 适用于包括 C、C++、C#、Java、OC、PHP、JavaScript等和其他用 “//” 和 “/* */”做为注释的语言
    /// </summary>
    public class CChildren : Remover
    {
        public override string Execute(string file)
        {
            file = Regex.Replace
            (
              file,
              @"(?ms)""[^""]*""|//.*?$|/\*.*?\*/",
              delegate (Match m)
              {
                  switch(m.Value.Substring(0, 2))
                  {
                      case "//":
                          return "";
                      case "/*":
                          return " ";
                      default:
                          return m.Value;
                  }
              }
            );
            return file;
        }
    }
}
