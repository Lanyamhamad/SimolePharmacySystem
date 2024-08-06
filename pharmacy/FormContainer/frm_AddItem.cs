using pharmacy.ClassContainer;
using pharmacy.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_AddItem : Form
    {
        Connection conn = new Connection();
        public string PrimaryID = "0";
        private string Primarykey = "item_id";
        private string TblName = "tbl_item";
        // Select la abain chwnka na searchman haia na dgv
        frm_ItemSearch FrmItemSearch;
        public frm_AddItem(frm_ItemSearch FrmItemSearch)
        {
            InitializeComponent();
            this.FrmItemSearch = FrmItemSearch;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            //ka formaka load bw ale bzanm ID iaka hichi tiaia agar hichi tia nabw awa aw derai xwarawa run aka
            if(PrimaryID!="0")
            {
                GetDataToText();
            }

        }
        private void btnclean_Click_1(object sender, EventArgs e)
        {
            txtItemName.Text = txtItemType.Text = txtItemBarcode.Text = txtNote.Text = string.Empty;
            //wata aw 3 text'a batal bkarawaw bikara naw purchase qty
            txtPurchaseQty1.Text = txtPurchasePrice.Text = txtSellPrice.Text = txtPurchaseQty2.Text = txtIncomeQty.Text = txtSellQty.Text = txtCurrentQty.Text = PrimaryID = "0";
            RefreshCMD();
            FrmItemSearch.RefreshDVG();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (CheckEmptyText())
            {
                return;
            }
            if (PrimaryID == "0")
            {
                //la date'da trim nanwsin kesha nia 
                conn.InsertData(TblName, $"N'{txtItemName.Text.Trim()}',N'{txtItemType.Text.Trim()}',N'{txtItemBarcode.Text.Trim()}',{txtPurchaseQty1.Text},{txtPurchasePrice.Text},{txtSellQty.Text},N'{txtNote.Text.Trim()}',0");
            }
            else
            {
                conn.UpdateData(TblName, $"item_name=N'{txtItemName.Text.Trim()}', item_type=N'{txtItemType.Text.Trim()}', item_barcode=N'{txtItemBarcode.Text.Trim()}',item_qty={txtPurchaseQty1.Text},item_pur_price={txtPurchasePrice.Text},item_sell_price={txtSellQty.Text},item_note=N'{txtNote.Text.Trim()}'", $"{Primarykey}={PrimaryID}");

            }
            //am derai xwarawa wata panja anem ba dwgmai clean'da
            btnclean.PerformClick();

        }
        //nwsrawa 2reference labar awai bo hardw button'akaia 
        private void btnQty_Click(object sender, EventArgs e)
        {
            string operation = "";
            if (sender as Button == btnInc) //agar click'm lam button'a krd awa amam bo bka
                operation = $"+";
            else if (sender as Button == btnDec)
                operation = $"-";

            conn.UpdateData(TblName, $"item_qty=item_qty{operation}{txtIncomeQty.Text}", $"{Primarykey}={PrimaryID}");      //item_qty xoi chana alein kami yan zaedi aw 3adadai ka lanaw qty'iakaiaia 
            DataTable DT = conn.GetData($"SELECT * FROM view_item_stock WHERE {Primarykey}={PrimaryID}");
            txtPurchaseQty1.Text=txtPurchaseQty2.Text = DT.Rows[0]["item_qty"].ToString();
            txtSellQty.Text = DT.Rows[0]["total_sell_qty"].ToString();
            txtCurrentQty.Text = DT.Rows[0]["current_item_qty"].ToString();
            txtIncomeQty.Text = "0";
        }


        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //amanawe bejga la control button'akan w bejga la digit wata zhmarakan hichi tr naka
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }
        private void RefreshCMD()
        {
            //combobox'aka data akata nawi
            //bo awai jorakani Item'aka benetawa
            txtItemType.DataSource = conn.GetData($"SELECT DISTINCT item_type FROM {TblName} WHERE archived=0");//aw shtai ka pshani user'akai aiam britia lamai xwarawa
            txtItemType.DisplayMember= "item_type";
            txtItemType.Text= string.Empty;
        }
        private void GetDataToText()
        {
            DataTable DT = conn.GetData($"SELECT * FROM view_item_stock WHERE {Primarykey}={PrimaryID}");
 
            txtItemName.Text = DT.Rows[0]["item_name"].ToString();
            txtItemType.Text = DT.Rows[0]["item_type"].ToString();
            txtItemBarcode.Text = DT.Rows[0]["item_barcode"].ToString();
            txtPurchaseQty1.Text = DT.Rows[0]["item_qty"].ToString();
            txtPurchasePrice.Text = DT.Rows[0]["item_pur_price"].ToString();
            txtSellPrice.Text = DT.Rows[0]["item_sell_price"].ToString();
            txtNote.Text = DT.Rows[0]["item_note"].ToString();
            txtPurchaseQty2.Text = DT.Rows[0]["item_qty"].ToString();
            txtSellQty.Text = DT.Rows[0]["total_sell_qty"].ToString();
            txtCurrentQty.Text = DT.Rows[0]["current_item_qty"].ToString();

        }
        //bo awai natwane la zhmara ziatr hichi tr daghl bkat
        private bool CheckEmptyText()
        {
            bool Check = false;
            //wata agar text'aka batal bw
            if (string.IsNullOrWhiteSpace(txtItemType.Text.Trim()))
            {
                MessageBox.Show("please enter a Type of Item", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            else if (string.IsNullOrWhiteSpace(txtItemName.Text.Trim()))
            {
                MessageBox.Show("please enter a Name of Item", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            else if (string.IsNullOrWhiteSpace(txtItemBarcode.Text.Trim()))
            {
                MessageBox.Show("please enter a Barcode", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }

            return Check;
        }

    }
}
