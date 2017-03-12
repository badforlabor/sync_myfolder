using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glob;

/*
 星号 * 匹配多个字符

问号 ？ 匹配单个字符

中括号 [ ] 匹配括号中的任一字符

# 开头表示注释

以叹号 ! 开头表示不忽略匹配的文件(目录)

/ 开头表示目录的最顶层

反斜杠 \ 用于转义字符(如 #、!、*)

以 / 结尾表示忽略该目录下所有文件

** 表示无限深度目录
 
 */

namespace sync_myfolder
{
    class GlobalIgnore
    {
        List<Glob.Glob> globs = new List<Glob.Glob>();
        public void Init()
        {
            globs.Clear();

            try
            {
                // 读取global_ignore.txt
                string file = "global_ignore.txt";
                string[] patterns = System.IO.File.ReadAllLines(file);
                foreach (var p in patterns)
                {
                    globs.Add(new Glob.Glob(p));
                    bool b = IsIgnore(p);
                    Console.WriteLine("{0}{1}", b, p);
                }

            }
            catch (Exception)
            { }
        }
        public bool IsIgnore(string s)
        {
            foreach (var glob in globs)
            {
                if (glob.IsMatch(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
