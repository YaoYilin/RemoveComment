using System;

namespace RemoveComment
{
    public class CommendParser
    {
        public static Commend Parse(string input)
        {
            var commends = input.Replace(" ", "").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if(commends.Length <= 0)
            {
                Console.WriteLine("输入的参数错误。");
                return null;
            }
            if(commends[0].StartsWith("help"))
                return new HelpCommend(commends);
            if(commends[0].StartsWith("file"))
                return new FileCommend(commends);
            if(commends[0].StartsWith("folder"))
                return new FolderCommend(commends);

            return null;
        }
    }
}
