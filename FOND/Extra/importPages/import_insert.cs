using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SQLite;
using System;
using System.Data.Common;
using System.Collections;
using Common;


namespace FOND.Extra.importPages
{
    public partial class import_insert : UserControl, import.import_page
    {
        bool nb = false;
        bool prb = false;
        delegate void container();
        delegate void autoClickContainer();
        autoClickContainer auc;
        container c;
        private static string[] tnames = new string[] { "workers", "material_theme", "speakerlvl", "regions", "parts" };
        private static string[] atnames = new string[] { "Сотрудники", "Виды преступлений", "Категории сотрудников", "Районы", "Подразделения" };
        private static string[] mtnames = new string[] { "materialway", "material_presentation", "quality", "type" };
        private static string[][] mtvalues = new string[][] { new string[] { "Для подъема престижа ОВД", "Информационный", "Критический", "Проблемный", "Профилактический" }, new string[] { "Видеоматериал", "Фотосъемка", "Текстовый материал", "Устная консультация" }, new string[] { "Высокое", "Среднее", "Низкое" }, new string[] { "Комментарий для печатных СМИ", "Комментарий за кадром", "Лайф", "Ничего", "Синхрон" } };
        private static string[] allColumnsAcces = new string[] { "Код", "Отвественный сотрудник", "Направленность материала", "Тема материала", "Вид материала", "Качество", "Район", "Подразделения-участники", "Аннотация", "Вид выступления", "Категория выступающего", "Должность выступающего", "ФИО выступающего", "Местонахождение файла материала", "Дата материала" };
        private static string[] allColumnsSqlite = new string[] { "id", "workers", "materialway", "material_theme", "material_presentation", "quality", "regions", "parts", "discription", "type", "speakerlvl", "curspeaker", "fio", "link", "date" };
        private static string[] nonText = new string[] { "workers", "material_theme", "regions", "parts", "curspeaker" };
        private static string[] smiSqlColumns = new string[] { "id", "cards_num", "smi", "date", "times", "link", "pu" };
        private static string[] smiAccColumns = new string[] { "Код", "Код_события", "Наименование", "Дата", "Количество повторов", "Файл материала", "При участии" };
        private static string[] regA = new string[] { "Центральный", "Региональный", "Местный", "Информ. агенство/электронные СМИ", "Телевидение", "Печать", "Радио", "Интернет" };
        private static string[] regS = new string[] { "centr", "reg", "reg", "another", "tv", "gaz", "rad", "internet" };
        public import_insert()
        {
            InitializeComponent();
        }
        public void load(import.nextButtonAutoClickHandler deleg)
        {
            new Thread(insertInBase).Start();
            auc = new autoClickContainer(deleg);
        }

        public bool nextButtonIsEnabled(import.buttonChangedHandler deleg)
        {
            c = new container(deleg);
            return nb;
        }

        public bool prevButtonIsEnabled(import.buttonChangedHandler deleg)
        {
            c = new container(deleg);
            return prb;
        }

