using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrayLark.bin.Debug.Report;
using System.Data.SqlClient;

namespace GrayLark
{
    public partial class frmChartofAccount : Form
    {
        ActionModes actMode = ActionModes.Browsing;
        bool isChild = false;
        public enum ActionModes
        {
            Add,
            Edit,
            Browsing
        }
        String UID = Login.UserID;
        public string GreenSignal = "";
        public string FormID = "";
        public frmChartofAccount()
        {
            InitializeComponent();
            loadingFields(); this.KeyPreview = true;
        }

        public void UserNametesting()
        {
            string query = "  select * from user_access where USENAME='" + UID + "' AND FORM_NAME='" + FormID + "'";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);

            con.Open();
            SqlCommand sc = new SqlCommand(query, con);
            SqlDataReader dr = sc.ExecuteReader();
            dr.Read();

            if (dr.HasRows == true)
            {
                if (dr["ACCESS"].ToString() == "Yes")
                {
                    GreenSignal = "YES";
                }
                else
                {
                    GreenSignal = "NO";

                }
            }
        }

        private void Browsing()
        {
            txt_HeaderAct_code.Enabled = false;
            txt_HeaderActTitle.Enabled = false;
            txt_AccountTitle.Enabled = false;
            txt_Level.Enabled = false;
            txt_ShortTitle.Enabled = false;
            comboBox_Category.Enabled = false;
            comboBox_AccountType.Enabled = false;
            comboBox_HeaderAccount.Enabled = false;

            btn_Add.Enabled = true;
            btn_Edit.Enabled = true;
            button_Delete.Enabled = true;
            btn_Save.Enabled = false;
            btn_Undo.Enabled = false;

            PopulateTreeView();
            setCombo();
            treeView_Master.Enabled = true;
        }

        private void Add()
        {
            txt_HeaderAct_code.Text = "";
            txt_HeaderActTitle.Text = "";
            txt_AccountTitle.Text = "";
            txt_Level.Text = "";
            txt_ShortTitle.Text = "";
            txt_ActCode.Text = "";
           // comboBox_Category.SelectedIndex = 0;
           // comboBox_HeaderAccount.SelectedIndex = 0;

            txt_HeaderAct_code.Enabled = false;
            txt_HeaderActTitle.Enabled = false;
            txt_AccountTitle.Enabled = true;
            txt_Level.Enabled = false;
            txt_ShortTitle.Enabled = true;
            comboBox_Category.Enabled = true;
            comboBox_AccountType.Enabled = true;
            comboBox_HeaderAccount.Enabled = true;

            radioButton_Header.Checked = true;
            checkBox_Status.Checked = true;

            btn_Add.Enabled = false;
            btn_Edit.Enabled = false;
            button_Delete.Enabled = false;
            btn_Save.Enabled = true;
            btn_Undo.Enabled = true;
            treeView_Master.Enabled = false;
        }

