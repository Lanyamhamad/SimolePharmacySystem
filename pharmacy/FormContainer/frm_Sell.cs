using pharmacy.ClassContainer;
using pharmacy.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_Sell : Form
    {
        Connection conn = new Connection();
        private string PrimaryInvoiceID = "0";
        private string PrimaryInvoiceKey = "sell_id";
        private string PrimaryItemKey = "seq";
        private string TblInvoiceName = "tbl_sell";
        private string TblItemName = "tbl_sell_item";
        private string SelectedColumns = "seq,(SELECT item_name FROM tbl_item WHERE item_id=item_id_fk ) AS item_name,sell_qty,item_sell_price,ROUND(sell_qty*item_sell_price,2)";      //bawai SELECT'aka awtret subquery ka item_name'man bo ahenetawa la regai foregin key'awa
                                                                                                                                             //am rounda bo katek ka agar ba dolar eshi krd w point'i tekawt bo awai point'aka dwrwdrezh nabe
        public frm_Sell()
        {
            InitializeComponent();
        }

       
     //pewist naka refresh bbetawa ,form load'man hichi nacheta naw



        
        private void btnNewInvoice_Click(object sender, EventArgs e)
        {
            txtBarcode.Text=string.Empty;
            txtInvoiceID.Text=PrimaryInvoiceID=txtSubTotalInvoice.Text=txtDiscount.Text=txtFinalDisc.Text ="0";
            txtInvoiceDate.Text = DateTime.Now.ToString("d");
            dgv.Columns.Clear();    

        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                //agar wti naxer naiata xwartr ,agar wti bale aisretawa
                if (MessageBox.Show("are you sure for acting this action?", "delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                conn.DeleteData(TblItemName,$"{PrimaryItemKey}  = { dgv.CurrentRow.Cells[PrimaryItemKey].Value.ToString()}");
                RefreshDVG();
            }
        }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)    //ka panjam na ba eraia am esham bo bka,pashan aw eshanai ka ala dwgmai save'da nwsibwman aihenina erawa
            {
                if (CheckEmptyText())
                {
                    return;
                }
                if (PrimaryInvoiceID == "0")
                {
                    //la date'da trim nanwsin kesha nia 
                    string MaxID =PrimaryInvoiceID=txtInvoiceID.Text= conn.GetData($"SELECT ISNULL(MAX ({PrimaryInvoiceKey}),0)+1 FROM {TblInvoiceName}").Rows[0][0].ToString();       //xoi Max waia agar table'akaw hichi tia nabe errort aiate boia isnull bakarahenin,agar null be sfrt aiate agar null'ish nabe 1'i axata sar 
                    conn.InsertData(TblInvoiceName, $"{MaxID},'{txtInvoiceDate.Text}',{txtDiscount.Text},0",true,false);
                    conn.InsertData(TblItemName, $"{MaxID},(SELECT item_id FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),1,(SELECT item_pur_price FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),(SELECT item_sell_price FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),0'");

                }
                else
                {
                    if (conn.GetData($"SELECT * FROM {TblItemName} where {PrimaryInvoiceKey}_fk={PrimaryInvoiceID} AND item_id_fk=(SELECT item_id FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}')").Rows.Count <= 0)
                        //invoice drwstnakain tanha yakjara
                        conn.InsertData(TblItemName, $"{PrimaryInvoiceID},(SELECT item_id FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),1,(SELECT item_pur_price FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),(SELECT item_sell_price FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}'),0'");
                    else
                        conn.UpdateData(TblItemName,"sell_qty=sell_qty+1",$"{PrimaryInvoiceKey}_fk={PrimaryInvoiceID} AND item_id_fk=(SELECT item_id FROM tbl_item WHERE item_barcode=N'{txtBarcode.Text.Trim()}')",false);

                }
                RefreshDVG();

            }
        }


        private void RefreshDVG()
        {
            txtBarcode.ResetText();
            dgv.Columns.Clear();
            dgv.DataSource = conn.GetData($"SELECT {SelectedColumns} FROM {TblItemName} WHERE {PrimaryInvoiceKey}_fk={PrimaryInvoiceID} AND archived=0");
            dgv.ClearSelection();

            ResizeDGVHeader();
            RefreshInvoiceTotal();
        }

        private void RefreshInvoiceTotal()
        {
            DataTable dt = conn.GetData($"SELECT sell_discount,sell_total_price,sell_final_price FROM view_sell_result WHERE {PrimaryInvoiceKey}={PrimaryInvoiceID}");

            txtSubTotalInvoice.Text = dt.Rows[0]["sell_total_price"].ToString();
            txtDiscount.Text = dt.Rows[0]["sell_discount"].ToString();
            txtFinalDisc.Text = dt.Rows[0]["sell_final_price"].ToString();
        }
        private void ResizeDGVHeader()
        {
            dgv.Columns.Add(new DataGridViewImageColumn() { Name = "btnDelete", Image = Resources.icons8_delete_24px, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });

            dgv.Columns[PrimaryItemKey].Visible = false;
            dgv.Columns["item_name"].HeaderText = "Item Name";
            dgv.Columns["item_qty"].HeaderText = "Quantity";
            dgv.Columns["item_sell_price"].HeaderText = "Price";
            dgv.Columns["item_total"].HeaderText = "Total";

            dgv.Columns["btnDelete"].Width = 40;


        }
        private bool CheckEmptyText()
        {
            bool Check = false;
            //wata agar text'aka batal bw
            if (string.IsNullOrWhiteSpace(txtBarcode.Text.Trim()))
            {
                MessageBox.Show("please first enter a Barcode of Item", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Check = true;
            }
            
            return Check;
        }


        //bo awai natwane la zhmara ziatr hichi tr daghl bkat
        private void txtExpPrice_KeyPress(object sender, KeyPressEventArgs e)   //ama tanha lasar discount hamana
        {
            //amanawe bejga la control button'akan w bejga la digit wata zhmarakan hichi tr naka
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }


       
    }
}