        public bool worker()
        {
            return true;
        }
        private void insertInBase()
        {
            try
            {
                for (int i = 0; i < atnames.Length; i++)
                {
                    OleDbCommand oledbcomm = new OleDbCommand("SELECT count(*) from [" + atnames[i] + "]", import.OleDbconn);
                    int countL = (int)oledbcomm.ExecuteScalar();
                    itemWithId[] iWD = new itemWithId[countL];
                    oledbcomm = new OleDbCommand("SELECT * from [" + atnames[i] + "]", import.OleDbconn);
                    DbDataReader ddr = oledbcomm.ExecuteReader();
                    PB2TextValue(0, atnames[i]);
                    int p = 0;
                    foreach (DbDataRecord dr in ddr)
                    {
                        int id = ddr.GetInt32(ddr.GetOrdinal("код"));
                        string value = "";
                        switch (atnames[i])
                        {
                            case "Сотрудники":
                                value = ddr["Сотрудник"].ToString();
                                break;
                            case "Виды преступлений":
                                value = ddr["Вид_преступления"].ToString();
                                break;
                            case "Вид материала":
                                value = ddr["Вид_материала"].ToString();
                                break;
                            case "Категории сотрудников":
                                value = ddr["Наименование"].ToString();
                                break;
                            case "Районы":
                                value = ddr["РАЙОН"].ToString();
                                break;
                            case "Подразделения":
                                value = ddr["Поле1"].ToString();
                                break;
                        }
                        iWD[p] = new itemWithId(id, value);
                        PB2TextValue(p, countL - 1, atnames[i]);
                        p++;
                    }
                    insertInBd(tnames[i], iWD);
                    pbh(6);
                }
                for (int i = 0; i < mtnames.Length; i++)
                {
                    int p = 0;
                    itemWithId[] iWD = new itemWithId[mtvalues[i].Length];
                    PB2TextValue(0, mtnames[i]);
                    foreach (string s in mtvalues[i])
                    {
                        iWD[p] = new itemWithId(p, s);
                        PB2TextValue(p, mtvalues[i].Length - 1, mtnames[i]);
                        p++;
                    }
                    insertInBd(mtnames[i], iWD);
                }
                PB2TextValue(0, "smi");
                SQLiteCommand com = new SQLiteCommand("CREATE TABLE `smi` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`name`	TEXT,`type`	TEXT NOT NULL,`place`	TEXT NOT NULL); ", import.conn);
                com.ExecuteNonQuery();
                OleDbCommand comm = new OleDbCommand("SELECT count(*) FROM [Имя_СМИ];", import.OleDbconn);
                int count = (int)comm.ExecuteScalar();
                comm.CommandText = "SELECT * FROM [Имя_СМИ];";
                DbDataReader ddr2 = comm.ExecuteReader();
                int c = 0;
                foreach (DbDataRecord dr in ddr2)
                {
                    com = new SQLiteCommand(string.Format("INSERT INTO smi (id,name,place,type) VALUES ('{0}','{1}','{2}','{3}');", dr["Код"], dr["Наименование"].ToString().realEscapeString(), placeAtoS(dr["Регион"].ToString()), placeAtoS(dr["Вид"].ToString())), import.conn);
                    com.ExecuteNonQuery();
                    PB2TextValue(c, count, "Имя_СМИ");
                    c++;
                }
                pbh(14);
                PB2TextValue(0, "Main_Table");
                comm = new OleDbCommand("SELECT COUNT(*) FROM [Main_Table]", import.OleDbconn);
                count = (int)comm.ExecuteScalar();
                comm = new OleDbCommand("SELECT * FROM [Main_Table]", import.OleDbconn);
                ddr2 = comm.ExecuteReader();
                c = 0;
                com = new SQLiteCommand("CREATE TABLE `cards` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`workers`	TEXT,`materialway`	TEXT,`material_theme`	TEXT,`material_presentation`	TEXT,`quality`	TEXT,`regions`	TEXT,`parts`	TEXT,`discription`	TEXT,`type`	TEXT,`speakerlvl`	TEXT,`curspeaker`	TEXT,`fio`	TEXT,`link`	TEXT,`date`	TEXT)", import.conn);
                com.ExecuteNonQuery();
                foreach (DbDataRecord dr in ddr2)
                {
                    if (c % (count / 24) == 0)
                    {
                        pbh(1);
                    }
                    PB2TextValue(c, count, "Main_Table");
                    c++;
                    string requestTbl = "";
                    string requestVle = "";
                    foreach (string s in allColumnsAcces)
                    {
                        string sqliteTN = fromAccessToSQlite(s, FATS.fromAtoS);
                        requestTbl += "[" + sqliteTN + "],";
                        requestVle += "'" + txtMassToIndexes(dr[s].ToString(), sqliteTN).realEscapeString() + "',";
                    }
                    com = new SQLiteCommand("INSERT INTO cards (" + requestTbl.cutLastSymbol() + ") VALUES (" + requestVle.cutLastSymbol() + ");", import.conn);
                    com.ExecuteNonQuery();
                }
                PB2TextValue(0, "card_in_smi");
                com = new SQLiteCommand("CREATE TABLE `card_in_smi` (`id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`cards_num`	INTEGER,`smi`	TEXT,`date`	TEXT,`times`	INTEGER,`link`	TEXT,`pu` TEXT); ", import.conn);
                com.ExecuteNonQuery();
                ddr2.Close();
                comm.CommandText = "SELECT COUNT(*) FROM [СМИ]";
                count = (int)comm.ExecuteScalar();
                comm.CommandText = "SELECT * FROM [СМИ]";
                ddr2 = comm.ExecuteReader();
                c = 0;
                foreach (DbDataRecord dr in ddr2)
                {
                    if (c % count / 24 == 0)
                    {
                        pbh(1);
                    }
                    c++;
                    PB2TextValue(c, count, "cards_in_smi");
                    string Trequest = "";
                    string Vrequest = "";
                    foreach (string s in smiAccColumns)
                    {
                        try
                        {
                            string tName = smiAtoS(s);
                            Trequest += tName + ",";
                            Vrequest += "'" + getValue(dr, s).realEscapeString() + "'" + ",";
                        }
                        catch (Exception e)
                        {
                            new lastlog().add("select from smi: " + e.Message);
                        }
                    }
                    com = new SQLiteCommand("INSERT INTO card_in_smi (" + Trequest.cutLastSymbol() + ") VALUES (" + Vrequest.cutLastSymbol() + ");", import.conn);
                    com.ExecuteNonQuery();
                }
                pbs(100);
                nb = true;
                this.c?.Invoke();
                auc ?.Invoke();
            }
            catch (Exception e)
            {
                new lastlog().add("import: " + e.Message);
                MessageBox.Show("Ошибка подключения!\n" + e.Message, "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string getValue(DbDataRecord dr, string AccColumnName)
        {
            switch (AccColumnName)
            {
                case "Наименование":
                    SQLiteCommand comm = new SQLiteCommand("SELECT id FROM " + smiAtoS(AccColumnName) + " WHERE name = '" + dr[AccColumnName] + "';", import.conn);
                    var res = comm.ExecuteScalar();
                    if (res != null)
                    {
                        return res.ToString();
                    }
                    else
                    {
                        comm.CommandText = string.Format("INSERT INTO smi (name,place,type) VALUES ('{0}','{1}','{2}');", dr["Наименование"].ToString().realEscapeString(), placeAtoS(dr["Регион"].ToString()), placeAtoS(dr["Тип СМИ"].ToString()));
                        comm.ExecuteNonQuery();
                        comm.CommandText = "SELECT MAX(id) FROM smi;";
                        return comm.ExecuteScalar().ToString();
                    }
                case "При участии":
                    return (bool)dr[AccColumnName] == true ? "true" : "";
                case "Дата":
                    return txtMassToIndexes(dr[AccColumnName].ToString(), "date");
                default:
                    if (dr[AccColumnName] != DBNull.Value)
                        return dr[AccColumnName].ToString();
                    else return "";
            }
        }
        public static string placeAtoS(string value)
        {
            ArrayList a = new ArrayList(regA);
            if (a.Contains(value))
            {
                return regS[a.IndexOf(value)];
            }
            return "";
        }
        public static string fromAccessToSQlite(string tableName, FATS f)
        {
            ArrayList access = new ArrayList(allColumnsAcces);
            ArrayList sqlite = new ArrayList(allColumnsSqlite);
            if (f == FATS.fromAtoS)
            {
                if (access.Contains(tableName))
                    return (string)sqlite[access.IndexOf(tableName)];
            }
            else
            {
                if (sqlite.Contains(tableName))
                    return (string)access[sqlite.IndexOf(tableName)];
            }
            return "";
        }
        public static string smiAtoS(string tblName)
        {
            ArrayList A = new ArrayList(smiAccColumns);
            ArrayList S = new ArrayList(smiSqlColumns);
            if (A.Contains(tblName))
                return (string)S[A.IndexOf(tblName)];
            return "";
        }
        /// <summary>
        /// From Access To Sqlite returnType
        /// </summary>
        public enum FATS
        {
            fromAtoS,
            fromStoA
        }
        private void insertInBd(string tableName, itemWithId[] values)
        {
            SQLiteCommand comm = new SQLiteCommand("CREATE TABLE `" + tableName + "`  (`value` TEXT NOT NULL , `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ); ", import.conn);
            comm.ExecuteNonQuery();
            foreach (itemWithId iWD in values)
            {
                comm = new SQLiteCommand("INSERT INTO " + tableName + "  ( id, value ) VALUES (" + iWD.id + ",'" + Common.SqliteCommon.realEscapeString(iWD.value) + "')", import.conn);
                comm.ExecuteNonQuery();
            }
        }
        private static string txtMassToIndexes(string param, string tblName)
        {
            ArrayList a = new ArrayList(nonText);
            if (a.Contains(tblName))
            {
                string[] s = param.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string result = "";
                foreach (string sf in s)
                {
                    result += sf + ";";
                }
                return result;
            }
            a = new ArrayList(mtnames);
            if (a.Contains(tblName))
            {
                string[] s = param.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string result = "";
                foreach (string sf in s)
                {
                    SQLiteCommand comm = new SQLiteCommand("SELECT id FROM " + tblName + " WHERE value = '" + sf.realEscapeString() + "';", import.conn);
                    result += comm.ExecuteScalar().ToString() + ";";
                }
                return result;
            }
            if (tblName == "date")
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(param.Substring(0, 10), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return dt.ToString("yyyy-MM-dd");
                }
                catch (Exception e)
                {
                    new lastlog().add("import->datetime: " + e.Message);
                }
            }
            return param;
        }
        public bool workerBack()
        {
            if (import.OleDbconn != null)
            {
                import.OleDbconn.Close();
                import.OleDbconn.Dispose();
                import.OleDbconn = null;
            }
            if (import.conn != null)
            {
                import.conn.Close();
                import.conn.Dispose();
                import.conn = null;
            }
            return true;
        }
        private struct itemWithId
        {
            public int id;
            public string value;
            public itemWithId(int _id, string _value)
            {
                id = _id;
                value = _value;
            }
        }
        private delegate void pb(int value);
        private delegate void pbV();
        private void pbh(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                this.Invoke(new pb(pbh), new object[] { value });
            }
            else
            {
                progressBar1.Value += value;
            }
        }
        private void pbs(int value)
        {
            progressBar1.Invoke(new pbV(delegate { progressBar1.Value = value; }));
        }
        private void PB2TextValue(int value, string text)
        {
            progressBar2.Invoke(new pbV(delegate { progressBar2.Value = value; }));
            label2.Invoke(new pbV(delegate { label2.Text = text; }));
        }
        private void PB2TextValue(int currValue, int maxValue, string tableName)
        {
            progressBar2.Invoke(new pbV(delegate { progressBar2.Value = (currValue * 100) / maxValue; }));
            label2.Invoke(new pbV(delegate { label2.Text = string.Format("Прогресс {0}: {1}/{2}", tableName, currValue, maxValue); }));
        }

    }
}