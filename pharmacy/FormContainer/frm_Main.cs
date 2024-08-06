using pharmacy.ClassContainer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pharmacy.FormContainer
{
    public partial class frm_Main : Form
    {
        public frm_Main()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            lblUser.Text = "User:" + UserInfo.Fullname;
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            using(frm_User frm= new frm_User()) 
                frm.ShowDialog();
        }

        private void btnExpence_Click(object sender, EventArgs e)
        {
            using (frm_Expense frm = new frm_Expense())
                frm.ShowDialog();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            using (frm_ItemSearch frm = new frm_ItemSearch())
                frm.ShowDialog();

        }

        private void btnSaleItems_Click(object sender, EventArgs e)
        {
            using (frm_Sell frm = new frm_Sell())
                frm.ShowDialog();
        }
    }
}