        private void Edit()
        {
            txt_HeaderAct_code.Enabled = false;
            txt_HeaderActTitle.Enabled = false;
            comboBox_HeaderAccount.Enabled = false;
            txt_AccountTitle.Enabled = true;
            txt_Level.Enabled = false;
            txt_ShortTitle.Enabled = true;
            comboBox_Category.Enabled = true;
            comboBox_AccountType.Enabled = true;

            btn_Add.Enabled = false;
            btn_Edit.Enabled = false;
            button_Delete.Enabled = false;
            btn_Save.Enabled = true;
            btn_Undo.Enabled = true;
            treeView_Master.Enabled = false;
        }
        private void SeletionModes(ActionModes modes)
        {
            if (modes == ActionModes.Browsing)
            {
                Browsing();
            }
            else if (modes == ActionModes.Add)
            {
                Add();
            }
            else if (modes == ActionModes.Edit)
            {
                Edit();
            }
        }
        private void setCombo()
        {
            try
            {
                String q2 = "select * from Accounts where ID < 15";
                string Query = @"Select '00' ID, 'Main Account' Acttitle , 'True' Status
            Union ALL  Select ID, ActTitle,Status from Accounts Where Status='True' And ActHierarchy='H' And ActLevel<>4";
                comboBox_HeaderAccount.DataSource = clsDataLayer.RetreiveQuery(q2);
                comboBox_HeaderAccount.DisplayMember = "ActTitle";
                comboBox_HeaderAccount.ValueMember = "ID";

                string Query1 = " Select ID, ActTitle from Accounts Where HeaderActCode='00' and ID < 15";
                comboBox_AccountType.DataSource = clsDataLayer.RetreiveQuery(Query1);
                comboBox_AccountType.DisplayMember = "ActTitle";
                comboBox_AccountType.ValueMember = "ID";
            }
            catch { }
        }
//        private void setCombo()
//        {
//            try
//            {
//                string Query = @"Select '00' ID, 'Main Account' Acttitle , 'True' Status
//            Union ALL  Select ID, ActTitle,Status from Accounts Where Status='True' And ActHierarchy='H' And ActLevel<>4";
//                comboBox_HeaderAccount.DataSource = clsDataLayer.RetreiveQuery(Query);
//                comboBox_HeaderAccount.DisplayMember = "ActTitle";
//                comboBox_HeaderAccount.ValueMember = "ID";

//                string Query1 = " Select ID, ActTitle from Accounts Where HeaderActCode='00'";
//                comboBox_AccountType.DataSource = clsDataLayer.RetreiveQuery(Query1);
//                comboBox_AccountType.DisplayMember = "ActTitle";
//                comboBox_AccountType.ValueMember = "ID";
//            }
//            catch { }
//        }
        private void loadingFields()
        {
            SeletionModes(actMode);
        }

