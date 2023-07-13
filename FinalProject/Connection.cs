using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace FinalProject
{
    class Connection
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataTable dt;
        public Connection(string DbName, string serverName, string cmd)
        {
            conn = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + DbName + ";Integrated Security=True");
            this.cmd = new SqlCommand(cmd, conn);
            adapter = new SqlDataAdapter();
            adapter.SelectCommand = this.cmd;
        }
        public Connection(string DbName, string serverName)
        {
            conn = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + DbName + ";Integrated Security=True");
       
         
        }
        public void SetConnection(string DbName, string serverName)
        {
             conn = new SqlConnection("Data Source="+serverName+";Initial Catalog="+DbName+";Integrated Security=True");
        }

        public void SetCommand(string cmd)
        {

         this.cmd = new SqlCommand(cmd, conn);
            adapter = new SqlDataAdapter();
            adapter.SelectCommand = this.cmd;
        }
        public void SetAdapter()
        {
            adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
         
        }
        public void Insert()
        {
            cmd.ExecuteNonQuery();
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.Update(dt);
        }
        public bool Open()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                return true;
            }
            return false;
        }
        public bool Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                return true;
            }
            return false;
        }
        public bool SetDataTable(string title)
        {
            dt = new DataTable(title);
            adapter.Fill(dt);
            return true;
        }
        public void PrintDataTable()
        {
            foreach (DataColumn dc in dt.Columns)
            {
                Console.Write(dc.ColumnName + "\t");
            }
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Console.Write(dr[i] + "\t");
                }
                Console.WriteLine();
            }
        }
        public int GetID()
        {
            int val = 0;
            foreach (DataRow dr in dt.Rows)
            {
               
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                   val = Convert.ToInt32(dr[i]);
                }
               
            }
            return val;
        }
    }
}
