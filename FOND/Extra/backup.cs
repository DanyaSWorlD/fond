using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace FOND.Extra
{
    class backup
    {
        ToolStripMenuItem item1;
        ToolStripMenuItem item2;
        ContextMenuStrip menu1;
        NotifyIcon noIco;
        Common.lastlog lg;
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
            if (noIco == null)
            {
                noIco = new NotifyIcon();
                noIco.Visible = true;
                noIco.Icon = Properties.Resources.yavbttkp;
                noIco.BalloonTipTitle = "FOND - Резервное копирование";
                noIco.Click += new EventHandler(icon_click);
            }
            if (menu1 == null)
            {
                menu1 = new ContextMenuStrip();
                menu1.ImageScalingSize = new System.Drawing.Size(20, 20);
                menu1.Name = "menu1";
                menu1.Size = new System.Drawing.Size(182, 58);
                if (item1 == null)
                {
                    item1 = new ToolStripMenuItem();
                    item1.Name = "open";
                    item1.Size = new System.Drawing.Size(186, 26);
                    item1.Text = "открыть форму";
                }
                if(item2 == null)
                {
                    item2 = new ToolStripMenuItem();
                    item2.Name = "close";
                    item2.Size = new System.Drawing.Size(186, 26);
                    item2.Text = "отключить приложение";
                }
                if(!connector.getInstance().appRunning)
                {
                    menu1.Items.Add(item1);
                }
                menu1.Items.Add(item2);
                menu1.ItemClicked += new ToolStripItemClickedEventHandler(menuItemClick);
            }
            string bd_file = Properties.Settings.Default.db_file_dir;
            string bd_backup_file = Properties.Settings.Default.backup_file_dir;
            lg = new Common.lastlog();
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
                            if (MainForm.dbCheck(bd_file))
                            {
                                File.Delete(bd_backup_file);
                                Thread.Sleep(60000);
                                File.Copy(bd_file, bd_backup_file);
                                f = new FileInfo(bd_backup_file);
                                noIco.BalloonTipText = "Резервное копирование завершно\nРазмер файла: " + f.Length / 1024 + "кб";
                                noIco.ShowBalloonTip(3000);
                                noIco.Dispose();
                                bu b = new bu(Backup);
                                b.Invoke();

                            }
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
            else
            {
                noIco.BalloonTipText = "Резервное копирование будет активировано после заполнения базы данных";
                noIco.ShowBalloonTip(3000);
                connector.getInstance().bd_formClosed += new connector.bd_formClosedEventHandler(Backup);
            }
        }
        private void mainform_start()
        {
                if(!connector.getInstance().appRunning)
            {
                MainForm m = new MainForm();
                m.Show();
            }
        }
        private void icon_click(object sender, EventArgs e)
        {
            menu1.Show(Control.MousePosition);
        } 
        private void menuItemClick(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == item1)
            {
                mainform_start();
            }
            else
            {
                noIco.Visible = false;
                Thread.CurrentThread.Abort();
            }
        }
    }
}
