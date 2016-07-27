using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Linq;
using System.Data.Common;
using System.Threading;
using System.Data;
using System.ComponentModel;
using Common;


namespace FOND
{

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //load zone
    public partial class RedForm : Form
    {
        public Control[] controls;
        public SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source = {0};", Properties.Settings.Default.db_file_dir));
        public bool needload;
        public string[,] outarr = new string[2, 10];
        public string[,] outarr2 = new string[2, 10];
        public int outarri = 0;
        public int outarri2 = 0;
        public lastlog lg = new lastlog();
        private long id = -1;
        private string cnames, invals;
        private int ended = 0;
        private string savemode;
        private int goWay;
        private int minId;
        private int maxId;

        public RedForm(bool isnew)
        {
            InitializeComponent();
            smiStats3.setDisplayMode(UserControls.smiStats.smiStatsDisplayngModel.onlyResult);
            dataGridView1.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView1_CellMouseEnter);
            dataGridView2.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView1_CellMouseEnter);
            dataGridView3.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView1_CellMouseEnter);
            conn.Open();
            needload = !isnew;
        }

        private void RedForm_Load(object sender, EventArgs e)
        {
            tableLayoutPanel6.Visible = needload;
            button3.Visible = !needload;
            lg.add("RedForm_Load->loadstart");
            loadstart();
        }

        private void getLastId()
        {
            //lg.add("Tread debug: getLastId func started");
            if (id == -1)
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT MAX(id) FROM cards", conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                foreach (DbDataRecord dbdr in dr)
                {
                    id = (int)(long)dbdr[0];
                }
            }
            MultyThreadChangeId(id);
            loaddata();
        }

        #region DATABASE CHECKER

        private void loadstart()
        {
            pictureBox1.Visible = true;
            outarri = 0;
            getMnMId();
            lg.add("loadstart -> databasechecker");
            new Thread(databasechecker).Start();

        }
        private void databasechecker()
        {
            lg.add("databasechecker started");
            SQLiteCommand comm = new SQLiteCommand("SELECT name FROM sqlite_master WHERE name = 'cards'", conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            var i = 0;
            foreach (DbDataRecord rec in dr)
            {
                i++;
            }
            if (i == 0)
            {
                errshow();
            }
            else
            {
                lg.add("databasechecker -> datloader");
                datLoader();
                if (needload == true)
                {
                    getLastId();
                }

            }

        }
        private void errshow()
        {
            var rez = MessageBox.Show("Внимание!!!", "Таблица \"Карточки\" не найдена, при этом другие таблицы присутствуют. Вы уверены что вы открыли ту базу данных? Проверьте базу данных через мастер настройки, и если проблема не решится обратитесь в службу поддержки", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            switch (rez)
            {
                case DialogResult.Retry:
                    databasechecker();
                    break;
                case DialogResult.Abort:
                    this.Close();
                    this.Dispose();
                    break;
            }
        }
        #endregion

        #region STOP LOAD FUNCTION
        delegate void stoploadind();
        /// <summary>
        /// Hiding loading indicator
        /// </summary>
        private void MultyThreadstoploadind()
        {
            if (pictureBox1.InvokeRequired)
            {
                stoploadind stp = new stoploadind(MultyThreadstoploadind);
                this.Invoke(stp, new object[] { });
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }
        #endregion

        #region LOADING VALUE TO ALL COMBO BOXES
        private void datLoader()
        {
            lg.add("Tread debug: datLoader started");
            lg.add("datLoader->dbdataadapterload");
            new Thread(dbdataadapterload).Start();
            foreach (Control cmb in this.tableLayoutPanel1.Controls.OfType<ComboBox>())
            {
                datloadersw(cmb, 0);
            }

        }
        /// <summary>
        /// Приравнивает имена элементов формы таблицам базы данных
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="from"></param>
        private void datloadersw(Control cmb, int from)
        {
            string request = "";
            switch (cmb.Name)
            {
                case "работники":
                    request = "workers";
                    break;
                case "направление":
                    request = "materialway";
                    break;
                case "тема":
                    request = "material_theme";
                    break;
                case "вид":
                    request = "material_presentation";
                    break;
                case "качество":
                    request = "quality";
                    break;
                case "регион":
                    request = "regions";
                    break;
                case "части":
                    request = "parts";
                    break;
                case "описание":
                    request = "discription";
                    break;
                case "вид_выступления":
                    request = "type";
                    break;
                case "категория":
                    request = "speakerlvl";
                    break;
                case "должность":
                    request = "curspeaker";
                    break;
                case "фио":
                    request = "fio";
                    break;
                case "ссылка":
                    request = "link";
                    break;
                case "textBox2":
                    request = "date";
                    break;
                default:
                    request = "";
                    break;
            }
            if (request != "")
            {
                if (from == 1)
                {
                    dbsaveconinue(request, cmb);
                }
                else
                {
                    if (from == 2)
                    {
                        continuetextbox(request, cmb);
                    }
                    else
                    {
                        datloader2(request, cmb);
                    }
                }

            }
        }
        private void datloader2(String request, Control cmb)
        {
            SQLiteCommand comm = new SQLiteCommand("SELECT value,id FROM " + request, conn);
            SQLiteDataReader dr0 = comm.ExecuteReader();
            var i = 0;
            foreach (DbDataRecord rec in dr0)
            {
                i++;
            }
            combovalue[] cmbvl = new combovalue[i];
            i = 0;
            dr0.Close();
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord rec in dr)
            {
                cmbvl[i] = new combovalue();
                cmbvl[i].number = (int)(long)rec["id"];
                cmbvl[i].value = rec["value"].ToString();
                i++;

            }
            MultyThreadCCbChangeItem(cmb, cmbvl);
        }
        delegate void additem(Control cmb, combovalue[] cmbvl);
        private void MultyThreadCCbChangeItem(Control cmb, combovalue[] cmbvl)
        {
            if (cmb.InvokeRequired)
            {
                additem ad = new additem(MultyThreadCCbChangeItem);
                this.Invoke(ad, new object[] { cmb, cmbvl });
            }
            else
            {
                (cmb as ComboBox).Items.Clear();
                foreach (combovalue zn in cmbvl)
                {
                    (cmb as ComboBox).Items.Add(zn);
                }


            }
        }
        #endregion

        #region LOADING DATA TO CARD FORM FUNC AND IT'S REQUAREMENTS

        private void loaddata()
        {
            lg.add("Thread debug: load data function started");
            if (id != -1)
            {
                SQLiteCommand comm = new SQLiteCommand("SELECT * FROM cards WHERE id = " + id, conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    foreach (DbDataRecord ddr in dr)
                    {
                        for (var i = 1; i < ddr.FieldCount; i++)
                        {
                            if (ddr.GetValue(i).GetType() != Type.GetType("System.DBNull"))
                            {
                                switch (ddr.GetName(i))
                                {
                                    case "workers":
                                        select(работники, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "materialway":
                                        select(направление, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "material_theme":
                                        select(тема, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "material_presentation":
                                        select(вид, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "quality":
                                        select(качество, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "regions":
                                        select(регион, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "parts":
                                        select(части, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "discription":
                                        SetText(описание, ddr.GetString(i));
                                        break;
                                    case "type":
                                        select(вид_выступления, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "speakerlvl":
                                        select(категория, ddr.GetName(i), ddr.GetString(i));
                                        break;
                                    case "curspeaker":
                                        SetText(должность, ddr.GetString(i));
                                        break;
                                    case "fio":
                                        SetText(фио, ddr.GetString(i));
                                        break;
                                    case "link":
                                        SetText(ссылка, ddr.GetString(i));
                                        break;
                                    case "date":
                                        SetText(textBox2, ddr.GetString(i));
                                        break;
                                }
                            }
                            else
                            {
                                switch (ddr.GetName(i))
                                {
                                    case "discription":
                                        SetText(описание, "");
                                        break;
                                    case "curspeaker":
                                        SetText(должность, "");
                                        break;
                                    case "fio":
                                        SetText(фио, "");
                                        break;
                                    case "link":
                                        SetText(ссылка, "");
                                        break;
                                    case "date":
                                        SetText(textBox2, "");
                                        break;
                                }
                            }
                        }
                    }
                    loadInDataGridView();
                }
                else
                {
                    if (goWay == 2)
                    {
                        lg.add("Regform: not same card in base. Cant show to user, but can print it here.");
                        MultyThreadstoploadind();
                    }
                    else
                    {
                        if (id > minId && id < maxId)
                        {
                            id = id + goWay;
                            MultyThreadChangeId(id);
                            new Thread(loaddata).Start();
                        }
                    }



                }
            }
        }
        delegate void st(TextBox tb, string txt);
        private void SetText(TextBox tb, string txt)
        {
            if (tb.InvokeRequired)
            {
                st st = new st(SetText);
                this.Invoke(st, new object[] { tb, txt });
            }
            else
            {
                tb.Text = txt;
            }

        }
        /// <summary>
        /// Выделяет те строки в чекбокс комбобоксе, которые были выделены при сохранении карточки
        /// </summary>
        /// <param name="cbcb">ЧекБоксКомбоБокс, в котором нужно выделить</param>
        /// <param name="table">Таблица, в которой искать</param>
        /// <param name="id">Строка индексов из таблицы сards</param>
        private void select(PresentationControls.CheckBoxComboBox cbcb, string table, string _id)
        {
            int[] indexes = getIndex(table, _id);
            for (int i = 0; i < indexes.Length; i++)
            {
                SetChecked(cbcb, indexes[i]);
            }

        }
        delegate void sc(PresentationControls.CheckBoxComboBox cbcb, int index);
        private void SetChecked(PresentationControls.CheckBoxComboBox cbcb, int index)
        {
            if (cbcb.InvokeRequired)
            {
                sc sc = new sc(SetChecked);
                this.Invoke(sc, new object[] { cbcb, index });
            }
            else
            {
                cbcb.CheckBoxItems[index + 1].Checked = true;
            }
        }
        delegate void sc2(ComboBox cb, int index);
        private void SetChecked(ComboBox cb, int index)
        {
            if (cb.InvokeRequired)
            {
                sc2 sc = new sc2(SetChecked);
                this.Invoke(sc, new object[] { cb, index });
            }
            else
                cb.SelectedIndex = index;
        }
        /// <summary>
        /// Вибирает то значение, которое было выделено при сохранении карточки
        /// </summary>
        /// <param name="cb">КомбоБокс, в котором нужно выделить</param>
        /// <param name="table">Таблица, в которой искать</param>
        /// <param name="_id">Строка индексов из таблицы сards</param>
        private void select(ComboBox cb, string table, string _id)
        {
            int[] indexes = getIndex(table, _id);
            if (indexes.Length != 0)
                SetChecked(cb, indexes[0]);
        }
        /// <summary>
        /// Возвращает индексы строк (чекбокс-)комбобокса которые были записаны в базу.
        /// </summary>
        /// <param name="table">Таблица, в которой искать</param>
        /// <param name="id">Строка индексов из таблицы сards</param>
        /// <returns>Возвращает индексы строк (чекбокс-)комбобокса которые были записаны в базу</returns>
        private int[] getIndex(string table, string _id)
        {
            int index = 0;
            int[] value = new int[0];
            string[] str = _id.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM " + table + ";", conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ddr["id"].ToString())
                    {
                        value = rewritearray(value, new int[value.Length + 1]);
                        value[value.Length - 1] = index;
                    }
                }
                index++;
            }
            return value;
        }
        private int[] rewritearray(int[] oldarray, int[] newarray)
        {
            for (int i = 0; i < oldarray.Length; i++)
            {
                newarray[i] = oldarray[i];
            }
            return newarray;
        }
        #endregion

        //end of load zone
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //work zone

        private void button2_Click(object sender, EventArgs e)
        {
            savemode = "";
            if (id == -1)
            {
                savemode = "save";
                cardsave();
            }
            else
            {
                savemode = "update";
                cardsave();
            }
        }
        private int end = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            end = 1;
            savemode = "";
            if (id == -1)
            {
                savemode = "save";
                cardsave();
            }
            else
            {
                savemode = "update";
                cardsave();
            }
        }

        #region CARD SAVING REGION
        private void cardsave()
        {
            pictureBox1.Visible = true;
            ended = 0;
            outarri = 0;
            outarri2 = 0;
            outarr = new string[2, 10];
            outarr2 = new string[2, 10];
            new Thread(newthreadsave).Start();
            new Thread(newthreadsavetxtbox).Start();
        }

        private void newthreadsave()
        {
            foreach (Control cb in this.tableLayoutPanel1.Controls.OfType<ComboBox>())
            {
                datloadersw(cb, 1);
            }
            ending("combobox");
        }
        private void newthreadsavetxtbox()
        {
            foreach (Control cb in this.tableLayoutPanel1.Controls.OfType<TextBox>())
            {
                datloadersw(cb, 2);
            }
            ending("textbox");
        }
        delegate void getitem(string req, Control cmb);
        private void MultyThreadCCbGetItem(string req, Control cmb)
        {
            string res;
            if (cmb.InvokeRequired)
            {
                getitem gt = new getitem(MultyThreadCCbGetItem);
                this.Invoke(gt, new object[] { req, cmb });
            }
            else
            {
                res = "";
                ComboBox cbcb = cmb as ComboBox;
                string[] str = cbcb.Text.Split(',');
                if (str[0] != "")
                {
                    foreach (string st in str)
                    {
                        if (st.ToCharArray()[0] == ' ')
                        {
                            for (var i = 0; i < cbcb.Items.Count; i++)
                            {
                                if (i == 0 && cbcb.GetType() == работники.GetType())
                                {
                                    i++;
                                }
                                if (cbcb.Items[i].ToString() == st.Substring(1, st.Length - 1))
                                {
                                    res += (cbcb.Items[i] as combovalue).number.ToString() + ";";
                                }
                            }
                        }
                        else
                        {
                            for (var i = 0; i < cbcb.Items.Count; i++)
                            {
                                if (i == 0 && cbcb.GetType() == работники.GetType())
                                {
                                    i++;
                                }
                                if (cbcb.Items[i].ToString() == st)
                                {
                                    res += (cbcb.Items[i] as combovalue).number.ToString() + ";";
                                }
                            }
                        }
                    }
                }
                dbsavecontinue2(req, cmb, res);

            }
        }
        delegate void close();
        private void MultyThreadClose()
        {
            if (this.InvokeRequired)
            {
                close c = new close(MultyThreadClose);
                this.Invoke(c);
            }
            else
            {
                this.Close();
            }
        }
        private void dbsaveconinue(string req, Control cmb)
        {

            MultyThreadCCbGetItem(req, cmb);

        }
        private void dbsavecontinue2(string req, Control cmb, string res)
        {
            if (res != "" && res != null)
            {
                outarr[0, outarri] = SqliteCommon.realEscapeString(req);
                outarr[1, outarri] = SqliteCommon.realEscapeString(res);
                lg.add("err debug: " + outarr[1, outarri]);
                outarri++;
            }
            else


                lg.add("err debug: this object selected items is null");


        }
        //textbox savings continue---------------------------------------------------------------------------------------------------------------------------------------
        private void continuetextbox(string request, Control cmb)
        {
            if (cmb.Text != "" && request != "" && cmb.Text != null)
            {
                outarr2[0, outarri2] = SqliteCommon.realEscapeString(request);
                outarr2[1, outarri2] = SqliteCommon.realEscapeString(cmb.Text);
                lg.add("err debug: " + request + "  value: " + cmb.Text);
                outarri2++;
            }
        }
        private void ending(string from)
        {
            object locker = new object();
            if (ended != 1)
            {
                ended = ended + 1;
                lg.add("err debug: первым завершился поток " + from);
            }
            else
            {
                lg.add("err debug: вторым завершился поток " + from);
                cnames = "";
                invals = "";
                for (var i = 0; i < outarri; i++)
                {
                    if (outarr[0, i] != null)
                    {
                        if (i == 0)
                        {
                            cnames = outarr[0, i];
                            invals = "'" + outarr[1, i] + "'";
                        }
                        else
                        {
                            cnames += "," + outarr[0, i];
                            invals += ",'" + outarr[1, i] + "'";
                        }
                    }
                }
                for (var i = 0; i < outarri2; i++)
                {
                    if (outarr2[0, i] != null)
                    {
                        if (cnames == null || cnames == "" || invals == null || invals == "")
                        {
                            cnames += outarr2[0, i];
                            invals += "'" + outarr2[1, i] + "'";
                        }
                        else
                        {
                            cnames += "," + outarr2[0, i];
                            invals += ",'" + outarr2[1, i] + "'";
                        }
                    }
                }
                if (cnames == "" || cnames == null)
                {
                    MessageBox.Show("заполните хоть однно поле", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Thread.Sleep(2000);
                    MultyThreadstoploadind();
                }
                else
                {
                    lock (locker)
                    {
                        try
                        {
                            if (savemode == "save")
                            {
                                string cnn = "INSERT INTO cards (" + cnames + ") VALUES ( " + invals + " );";
                                SQLiteCommand comm = new SQLiteCommand(cnn, conn);
                                comm.ExecuteNonQuery();
                                comm = new SQLiteCommand("SELECT MAX(id) FROM cards", conn);
                                SQLiteDataReader dr = comm.ExecuteReader();
                                foreach (DbDataRecord dbdr in dr)
                                {
                                    id = (int)(long)dbdr[0];
                                }
                                MultyThreadChangeId(id);
                                MultyThreadstoploadind();
                            }
                            else
                            {
                                if (savemode == "update")
                                {
                                    string zaprstr = "";
                                    for (var i = 0; i < outarri; i++)
                                    {
                                        if (zaprstr == "")
                                        {
                                            zaprstr = outarr[0, i] + " = " + "'" + outarr[1, i] + "'";
                                        }
                                        else
                                        {
                                            zaprstr += "," + outarr[0, i] + " = " + "'" + outarr[1, i] + "'";
                                        }
                                    }
                                    for (var i = 0; i < outarri2; i++)
                                    {
                                        if (zaprstr == "")
                                        {
                                            zaprstr = outarr2[0, i] + " = " + "'" + outarr2[1, i] + "'";
                                        }
                                        else
                                        {
                                            zaprstr += "," + outarr2[0, i] + " = " + "'" + outarr2[1, i] + "'";
                                        }
                                    }
                                    zaprstr = "UPDATE cards SET " + zaprstr + "WHERE id =" + id;
                                    SQLiteCommand comm = new SQLiteCommand(zaprstr, conn);
                                    comm.ExecuteNonQuery();
                                    MultyThreadChangeId(id);
                                    MultyThreadstoploadind();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (end == 1)
                            MultyThreadClose();
                    }
                }
            }
        }
        delegate void chid(long id);
        private void MultyThreadChangeId(long id)
        {
            if (textBox1.InvokeRequired && numericUpDown1.InvokeRequired)
            {
                chid ad = new chid(MultyThreadChangeId);
                this.Invoke(ad, new object[] { id });
            }
            else
            {
                textBox1.Text = id.ToString();
                numericUpDown1.Value = id;
            }
        }
        #endregion

        #region DATAGRIDVIEW CREATE
        //zone of datagrig view loading
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------~
        private void dbdataadapterload()
        {
            lg.add("Thread debug: dbdataadapterload started");
            DataGridView[] dar = new DataGridView[] { dataGridView1, dataGridView2, dataGridView3 };
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM card_in_smi;", conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            DataGridViewColumn dgvc = new DataGridViewColumn();
            for (var i = 0; i < dar.Length; i++)
            {
                if (dar[i].ColumnCount != 7)
                {
                    MultyThreadChangeds(dar[i], null, 2);
                    dgvc = CreateColumn("Тип СМИ", "type", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("Наименование", "name", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("Дата", "date", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("Количество повторов", "times", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("Файл материала", "link", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("При участии", "pu", Type.GetType("System.String"), true);
                    MultyThreadChangeds(dar[i], dgvc, 1);
                    dgvc = CreateColumn("ID СМИ", "smi_id", Type.GetType("System.String"), true);
                    dgvc.Visible = false;
                    MultyThreadChangeds(dar[i], dgvc, 1);
                }
            }
            if (needload == false)
                MultyThreadstoploadind();
        }
        private string isnull(DataGridViewColumn dgvc)
        {
            if (dgvc != null)
                return dgvc.Name;
            else
                return "null";
        }
        delegate void cadds(DataGridView at, DataGridViewColumn dvgc, int ind);
        /// <summary>
        /// Многопоточно безопасный доступ к датагридвью
        /// </summary>
        /// <param name="at">Целевая Datagridview</param>
        /// <param name="dgvc">Добавляемая колонка</param>
        /// <param name="ind">Индекс операции</param>
        private void MultyThreadChangeds(DataGridView at, DataGridViewColumn dgvc, int ind)
        {
            lg.add("Thread debug: MultyTreadChangeds started." + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
            if (at.InvokeRequired)
            {
                lg.add("Tread debug: Invoke requared. Invoking" + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
                cadds ad = new cadds(MultyThreadChangeds);
                this.Invoke(ad, new object[] { at, dgvc, ind });
            }
            else
            {
                lg.add("Tread debug: Invoke not requared. Continuing..." + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
                if (ind == 2)
                {
                    if (at.ColumnCount != 0)
                    {
                        at.Columns.Clear();
                        lg.add(Environment.NewLine + Environment.NewLine + "Thread debug: Columns cleared" + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
                    }
                }
                if (ind == 0)
                {
                    if (at.Rows.Count != 0)
                    {
                        at.Rows.Clear();
                        //lg.add(Environment.NewLine+ Environment.NewLine+"Thread debug: Rows cleared" + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
                    }
                }
                if (ind == 1)
                {
                    at.Columns.Add(dgvc);
                    lg.add(Environment.NewLine + Environment.NewLine + "Tread debug: column " + dgvc.Name + " added" + Environment.NewLine + " Params: ind = " + ind.ToString() + " column name = " + isnull(dgvc) + " datagridview name = " + at.Name + Environment.NewLine);
                }
            }
        }
        /// <summary>
        /// Создает столбец в заданной таблице с заданными параметрами
        /// </summary>
        /// <param name="dt">Таблица, в котрой требуется создать столбец</param>
        /// <param name="ColumnCaption">Отображаемый заголовок столбца</param>
        /// <param name="Name">Системное имя столбца</param>
        /// <param name="TypeOfColumn">Тип данных столбца</param>
        /// <param name="ReadOnly">Параметр ТОЛЬКО ДЛЯ ЧТЕНИЯ столбца</param>
        /// <returns></returns>
        private DataGridViewColumn CreateColumn(string ColumnCaption, string Name, Type TypeOfColumn, bool ReadOnly)
        {
            DataGridViewColumn column = new DataGridViewColumn();
            column.ValueType = TypeOfColumn;
            column.Name = Name;
            column.HeaderText = ColumnCaption;
            column.ReadOnly = ReadOnly;
            column.CellTemplate = new DataGridViewTextBoxCell();
            return column;
        }
        #endregion

        #region DATAGRIDVIEW LOAD
        private void loadInDataGridView()
        {
            //lg.add("Tread debug: loadInDataGridView started");
            DataGridView[] dgv = new DataGridView[] { dataGridView1, dataGridView2, dataGridView3 };
            MultyThreadChangeds(dgv[0], null, 0);
            MultyThreadChangeds(dgv[1], null, 0);
            MultyThreadChangeds(dgv[2], null, 0);
            mtupd(0);
            mtupd(1);
            mtupd(2);
            SQLiteCommand comm = new SQLiteCommand("SELECT * FROM card_in_smi WHERE cards_num = " + id, conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                comm = new SQLiteCommand("SELECT * FROM smi WHERE id = " + ddr["smi"], conn);
                SQLiteDataReader dr2 = comm.ExecuteReader();
                foreach (DbDataRecord ddr2 in dr2)
                {
                    int where = -1;
                    switch (ddr2["place"].ToString())
                    {
                        case "centr":
                            where = 0;
                            break;
                        case "reg":
                            where = 1;
                            break;
                        case "another":
                            where = 2;
                            break;
                    }
                    if (where != -1)
                    {
                        mtaddr(dgv[where], ddr2["type"].ToString(), ddr2["name"].ToString(), ddr["date"].ToString(), ddr["times"].ToString(), ddr["link"].ToString(), ddr["pu"].ToString() == "" ? "-" : "+", ddr["id"].ToString());
                        mtstat(where, ddr ,ddr2);
                    }
                }
            }
            MultyThreadstoploadind();
        }
        delegate void addr(DataGridView tg, params string[] args);
        private void mtaddr(DataGridView tg, params string[] args)
        {
            if (tg.InvokeRequired || tg.ColumnCount != 7)
            {
                addr dr = new addr(mtaddr);
                this.Invoke(dr, new object[] { tg, args });
            }
            else
            {
                int zaebalsya_uze = tg.Rows.Add();
                //lg.add("Tread debug: adding row."+Environment.NewLine+" Sys info: row_lenght:"+tg.Rows[zaebalsya_uze].Cells.Count+"; p1 = "+p1+ " p2 = " + p2 + " p3 = " + p3 + " p4 = " + p4 + " p5 = " + p5 + " p6 " + p6 + ";");
                for (int i = 0; i < 7; i++)
                {
                    tg.Rows[zaebalsya_uze].Cells[i].Value = args[i];
                }
                tg.Rows[zaebalsya_uze].ContextMenuStrip = contextMenuStrip2;
            }
        }
        delegate void stat(int TabLayoutPanelIndex, DbDataRecord ddr, DbDataRecord ddr2);
        private void mtstat(int TablayoutPanelIndex,DbDataRecord ddr, DbDataRecord ddr2)
        {
            UserControls.smiStats[] ss = new UserControls.smiStats[] { smiStats1, smiStats2, smiStats3 };
            if(ss[TablayoutPanelIndex].InvokeRequired)
            {
                this.Invoke(new stat(mtstat),new object[] { TablayoutPanelIndex,ddr,ddr2 });
            }
            else
            {
                switch (ddr2["type"].ToString())
                {
                    case "internet":
                        ss[TablayoutPanelIndex].addResultCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        break;
                    case "tv":
                        ss[TablayoutPanelIndex].addTvCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        ss[TablayoutPanelIndex].addResultCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        break;
                    case "rad":
                        ss[TablayoutPanelIndex].addRadCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        ss[TablayoutPanelIndex].addResultCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        break;
                    case "gaz":
                        ss[TablayoutPanelIndex].addPrtCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        ss[TablayoutPanelIndex].addResultCount((int)(long)ddr["times"] == 0 ? 1 : (int)(long)ddr["times"]);
                        break;
                }
            }
        }
        delegate void upd(int ind);
        private void mtupd(int ind)
        {
            UserControls.smiStats[] ss = new UserControls.smiStats[] { smiStats1, smiStats2, smiStats3 };
            if (ss[ind].InvokeRequired)
            {
                this.Invoke(new upd(mtupd), new object[] { ind });
            }
            else
            {
                ss[ind].setPrtCount(0);
                ss[ind].setRadCount(0);
                ss[ind].setTvCount(0);
                ss[ind].setResultCount(0);
            }
        }
        #endregion

        #region DATAGRIDVIEW EVENTS
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        int rowIndex = 0;
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
        }
        #endregion

        #region CONTEXT MENU STRIP
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (id == -1)
            {
                savemode = "save";
                cardsave();
            }
            else
            {
                if (e.ClickedItem == CM1add || e.ClickedItem == CM2add)
                {
                    Forms.smi_inp sminp = new Forms.smi_inp(id);
                    sminp.ShowDialog();
                    pictureBox1.Visible = true;
                    new Thread(loadInDataGridView).Start();
                }
                if (e.ClickedItem == CM2RED)
                {
                    DataGridView[] dar = new DataGridView[] { dataGridView1, dataGridView2, dataGridView3 };
                    var cisid = dar[tabControl1.SelectedIndex].Rows[rowIndex].Cells[dar[tabControl1.SelectedIndex].Rows[rowIndex].Cells.Count - 1].Value.ToString();
                    Forms.smi_inp sminp = new Forms.smi_inp(id, cisid);
                    sminp.ShowDialog();
                    pictureBox1.Visible = true;
                    new Thread(loadInDataGridView).Start();
                }
            }
        }
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            if (id == -1)
                CM1add.Text = "сохранить карточку";
            else
                CM1add.Text = "добавить";
        }

        private void CM2Delthis_Click(object sender, EventArgs e)
        {
            DataGridView[] dar = new DataGridView[] { dataGridView1, dataGridView2, dataGridView3 };
            SQLiteCommand comm = new SQLiteCommand("DELETE FROM card_in_smi WHERE id = " + dar[tabControl1.SelectedIndex].Rows[rowIndex].Cells[dar[tabControl1.SelectedIndex].Rows[rowIndex].Cells.Count - 1].Value.ToString(), conn);
            comm.ExecuteNonQuery();
            pictureBox1.Visible = true;
            new Thread(loadInDataGridView).Start();

        }

        private void CM2DelALL_Click(object sender, EventArgs e)
        {
            DataGridView[] dar = new DataGridView[] { dataGridView1, dataGridView2, dataGridView3 };
            for (var i = 0; i < dar[tabControl1.SelectedIndex].Rows.Count; i++)
            {
                SQLiteCommand comm = new SQLiteCommand("DELETE FROM card_in_smi WHERE id = " + dar[tabControl1.SelectedIndex].Rows[i].Cells[dar[tabControl1.SelectedIndex].Rows[rowIndex].Cells.Count - 1].Value.ToString(), conn);
                comm.ExecuteNonQuery();
            }
            pictureBox1.Visible = true;
            new Thread(loadInDataGridView).Start();
        }
        #endregion
        //end of work zone
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region Navigator panel region
        private void changeid(long newid, int goway)
        {
            lg.add("changeId started");
            id = newid;
            numericUpDown1.Value = id;
            goWay = goway;
            needload = true;
            lg.add("changeid->loadstart");
            loadstart();
        }
        private void bNP_Click(object sender, EventArgs e)
        {
            lg.add("bNP_Click started -> changeid");
            Button ttb = (Button)sender;
            switch (ttb.Name)
            {
                case "bPrevius":
                    if (id > minId)
                    {
                        changeid(id - 1, -1);
                    }
                    break;
                case "bNext":
                    if (id < maxId)
                    {
                        changeid(id + 1, 1);
                    }
                    break;
                case "bFirst":
                    changeid(minId, 0);
                    break;
                case "bLast":
                    changeid(maxId, 0);
                    break;
            }
        }

        private void getMnMId()
        {
            SQLiteCommand comm = new SQLiteCommand("SELECT MAX(id) FROM cards;", conn);
            SQLiteDataReader dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                maxId = (int)(long)ddr[0];
            }
            comm = new SQLiteCommand("SELECT MIN(id) FROM cards;", conn);
            dr = comm.ExecuteReader();
            foreach (DbDataRecord ddr in dr)
            {
                minId = (int)(long)ddr[0];
            }
        }

        #region CHANGE ID BY TEXTBOX
        private bool ttchanged = false;
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ttchanged = true;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            lg.add("textBox3_KeyDown -> changeid");
            if (e.KeyCode == Keys.Enter && ttchanged)
            {
                changeid((int)(sender as NumericUpDown).Value, 2);
                ttchanged = false;
            }
            if ((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) || (e.KeyCode >= Keys.D0 && e.KeyCode < Keys.D9) || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                ttchanged = true;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            lg.add("textBox3_Leave -> changeid");
            if (ttchanged)
            {
                changeid((int)(sender as NumericUpDown).Value, 2);
                ttchanged = false;
            }
        }

        private void numericUpDown1_MouseClick(object sender, MouseEventArgs e)
        {
            if (ttchanged)
            {
                changeid((int)(sender as NumericUpDown).Value, 2);
                ttchanged = false;
            }
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void miBut1_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("Удалить?", "Удаление карточки", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
            {
                SQLiteCommand comm = new SQLiteCommand("DELETE FROM cards WHERE id = " + id + ";", conn);
                comm.ExecuteNonQuery();
                changeid(1, 1);
            }
        }

        #endregion

        #endregion

        #region CLOSE FORM EVENTS
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
        //close zone
        private void RedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }
        #endregion

    }
    #region COMBOVALUE class
    public class combovalue
    {
        public string value { get; set; }
        public int number { get; set; }
        public override string ToString()
        {
            return this.value;
        }
        public override bool Equals(object obj)
        {
            if (obj.ToString() == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
}
