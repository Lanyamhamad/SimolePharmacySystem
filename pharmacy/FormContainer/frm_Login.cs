using pharmacy.ClassContainer;
using System;
using System.Data;
using System.Windows.Forms;

namespace pharmacy.FormContainer

{
    public partial class frm_Login : Form
    {
        Connection conn = new Connection();

        public frm_Login()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtusername.Text.Trim()) || string.IsNullOrWhiteSpace(txtpass.Text.Trim()))
            {
                MessageBox.Show("please enter username and password!!");
                return;
            }
            DataTable dt=conn.GetData($"SELECT user_id,username,fullname FROM tbl_user WHERE username='{txtusername.Text.Trim()}' AND pass='{txtpass.Text.Trim()}' AND archived=0;");
            if(dt.Rows.Count > 0 )
            {
                UserInfo.UserId = dt.Rows[0]["user_id"].ToString();
                UserInfo.Username = dt.Rows[0]["username"].ToString();
                UserInfo.Fullname = dt.Rows[0]["fullname"].ToString();
                using (frm_Main frm=new frm_Main()) 
                {
                    this.Hide();
                    frm.ShowDialog();
                    this.Show();
                    txtusername.Text=txtpass.Text=string.Empty;
                    UserInfo.UserId = "0";
                    UserInfo.Username = UserInfo.Fullname = "";
                }
            }
            else
            {
                MessageBox.Show("incorrect username or password");
            }
        
        }

        private void closelabel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
