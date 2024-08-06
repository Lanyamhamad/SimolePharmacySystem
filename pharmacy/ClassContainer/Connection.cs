using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace pharmacy.ClassContainer
{
    public class Connection
    {
        private string connectionstring = @"Data Source=DESKTOP-SH2ET8I;Initial Catalog=db_pharmacy;Integrated Security=True";

        public SqlConnection GetConnection()
        {
            SqlConnection con=new SqlConnection(connectionstring);
            con.Open();
            return con;
        }
        public DataTable GetData(String query)
        {
            DataTable dt = new DataTable();
            //linkekman lagal server'akaia drwst krdwa
            using (SqlConnection con=GetConnection()) 
            {
                //nafarekman bakre grtwa query'iakman dawate w nawnishanekman dawate alein bro ba pei aw query'ia shtman bo bena
                using (SqlDataAdapter sda =new SqlDataAdapter(query,con))
                {
                    sda.Fill(dt);
                }
            }
            return dt;

        }
        public void InsertData(string Tablename, string TableValue, bool HasEntryby = true, bool HasUpdateby = true)
        {
            string insertQuery = $"INSERT INTO {Tablename} VALUES ({TableValue}{(HasEntryby == true ? $",{UserInfo.UserId}, GETDATE()" : "")} {(HasUpdateby == true ? $", NULL , NULL " : "")});";
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void UpdateData(string Tablename, string TableValue,string TableWhere ,bool HasUpdateby = true)
        {
            string UpdateQuery = $"UPDATE {Tablename} SET {TableValue}{(HasUpdateby == true ? $",e_by={UserInfo.UserId},u_date=GETDATE()" : "")} WHERE {TableWhere};";
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(UpdateQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void DeleteData(string Tablename,string TableWhere)
        {
            string DeleteQuery = $"UPDATE {Tablename} SET archived=1 WHERE {TableWhere};";
            MessageBox.Show(DeleteQuery);
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(DeleteQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
