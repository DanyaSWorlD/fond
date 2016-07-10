using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common
{
    public class lastlog
    {
        public delegate void ConsoleUpdatedEventHandler(object sender, ConsoleUpdatedArgs e);
        #region params
        public static string Log { get; set; }
        public static bool Nu { get; set; }
        public lastlog()
        {
            Log = Log;
            onCreate();
        }
        #endregion
        /// <summary>
        /// добавлено значение
        /// </summary>
        public event ConsoleUpdatedEventHandler WasUpdated;
        /// <summary>
        /// Добавляет значение переменной  log к всем логам текущего сеанса и дает ему текущее время
        /// </summary>
        /// <param name="log"></param>
        public void add(string log)
        {
            Nu = true;
            if (Log == null)
            {
                Log = log;
            } else
            {
                Log = Log.Insert(0, DateTime.Now.ToLongTimeString().ToString() + " " + log + "\r\n");
            }
        }
        /// <summary>
        /// Срабатывает при обновлении
        /// </summary>
        protected void OnUpdate()
        {
            if (WasUpdated != null)
                WasUpdated(this, new ConsoleUpdatedArgs(Log));
        }
        protected void onCreate()
        {
        }
    }
    public class ConsoleUpdatedArgs
    {
        protected string _value;
        public string value { get { return _value; }}
        public ConsoleUpdatedArgs(string Value)
        {
            _value = value;
        }

    }
}
