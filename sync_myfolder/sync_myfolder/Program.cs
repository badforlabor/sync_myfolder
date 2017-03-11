using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace sync_myfolder
{
    class Program
    {
        static void Main(string[] args)
        {
            string src_folder = @"E:\liubo\github\sync_myfolder\test\1\";
            string dst_folder = @"E:\liubo\github\sync_myfolder\test\2\";
                       
            
            Sync(src_folder, dst_folder);

            Console.ReadKey();
        }
        static void Sync(string src, string dst)
        {
            if (!Directory.Exists(src) || !Directory.Exists(dst))
            {
                Console.WriteLine("目标文件夹不存在，退出");
                return;
            }

            List<string> MyRecord = new List<string>();
            SyncImpl(MyRecord, src, dst);
            File.WriteAllLines(string.Format("{0}-{1}-{2} {3}-{4}-{5}.txt", DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day,
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), MyRecord);
        }
        static void SyncImpl(List<string> record,string src, string dst)
        {
            Console.WriteLine("sync folder [{0}] => [{1}]", src, dst);

            if (!Directory.Exists(dst))
                Directory.CreateDirectory(dst);

            string[] files = Directory.GetFiles(src);
            string[] dst_files = Directory.GetFiles(dst);

            // 删除的
            foreach (var df in dst_files)
            {
                string shortname = Path.GetFileName(df);
                int idx = files.ToList().FindIndex(delegate(string f1)
                {
                    return Path.GetFileName(f1) == shortname;
                });
                if (idx == -1)
                {
                    // 删除的
                    DoDelete(df);
                    record.Add(string.Format("[-]{0}", df));
                }
            }

            // 修改的
            foreach (var f in files)
            {
                string shortname = Path.GetFileName(f);
                int idx = dst_files.ToList().FindIndex(delegate(string f2)
                {
                    return Path.GetFileName(f2) == shortname;
                });
                if (idx == -1)
                {
                    string dst_file = Path.Combine(dst, shortname);
                    // 增加的
                    DoCopy(f, dst_file);
                    record.Add(string.Format("[+]{0}", dst_file));
                }
                else
                {
                    DateTime dt = File.GetLastWriteTime(f);
                    DateTime dt2 = File.GetLastWriteTime(dst_files[idx]);
                    if (dt != dt2)
                    {
                        // 修改
                        DoCopy(f, dst_files[idx]);
                        record.Add(string.Format("[m]{0}", dst_files[idx]));
                    }
                }
            }

            string[] folders = Directory.GetDirectories(src);
            string[] dst_folders = Directory.GetDirectories(dst);
            
            // 删除的文件夹
            {
                foreach (var df in dst_folders)
                {
                    string df_shortname = df.Substring(dst.Length);
                    int idx = folders.ToList().FindIndex(delegate(string f1)
                    {
                        return f1.Substring(src.Length) == df_shortname;
                    });
                    if (idx == -1)
                    {
                        DoDelete(df);
                        record.Add(string.Format("[-]{0}", df));
                    }
                }
            }


            // 递归检索下一层
            foreach (var f in folders)
            {
                string foldername = f.Length > src.Length ? f.Substring(src.Length) : f;
                SyncImpl(record, f, dst + foldername);
            }

        }
        static void DoCopy(string src, string dst)
        {
            try
            {
                File.Copy(src, dst, true);
            }
            catch (Exception)
            { }
        }
        static void DoMove(string src, string dst)
        {
            try
            {
                File.Move(src, dst);
            }
            catch (Exception)
            { }
        }
        static void DoDelete(string f)
        {
            try 
            {
                if (Directory.Exists(f))
                    Directory.Delete(f, true);
                else
                    File.Delete(f);
            }
            catch (Exception) { }
        }
    }
}
