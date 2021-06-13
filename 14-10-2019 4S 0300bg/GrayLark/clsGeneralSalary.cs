using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reddot_Express_Inventory
{
    public static class clsGeneralSalary
    {
        public static string getMAXCode(string tableName, string columnName, string format)
        {
            string strFormat = "";

            DataTable dt = clsDataLayer2.RetreiveQuery(" SELECT ISNULL( MAX(" + columnName + "),0) AS MAXID FROM " + tableName);

            if (dt.Rows.Count > 0)
            {
                String ans = dt.Rows[0][0].ToString();
                String[] ab = ans.Split('-');
                String abc = "";
                if (ab.Length == 1)
                {
                    abc = ab[0];
                }
                else
                {
                    abc = ab[1];
                }
                strFormat = format + "-" + (int.Parse(abc) + 1).ToString("D5");
            }
            else
            {
                strFormat = format + "-" + (int.Parse(dt.Rows[0][0].ToString()) + 1).ToString("D5");
            }
            return strFormat;
        }

        public static void ComboPop(ComboBox cb, string tableName, string DisplayMember, string ValueMember)
        {
            DataTable dt = clsDataLayer2.RetreiveQuery(" SELECT " + ValueMember + " , " + DisplayMember + " FROM " + tableName);
            DataTable dtSource = new DataTable();
            foreach (DataColumn c in dt.Columns)
            {
                dtSource.Columns.Add(c.ColumnName);
            }
            //dtSource.Rows.Add(0, "Select");
            int i = 0;
            foreach (DataRow r in dt.Rows)
            {
                dtSource.Rows.Add();
                dtSource.Rows[i][0] = r[0];
                //  dtSource.Rows[i][1] = r[1];
                i++;
            }
            cb.DataSource = dtSource;
            cb.DisplayMember = DisplayMember;
            cb.ValueMember = ValueMember;
            cb.SelectedIndex = 0;
        }

        public static void SetAutoCompleteTextBox(TextBox tb, DataTable dt)
        {
            try
            {
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                foreach (DataRow row in dt.Rows)
                {
                    acsc.Add(row[0].ToString());
                }

                tb.AutoCompleteCustomSource = acsc;
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch { }
        }

        public static void SetAutoCompleteGridTextBox(DataGridTextBox tb, DataTable dt)
        {
            try
            {
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

                foreach (DataRow row in dt.Rows)
                {
                    acsc.Add(row[0].ToString());
                }

                tb.AutoCompleteCustomSource = acsc;
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch { }
        }

        public static void SetAutoCompleteTextBoxs(ComboBox tb, DataTable dt)
        {
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();

            foreach (DataRow row in dt.Rows)
            {
                acsc.Add(row[0].ToString());
            }

            tb.AutoCompleteCustomSource = acsc;
            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

    }
}