        private void OnNew()
        {
            FormID = "Add";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                actMode = ActionModes.Add;
                SeletionModes(actMode);
                comboBox_HeaderAccount.Focus();
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OnNew(); 
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
                 FormID = "Edit";
                UserNametesting();
                if (GreenSignal == "YES")
                {
                    actMode = ActionModes.Edit;
                    SeletionModes(actMode);
                }
                else
                {
                    MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
        }
        private void btn_Undo_Click(object sender, EventArgs e)
        { 
                    actMode = ActionModes.Browsing;
                    SeletionModes(actMode);
                
        }

        private void OnSave()
        {
            FormID = "Save";
            UserNametesting();
            if (GreenSignal == "YES")
            {
                try
                {
                    if (actMode == ActionModes.Add)
                    {
                        String getname = "select * from Accounts where ActTitle='" + txt_AccountTitle.Text + "'";
                        DataTable de = clsDataLayer.RetreiveQuery(getname);
                        if (de.Rows.Count > 0)
                        {
                            MessageBox.Show("UserName Already Added Please Some Change ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string insertQuery = " INSERT INTO Accounts( ActCode, HeaderActCode, ActTitle, ShortTitle, ActLevel, ActHierarchy, " +
                            " ActCategory, ActCount, ActType, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                            " VALUES('" + txt_ActCode.Text + "','" + txt_HeaderAct_code.Text + "','" + txt_AccountTitle.Text + "','" + txt_ShortTitle.Text + "'," + txt_Level.Text +
                            ",'" + getActHireachy() + "','" + getActCategory(comboBox_Category.SelectedIndex) + "',0,'" + comboBox_AccountType.SelectedValue + "','" + checkBox_Status.Checked + "','" + Login.UserID + "',GETDATE(),0,' ')";
                            Console.WriteLine(insertQuery);
                            if (txt_ActCode.Text != "" && txt_AccountTitle.Text != "" && txt_HeaderAct_code.Text != "" && txt_ShortTitle.Text != "")
                            {
                                if (clsDataLayer.ExecuteQuery(insertQuery) > 0)
                                {
                                    String upd = "update Accounts set OpeningBalance='NILL', RemainingLimit=0 where ActCode='" + txt_ActCode.Text + "'"; clsDataLayer.ExecuteQuery(upd);

                                    UpdateCount(txt_HeaderAct_code.Text);
                                    MessageBox.Show("Record Successfully Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    actMode = ActionModes.Browsing;
                                    SeletionModes(actMode);
                                }
                                else
                                {
                                    MessageBox.Show("Action Rollback!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            else
                            {
                                MessageBox.Show("Please Fill All The TextField", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else if (actMode == ActionModes.Edit)
                    {
                        String del = "delete from Accounts where ActCode='" + txt_ActCode.Text + "'"; clsDataLayer.ExecuteQuery(del);
                        string insertQuery = " INSERT INTO Accounts( ActCode, HeaderActCode, ActTitle, ShortTitle, ActLevel, ActHierarchy, " +
                           " ActCategory, ActCount, ActType, Status, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                           " VALUES('" + txt_ActCode.Text + "','" + txt_HeaderAct_code.Text + "','" + txt_AccountTitle.Text + "','" + txt_ShortTitle.Text + "'," + txt_Level.Text +
                           ",'" + getActHireachy() + "','" + getActCategory(comboBox_Category.SelectedIndex) + "',0,'" + comboBox_AccountType.SelectedValue + "','" + checkBox_Status.Checked + "','" + Login.UserID + "',GETDATE(),0,' ')";

                        if (txt_ActCode.Text != "" && txt_AccountTitle.Text != "" && txt_HeaderAct_code.Text != "" && txt_ShortTitle.Text != "")
                        {
                            if (clsDataLayer.ExecuteQuery(insertQuery) > 0)
                            {
                                //UpdateCount(txt_HeaderAct_code.Text);
                                MessageBox.Show("Record Successfully Update", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                actMode = ActionModes.Browsing;
                                SeletionModes(actMode);
                            }
                            else
                            {
                                MessageBox.Show("Action Rollback!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Fill All The TextField", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Contact your Administrator You do not have permission", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            OnSave();    
        }

        private void UpdateCount(string code)
        {
            if (code != "00")
            {
                string query = " Update Accounts Set ActCount=" + label_Count.Text + ",ActHierarchy='H' Where ActCode='" + code + "'";
                clsDataLayer.ExecuteQuery(query);
            }
        }
        private void PopulateTreeView()
        {
            try
            {
                treeView_Master.Nodes.Clear();
                DataTable dtMaster = clsDataLayer.RetreiveQuery("SELECT   ActCode, HeaderActCode, ActTitle, ActLevel, Status FROM  Accounts Where ActLevel=1");
                DataTable dtDetails = clsDataLayer.RetreiveQuery("SELECT   ActCode, HeaderActCode, ActTitle, ActLevel, Status FROM  Accounts Where ActLevel!=1");
                for (int i = 0; i < dtMaster.Rows.Count; i++)
                {
                    TreeNode root = new TreeNode();
                    root.Name = dtMaster.Rows[i]["ActCode"].ToString();
                    root.Text = dtMaster.Rows[i]["ActCode"].ToString() + " | " + dtMaster.Rows[i]["ActTitle"].ToString();
                    ChildNodes(root, dtDetails);
                    treeView_Master.Nodes.Add(root);
                }
                treeView_Master.CollapseAll();
            }
            catch { }
        }
        private string AccountCodeGenerator()
        {
            string str = "";
            if (txt_HeaderAct_code.Text != "00")
            {
                str = txt_HeaderAct_code.Text + int.Parse(label_Count.Text).ToString("D2");
            }
            else
            {
                string query = " Select Max(Convert(bigint,ActCode) +1) as ActCode  from Accounts Where HeaderActCode='00'";
                str = int.Parse(clsDataLayer.RetreiveQuery(query).Rows[0][0].ToString()).ToString("D2");
            }
            return str;
        }
        private void ChildNodes(TreeNode node, DataTable dt)
        {
            string strFields = "HeaderActCode='" + node.Name + "'";
            DataRow[] rows = dt.Select(strFields);
            if (rows.Length == 0)
            {
                return;
            }
            for (int i = 0; i < rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode();
                Childnode.Name = rows[i]["ActCode"].ToString();
                Childnode.Text = rows[i]["ActCode"].ToString() + " | " + rows[i]["ActTitle"].ToString();
                node.Nodes.Add(Childnode);
                ChildNodes(Childnode, dt);
            }
        }
        private string getActHireachy()
        {
            string str = "";
            if (radioButton_Header.Checked)
            {
                str = "H";
            }
            else if (radioButton_Detail.Checked)
            {
                str = "D";
            }
            return str;
        }
        private string getActCategory(int catIndex)
        {
            string str = "";

            switch (catIndex)
            {
                case 0:
                    str = "C";
                    break;
                case 1:
                    str = "P";
                    break;
                case 2:
                    str = "S";
                    break;
                case 3:
                    str = "O";
                    break;
            }
            return str;
        }
        private int getActType(string str)
        {
            int no = 0;
            switch (str)
            {
                case "A":
                    no = 0;
                    break;
                case "L":
                    no = 1;
                    break;
                case "C":
                    no = 2;
                    break;
                case "E":
                    no = 3;
                    break;
                case "R":
                    no = 4;
                    break;
            }
            return no;
        }
        private void getActHireachy(string str)
        {
            if (str == "H")
            {
                radioButton_Header.Checked = true;
            }
            else
            {
                radioButton_Detail.Checked = true;
            }

        }
        private int getActCategory(string str)
        {
            int no = 0;
            switch (str)
            {
                case "C":
                    no = 0;
                    break;
                case "P":
                    no = 1;
                    break;
                case "S":
                    no = 2;
                    break;
                case "O":
                    no = 3;
                    break;
            }
            return no; ;
        }
        private string getActType(int TypeIndex)
        {
            string str = "";

            switch (TypeIndex)
            {
                case 0:
                    str = "A";
                    break;
                case 1:
                    str = "L";
                    break;
                case 2:
                    str = "C";
                    break;
                case 3:
                    str = "E";
                    break;
            }
            return str;
        }
        private void treeView_Master_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string Query = "SELECT  ID, ActCode, ActTitle,  ShortTitle, ActLevel,  HeaderActCode,  ActHierarchy, ActCategory,  ActType, " +
                 " ActCount,  Status FROM  Accounts   Where  ActCode='" + e.Node.Name + "'";
                DataTable dt = clsDataLayer.RetreiveQuery(Query);
                SelectParentNode(dt);
            }
            catch { }
        }
        private void SelectParentNode(DataTable dt)
        {
            try
            {
                txt_ActCode.Text = dt.Rows[0]["ActCode"].ToString();
                txt_AccountTitle.Text = dt.Rows[0]["ActTitle"].ToString();
                txt_HeaderAct_code.Text = dt.Rows[0]["HeaderActCode"].ToString();
                txt_HeaderActTitle.Text = GetHeaderAccountCode(dt.Rows[0]["HeaderActCode"].ToString());
                txt_ShortTitle.Text = dt.Rows[0]["ShortTitle"].ToString();
                txt_Level.Text = dt.Rows[0]["ActLevel"].ToString();
                getActHireachy(dt.Rows[0]["ActHierarchy"].ToString());
                comboBox_AccountType.SelectedIndex = getActType(dt.Rows[0]["ActType"].ToString());
                comboBox_Category.SelectedIndex = getActCategory(dt.Rows[0]["ActCategory"].ToString());
                label_Count.Text = dt.Rows[0]["ActCount"].ToString();
                comboBox_HeaderAccount.SelectedValue = int.Parse(dt.Rows[0]["ID"].ToString());
                label_ID.Text = dt.Rows[0]["ID"].ToString();
            }
            catch { }
        }
        private string GetHeaderAccountCode(string code)
        {
            string str = "";
            if (code == "00")
            {
                str = "Main Account";
            }
            else
            {
                str = clsDataLayer.RetreiveQuery("Select ActTitle from Accounts Where ActCode='" + code + "'").Rows[0][0].ToString();
            }
            return str;
        }
        private void comboBox_HeaderAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (actMode == ActionModes.Add)
                {
                    string Query = "SELECT  ID, ActCode, ActTitle,  ShortTitle, ActLevel,  HeaderActCode,  ActHierarchy, ActCategory,  ActType, " +
                    " ActCount+1 ActCount,  Status FROM  Accounts" +
                    " Where Id=" + comboBox_HeaderAccount.SelectedValue;
                    DataTable dt = clsDataLayer.RetreiveQuery(Query);
                    if (dt.Rows.Count > 0)
                    {
                        txt_HeaderAct_code.Text = dt.Rows[0]["ActCode"].ToString();
                        txt_HeaderActTitle.Text = dt.Rows[0]["ActTitle"].ToString();

                        comboBox_AccountType.Text = txt_HeaderActTitle.Text;
                        txt_Level.Text = (int.Parse(dt.Rows[0]["ActLevel"].ToString()) + 1).ToString();
                        label_Count.Text = dt.Rows[0]["ActCount"].ToString();
                        txt_ActCode.Text = AccountCodeGenerator();
                        getActHireachy(dt.Rows[0]["ActHierarchy"].ToString());
                        comboBox_AccountType.SelectedValue = int.Parse(dt.Rows[0]["ActType"].ToString());
                        comboBox_Category.SelectedIndex = getActCategory(dt.Rows[0]["ActCategory"].ToString());

                        if ((int.Parse(dt.Rows[0]["ActLevel"].ToString()) + 1) == 4)
                        {
                            radioButton_Detail.Checked = true;
                        }
                    }
                    else
                    {
                        txt_HeaderAct_code.Text = "00";
                        txt_HeaderActTitle.Text = "Main Account";
                        txt_Level.Text = "1";
                        label_Count.Text = "0";
                        txt_ActCode.Text = AccountCodeGenerator();
                    }
                }
            }
            catch { }
        }

        private void clear()
        {
            txt_HeaderAct_code.Text = "";
            txt_HeaderActTitle.Text = "";
            txt_AccountTitle.Text = "";
            txt_Level.Text = "";
            txt_ShortTitle.Text = "";
            comboBox_Category.SelectedIndex = 0;
            //comboBox_AccountType.SelectedIndex = 0;
            comboBox_HeaderAccount.SelectedIndex = 0;
        }
        private void button_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txt_ActCode.Text.Equals(""))
                {
                    if (txt_ActCode.Text.Length < 2)
                    {
                        MessageBox.Show("Header Account Cant Delete!");
                    }
                    else
                    {
                    String del = "delete from Accounts where ActCode = '" + txt_ActCode.Text + "'"; clsDataLayer.ExecuteQuery(del);
                    MessageBox.Show("Successfully Remove the Account", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopulateTreeView(); clear();
                    }
                }
                else
                {
                    MessageBox.Show("Select Account First!");
                }
            }
            catch { }
        }

        private void frmChartofAccount_Load(object sender, EventArgs e)
        {

        }

        private void txt_AccountTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);

        }

        private void txt_ShortTitle_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txt_ActCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            } 
        }

        private void frmChartofAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.A)
            {
                btn_Add.PerformClick();
            }else if (e.Control == true && e.KeyCode == Keys.S)
            {
                btn_Save.PerformClick();
            }
            else if (e.Control == true && e.KeyCode == Keys.E)
            {
                btn_Edit.PerformClick();
            }
             
        }

       
    }
}