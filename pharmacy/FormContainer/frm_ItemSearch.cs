using pharmacy.ClassContainer;
using pharmacy.Properties;
using System;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_ItemSearch : Form
    {
        Connection conn = new Connection();
        private string Primarykey = "item_id";
        private string TblName = "view_item_stock";
        private string SelectedColumns = "item_id,item_name,item_type,item_barcode,current_item_qty,item_pur_price,item_sell_price,item_note";

        public frm_ItemSearch()
        {
            InitializeComponent();
        }

        private void frm_User_Load(object sender, EventArgs e)
        {
            txtSearch.ResetText();
            RefreshDVG();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frm_AddItem frm = new frm_AddItem(this))           
                frm.ShowDialog();
            

        }
        private void btnclean_Click(object sender, EventArgs e)
        {
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
                using (frm_AddItem frm = new frm_AddItem(this))
                {
                    //pesh awai pshan bdre dataka aiain ba primaryID
                    frm.PrimaryID = dgv.CurrentRow.Cells[Primarykey].Value.ToString();
                frm.ShowDialog();
                }

            }
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} WHERE (item_name LIKE N'%{txtSearch.Text.Trim()}%' OR item_type LIKE N'%{txtSearch.Text.Trim()}%'  )");
            dgv.ClearSelection();

            ResizeDGVHeader();



        }


        public void RefreshDVG()
        {
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblName} ORDER BY {Primarykey} DESC");
            dgv.ClearSelection();

            ResizeDGVHeader();
        }
        private void ResizeDGVHeader()
        {
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnEdit", Image = Resources.icons8_edit_row_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnDelete", Image = Resources.icons8_delete_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });
            
            dgv.Columns["item_id"].HeaderText = "id";
            dgv.Columns["item_name"].HeaderText = "Item Name";
            dgv.Columns["item_type"].HeaderText = "Type";
            dgv.Columns["item_barcode"].HeaderText = "barcode";
            dgv.Columns["current_item_qty"].HeaderText = "Store";
            dgv.Columns["item_pur_price"].HeaderText = "purchase price";
            dgv.Columns["item_sell_price"].HeaderText = "Sell Price";
            dgv.Columns["item_note"].HeaderText = "Note";

            dgv.Columns["btnEdit"].Width = 40;
            dgv.Columns["btnDelete"].Width = 40;


        }

    }
}
