using pharmacy.ClassContainer;
using pharmacy.Properties;
using System;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_Expense : Form
    {
        Connection conn = new Connection();
        private string PrimaryID = "0";
        private string Primarykey = "exp_id";
        private string TblName = "tbl_expense";
        private string SelectedColumns = "exp_id,exp_type,exp_date,exp_price,exp_note";

        public frm_Expense()
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
                //la date'da trim nanwsin kesha nia 
                conn.InsertData(TblName, $"N'{txtExpType.Text.Trim()}','{txtExpDate.Text}',{txtExpPrice.Text},N'{txtExpNote.Text.Trim()}',0");//boia false'aka laabain chwnka by default xoi true'a bo awai true bgaretawa(labar e_by u_by)
            }
            else
            {
                conn.UpdateData(TblName, $"exp_type=N'{txtExpType.Text.Trim()}',exp_date=N'{txtExpDate.Text}',exp_price={txtExpPrice.Text.Trim()},exp_note=N'{txtExpNote.Text}'", $"{Primarykey}={PrimaryID}");

            }
            btnclean.PerformClick();

        }
        private void btnclean_Click(object sender, EventArgs e)
        {
            txtExpType.Text=txtExpNote.Text=txtExpNote.Text=txtSearch.Text=string.Empty;
            txtExpPrice.Text= PrimaryID = "0";
            txtExpDate.Text = DateTime.Now.ToString("d");
            RefreshDVG();
            RefreshCMD();
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
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} WHERE (exp_type LIKE N'%{txtSearch.Text.Trim()}%' OR exp_note LIKE N'%{txtSearch.Text.Trim()}%'  ) AND archived=0");
            dgv.ClearSelection();

            ResizeDGVHeader();



        }


        private void RefreshDVG()
        {
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} WHERE archived=0");
            dgv.ClearSelection();

            ResizeDGVHeader();
        }
        private void ResizeDGVHeader()
        {
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnEdit", Image = Resources.icons8_edit_row_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnDelete", Image = Resources.icons8_delete_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });

            dgv.Columns["exp_id"].HeaderText = "id";
            dgv.Columns["exp_type"].HeaderText = "Expense Type";
            dgv.Columns["exp_date"].HeaderText = "Date";
            dgv.Columns["exp_price"].HeaderText = "Expense Price";
            dgv.Columns["exp_note"].HeaderText = "Note";

            dgv.Columns["btnEdit"].Width = 40;
            dgv.Columns["btnDelete"].Width = 40;


        }
        private void RefreshCMD()
        {
            //combobox'aka data akata nawi
            txtExpType.DataSource = conn.GetData($"SELECT DISTINCT exp_type FROM {TblName} WHERE archived=0");//aw shtai ka pshani user'akai aiam britia lamai xwarawa
            txtExpType.DisplayMember= "exp_type";
            txtExpType.Text= string.Empty;
        }
        private void GetDataToText()
        {
            PrimaryID = dgv.CurrentRow.Cells[Primarykey].Value.ToString();
            txtExpType.Text = dgv.CurrentRow.Cells["exp_type"].Value.ToString();
            txtExpDate.Text = dgv.CurrentRow.Cells["exp_date"].Value.ToString();
            txtExpPrice.Text = dgv.CurrentRow.Cells["exp_price"].Value.ToString();
            txtExpNote.Text = dgv.CurrentRow.Cells["exp_note"].Value.ToString();

        }
        private bool CheckEmptyText()
        {
            bool Check = false;
            //wata agar text'aka batal bw
            if (string.IsNullOrWhiteSpace(txtExpType.Text.Trim()))
            {
                MessageBox.Show("please enter a Type of Expense", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            else if (string.IsNullOrWhiteSpace(txtExpPrice.Text.Trim()) || txtExpPrice.Text == "0")
            {
                MessageBox.Show("please enter a Price of Expense", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            return Check;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //bo awai natwane la zhmara ziatr hichi tr daghl bkat
        private void txtExpPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //amanawe bejga la control button'akan w bejga la digit wata zhmarakan hichi tr naka
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }
    }
}
