using System;

namespace RemoveComment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Copyright(C) 2018 All Rights Reserved

欢迎使用代码注释删除工具。
输入-help获取使用帮助。
目前支持语言：c、c++、c#、java、oc、php、js、lua，后续会继续扩展。
");
            while(true)
            {
                var input = Console.ReadLine();
                var commend = CommendParser.Parse(input);
                if(commend != null)
                {
                    commend.Execute();
                }
            }

        }
    }
}
