using pharmacy.ClassContainer;
using pharmacy.Properties;
using System;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_User : Form
    {
        Connection conn = new Connection();
        private string PrimaryID = "0";
        private string Primarykey = "user_id";
        private string TblName = "tbl_user";
        private string SelectedColumns = "user_id,fullname,username,pass";

        public frm_User()
        {
            InitializeComponent();
        }

        private void frm_User_Load(object sender, EventArgs e)
        {
            RefreshDVG();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckEmptyText())
            {
                return;
            }
            if (PrimaryID == "0")
            {
                conn.InsertData(TblName, $"N'{txtFullName.Text.Trim()}',N'{txtUsername.Text.Trim()}',N'{txtpassword.Text.Trim()}',0", false, false);
            }
            else
            {
                conn.UpdateData(TblName, $"fullname=N'{txtFullName.Text.Trim()}',username=N'{txtUsername.Text.Trim()}',pass=N'{txtpassword.Text.Trim()}'", $"{Primarykey}={PrimaryID}", false);

            }
            btnclean.PerformClick();

        }
        private void btnclean_Click(object sender, EventArgs e)
        {
            txtUsername.Text=txtFullName.Text=txtpassword.Text=txtSearch.Text=string.Empty;
            PrimaryID = "0";
            RefreshDVG();
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                //agar wti naxer naiata xwartr ,agar wti bale aisretawa
                if (MessageBox.Show("are you sure for acting this action?", "delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                conn.DeleteData(TblName,$"{Primarykey}  = { dgv.CurrentRow.Cells[Primarykey].Value.ToString()}");
                btnclean.PerformClick();
            }
            else if(dgv.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                GetDataToText();
            }
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} WHERE (fullname LIKE N'%{txtSearch.Text.Trim()}%' OR username LIKE N'%{txtSearch.Text.Trim()}%' OR pass LIKE N'%{txtSearch.Text.Trim()}%' ) AND archived=0 AND {Primarykey}<>1");  //wata userid 1 nabe chwnka yak awaia ka daghlabe
            dgv.ClearSelection();

            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnEdit", Image = Resources.icons8_edit_row_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnDelete", Image = Resources.icons8_delete_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });

            dgv.Columns["user_id"].HeaderText = "id";
            dgv.Columns["fullname"].HeaderText = "fullname";
            dgv.Columns["username"].HeaderText = "username";
            dgv.Columns["pass"].HeaderText = "password";

            dgv.Columns["btnEdit"].Width = 40;
            dgv.Columns["btnDelete"].Width = 40;


        }


        private void RefreshDVG()
        {
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} WHERE archived=0 AND {Primarykey}<>1");
            dgv.ClearSelection();

            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnEdit", HeaderText="", Image=Resources.icons8_edit_row_24px,AutoSizeMode=DataGridViewAutoSizeColumnMode.None });
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnDelete", HeaderText = "", Image = Resources.icons8_delete_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });

            dgv.Columns["user_id"].HeaderText = "id";
            dgv.Columns["fullname"].HeaderText = "fullname";
            dgv.Columns["username"].HeaderText = "username";
            dgv.Columns["pass"].HeaderText = "password";

            dgv.Columns["btnEdit"].Width = 40;
            dgv.Columns["btnDelete"].Width = 40;

        }
        private void GetDataToText()
        {
            PrimaryID = dgv.CurrentRow.Cells[Primarykey].Value.ToString();
            txtFullName.Text = dgv.CurrentRow.Cells["fullname"].Value.ToString();
            txtUsername.Text = dgv.CurrentRow.Cells["username"].Value.ToString();
            txtpassword.Text = dgv.CurrentRow.Cells["pass"].Value.ToString();

        }
        private bool CheckEmptyText()
        {
            bool Check = false;
            //wata agar text'aka batal bw
            if (string.IsNullOrWhiteSpace(txtFullName.Text.Trim()))
            {
                MessageBox.Show("please enter a fullname","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Check=true;
            }
            else if (string.IsNullOrWhiteSpace(txtUsername.Text.Trim()))
            {
                MessageBox.Show("please enter a username", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            else if (string.IsNullOrWhiteSpace(txtpassword.Text.Trim()))
            {
                MessageBox.Show("please enter a password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }

            return Check;
        }


    }
}
