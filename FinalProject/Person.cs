using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Person
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        virtual protected string name{ get; set; } 
        protected int age;
        protected string phoneNumber;
        protected string userName;
        protected string password;

        public Person()
        {

        }
           

        public Person(string name, int age, string phoneNumber, string userName, string password)
        {
            this.name = name;   
            this.age = age;
            this.phoneNumber = phoneNumber;
            this.userName = userName;
            this.password = password;

        }

        virtual public void SearchByName(string table,string letter)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, PhoneNumber FROM "+table+" "+"WHERE UserName LIKE @ltr+'%'", conn);
            cmd.Parameters.Add(new SqlParameter("@ltr", letter));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0)+"\t"+dr.GetInt32(1));
                Console.WriteLine();
            }
            conn.Close();
        }
       
        virtual public void SearchByPhoneNumber(string table, string phoneNumber)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, PhoneNumber FROM " + table + " WHERE PhoneNumber = @phoneNum", conn);
            cmd.Parameters.Add(new SqlParameter("@phoneNum", phoneNumber));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0) + "\t" + dr.GetInt32(1));
                Console.WriteLine();
            }
            conn.Close();
        }
        public int CountPeople(string table)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM "+table, conn);
            int total = Convert.ToInt32(cmd2.ExecuteScalar());
            conn.Close();
            return total;
        }


    }
}
