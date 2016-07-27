    namespace Common
{
    public static class SqliteCommon
    {
        public static string realEscapeString(this string param)
        {
            return param.Replace("'", "''");
        }
    }
    public static class HTMLcommon
    {
        public static string shield(this string param)
        {             
            param = param.Replace("<", "&lt;");
            param = param.Replace(">", "&gt;");
            param = param.Replace("\"", "&quot;");
            param = param.Replace("'", "&#39;");
            param = param.Replace("&", "&amp;");
            return param;
        }
    }
    public static class stringExtentions
    {
        public static string cutLastSymbol(this string s)
        {
            return s.Substring(0, s.Length - 1);
        }
    }

}
