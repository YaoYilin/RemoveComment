using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RemoveComment
{
    public abstract class Commend
    {
        public Commend(string[] commends)
        {
            this.commends = commends;
        }
        internal abstract int commendLength { get; }
        internal string[] commends;
        internal abstract bool Do();
        public bool Execute()
        {
            if(commends.Length >= commendLength)
            {
                return Do();
            }
            else
            {
                Console.WriteLine("输入参数缺失，无法完成解析。");
                return false;
            }
        }
    }

    public class HelpCommend : Commend
    {
        public HelpCommend(string[] commends) : base(commends)
        {
        }

        internal override int commendLength
        {
            get
            {
                return 1;
            }
        }

        internal override bool Do()
        {
            Console.WriteLine(@"
-help 获取帮助
-file='file full path' -lang=c# 删除此文件内的注释
-folder='input folder' -lang=c# -ext=*.cs [-all=true] 删除此文件夹内的所有此语言的文件内的注释, -all是可选参数，默认为true，表示包含目录子文件夹
");
            return true;
        }
    }

    public class FileCommend : Commend
    {
        public FileCommend(string[] commends) : base(commends)
        {
        }

        internal override int commendLength
        {
            get
            {
                return 2;
            }
        }

        internal override bool Do()
        {
            var fileInfo = commends[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if(fileInfo.Length < 2)
            {
                Console.WriteLine("输入的参数错误，没有有效的文件路径。");
                return false;
            }
            string fileFullPath = fileInfo[1];
            if(!File.Exists(fileFullPath))
            {
                Console.WriteLine($"输入的参数错误，找不到对应文件 {fileFullPath}。");
                return false;
            }
            if(!commends[1].StartsWith("lang"))
            {
                Console.WriteLine("输入的参数错误，请输入需要操作的文件所属语言。");
                return false;
            }

            var langInfo = commends[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if(langInfo.Length < 2)
            {
                Console.WriteLine("输入的参数错误，请输入正确的语言参数，例如'-lang:C#'。");
                return false;
            }
            var remover = Factory.Creator(langInfo[1]);
            if(remover == null)
                return false;

            string code = File.ReadAllText(fileFullPath);
            var newCode = remover.Execute(code);
            
            bool isSuccess = Utility.OverWriteFile(fileFullPath, newCode);
            if(isSuccess)
                Console.WriteLine($"{fileFullPath} 内的注释删除完毕！");

            return isSuccess;
        }
    }

    public class FolderCommend : Commend
    {
        public FolderCommend(string[] commends) : base(commends)
        {
        }

        internal override int commendLength
        {
            get
            {
                return 3;
            }
        }

        internal override bool Do()
        {
            var fileInfo = commends[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if(fileInfo.Length < 2)
            {
                Console.WriteLine("输入的参数错误，没有有效的文件夹路径。");
                return false;
            }
            string path = fileInfo[1];
            if(!Directory.Exists(path))
            {
                Console.WriteLine($"输入的参数错误，找不到对应目录： {path}。");
                return false;
            }
            if(!commends[1].StartsWith("lang"))
            {
                Console.WriteLine("输入的参数错误，请输入需要操作的文件所属语言。");
                return false;
            }

            var langInfo = commends[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if(langInfo.Length < 2)
            {
                Console.WriteLine("输入的参数错误，请输入正确的语言参数，例如'-lang:C#'。");
                return false;
            }

            var extInfo = commends[2].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if(langInfo.Length < 2)
            {
                Console.WriteLine("输入的参数错误，请输入正确的被操作的文件扩展名，例如'-ext=*.cs'。");
                return false;
            }
            var ext = extInfo[1];
            if(!Regex.IsMatch(ext, @"\*\.*"))
            {
                Console.WriteLine("输入的参数错误，请输入正确的被操作的文件扩展名，例如'-ext=*.cs'。");
                return false;
            }

            var remover = Factory.Creator(langInfo[1]);
            if(remover == null)
                return false;

            var all = true;
            if(commends.Length == 4)
            {
                var allInfo = commends[3].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if(allInfo.Length >= 2)
                    all = bool.Parse(allInfo[1]);
            }
            DirectoryInfo folder = new DirectoryInfo(path);
            var allFiles = folder.GetFiles(extInfo[1], all ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            foreach(var file in allFiles)
            {
                string code = File.ReadAllText(file.FullName);
                var newCode = remover.Execute(code);
                if(Utility.OverWriteFile(file.FullName, newCode))
                    Console.WriteLine($"{file.FullName} 内的注释删除完毕！");
            }

            return true;
        }
    }
}
