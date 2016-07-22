    namespace Common
{
    public class SqliteCommon
    {
        public static string realEscapeString(string param)
        {
            return param.Replace("'", "''");
        }
    }
    public class HTMLcommon
    {
        public static string shield(string param)
        {             
            param = param.Replace("<", "&lt;");
            param = param.Replace(">", "&gt;");
            param = param.Replace("\"", "&quot;");
            param = param.Replace("'", "&#39;");
            param = param.Replace("&", "&amp;");
            return param;
        }
    }
}
