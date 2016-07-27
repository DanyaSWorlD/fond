using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOND.Extra
{
    class parser
    {
        public static string parce(string path, RichTextBoxStreamType streamType)
        {
            RichTextBox rt = new RichTextBox();
            rt.LoadFile(path, streamType);
            string result = "";
            openedTag ot = new openedTag(false, false, false);
            Font fold = new Font(FontFamily.GenericSansSerif,12,FontStyle.Regular);
            for (int i = 0; i < rt.TextLength - 2; i++)
            {
                rt.SelectionStart = i;
                rt.SelectionLength = 1;
                Font f = rt.SelectionFont;
                if (f.Bold != fold.Bold)
                {
                    if (f.Bold)
                    {
                        result += "<strong>";
                        ot.bold = true;
                    }
                    else
                    {
                        result += "</strong>";
                        ot.bold = false;
                    }
                }
                if (f.Italic != fold.Italic)
                {
                    if (f.Italic)
                    {
                        result += "<i>";
                        ot.italic = true;
                    }
                    else
                    {
                        result += "</i>";
                        ot.italic = false;
                    }

                }
                if (f.Underline != fold.Underline)
                {
                    if (f.Underline)
                    {
                        result += "<u>";
                        ot.underline = true;
                    }
                    else
                    {
                        result += "</u>";
                        ot.underline = false;
                    }
                }
                if (rt.SelectedText.ToCharArray()[0] == '\n')
                {
                    result += "<br />";
                }
                else
                {
                    result += Common.HTMLcommon.shield(rt.SelectedText);
                }
                fold = rt.SelectionFont;
            }
            if (ot.bold == true)
            {
                result += "</strong>";
            }
            if (ot.italic == true)
            {
                result += "</i>";
            }
            if (ot.underline == true)
            {
                result += "</u>";
            }

            return result;
        }
        private struct openedTag
        {
            public bool bold;
            public bool italic;
            public bool underline;
            public openedTag(bool _bold, bool _italic, bool _underline)
            {
                bold = _bold;
                italic = _italic;
                underline = _underline;
            }
        }
    }
}
