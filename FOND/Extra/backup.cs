using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace FOND.Extra
{
    class backup
    {
        public backup()
        {
            bool IFC;
            Mutex m = new Mutex(true, "backup_fond", out IFC);
            if(IFC)
            {
                new Thread(Backup).Start();
            }
        }
        public static string encriptor(string param)
        {
            return (param.Substring(2)).Replace('\\', '_');
        }
        private delegate void bu();
        private void Backup()
        {
            NotifyIcon noIco = new NotifyIcon();
            noIco.Visible = true;
            noIco.Icon = Properties.Resources.yavbttkp;
            noIco.BalloonTipTitle = "FOND - Резервное копирование";
            string bd_file = Properties.Settings.Default.db_file_dir;
            string bd_backup_file = Properties.Settings.Default.backup_file_dir;
            Common.lastlog lg = new Common.lastlog();
            if(bd_file!="")
            {
                if(bd_backup_file!="")
                {
                    FileInfo f = new FileInfo(bd_backup_file);
                    var since_f = DateTime.Now - f.CreationTime;
                    var since_y = DateTime.Now - DateTime.Now.AddDays(-1);
                    if(since_f<since_y)
                    {
                        var ba = since_y - since_f;
                        noIco.BalloonTipText = "Резервная копия будет создана через " + ba.Hours + "ч. " + ba.Minutes + "мин.";
                        noIco.ShowBalloonTip(3000);
                        Thread.Sleep((int)ba.TotalMilliseconds);
                        noIco.Dispose();
                        bu b = new bu(Backup);
                        b.Invoke();
                    }
                    else
                    {
                        try
                        {
                            noIco.BalloonTipText = "Начато";
                            noIco.ShowBalloonTip(3000);
                            Thread.Sleep(10000);
                            File.Delete(bd_backup_file);
                            File.Copy(bd_file, bd_backup_file);
                            f = new FileInfo(bd_backup_file);
                            noIco.BalloonTipText = "Резервное копирование завершно\nРазмер файла: " + f.Length / 1024 + "кб";
                            noIco.ShowBalloonTip(3000);
                        }
                        catch(Exception e) {
                            lg.add("backup: " + e.Message);
                            noIco.BalloonTipText = "Резервное копирование не завершно\n" + e.Message;
                            noIco.ShowBalloonTip(3000);
                        }
                    }
                }
                else
                {
                                        
                    try
                    {
                        noIco.BalloonTipText = "Начато";
                        noIco.ShowBalloonTip(3000);
                        Thread.Sleep(10000);
                        bd_backup_file = Directory.GetCurrentDirectory() + "/backup";
                        if(!Directory.Exists(bd_backup_file))
                        {
                            Directory.CreateDirectory(bd_backup_file);
                        }
                        bd_backup_file += "/" + encriptor(bd_file);
                        File.Copy(bd_file, bd_backup_file);
                        FileInfo f = new FileInfo(bd_backup_file);
                        noIco.BalloonTipText = "Резервное копирование завершно\nРазмер файла: " + f.Length / 1024 + "кб";
                        noIco.ShowBalloonTip(3000); 
                    }
                    catch (Exception e)
                    {
                        lg.add("backup: " + e.Message);
                        noIco.BalloonTipText = "Резервное копирование не завершно\n"+e.Message;
                        noIco.ShowBalloonTip(3000);
                    }
                }
            }
        }
    }
}
