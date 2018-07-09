using System.Text.RegularExpressions;

namespace RemoveComment
{
    /// <summary>
    /// 适用于Lua语言注释去除
    /// </summary>
    public class Lua : Remover
    {
        public override string Execute(string file)
        {
            return RemoveSignleLineComment(RemoveBlockComment(file));
        }

        private string RemoveBlockComment(string file)
        {
            file = Regex.Replace
            (
              file,
              @"(?ms)""[^""]*""|\-\-\[\[.*?\]\]",
              delegate (Match m)
              {
                  if(m.Value.StartsWith("--[["))
                      return "";
                  return m.Value;
              }
            );

            return file;
        }
        private string RemoveSignleLineComment(string file)
        {
            file = Regex.Replace
            (
              file,
              @"(?ms)""[^""]*""|--.*?$",
              delegate (Match m)
              {
                  if(m.Value.StartsWith("--"))
                      return "";
                  return m.Value;
              }
            );

            return file;
        }
    }
}
