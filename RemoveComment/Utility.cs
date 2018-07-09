using System;
using System.IO;

namespace RemoveComment
{
    public static class Utility
    {
        public static bool OverWriteFile(string path, string content)
        {
            try
            {
                File.Delete(path);
                FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(content);
                sw.Close();
                fs.Close();
                return true;
            }
            catch(Exception e)
            {

                Console.WriteLine(e);
                return false;
            }

        }
    }
}
