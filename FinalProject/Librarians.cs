using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class Librarians:Person
    {

        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private int yrsInCompany;
        private decimal hrlyWage;

        public Librarians()
        {

        }
        public string GetName()
        {
            return base.name;
        }
        public decimal GetSalary() { return this.hrlyWage; }
        public Librarians(string name, int age, string phoneNumber,string userName, string password, decimal hrlyWage, int yrsInCompany = 0) : base(name, age, phoneNumber,userName, password)
        {
        
            this.yrsInCompany = yrsInCompany;
            this.hrlyWage = hrlyWage;
           
        }
        public void SetByUserNamePassword(string username, string password)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, PhoneNumber, YearsInCompany, HourlyWage FROM Librarians WHERE UserName = @userName AND Password = @password", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", username));
            cmd.Parameters.Add(new SqlParameter("@password", password));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                this.name = dr.GetString(0);
                this.phoneNumber = dr.GetString(1);
                this.yrsInCompany = dr.GetInt32(2);
                this.hrlyWage = dr.GetDecimal(3);
                this.userName = username;
                this.password = password;
            }
            conn.Close();
        }
        public void Add()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Librarians (Name, Age, PhoneNumber, UserName, Password, YearsInCompany, HourlyWage) VALUES (@name, @age, @phoneNumber, @userName , @password, @yrsInComp, @hrlyWage)", conn);
            cmd.Parameters.Add(new SqlParameter("@name", this.name));
            cmd.Parameters.Add(new SqlParameter("@age", this.age));
            cmd.Parameters.Add(new SqlParameter("@phoneNumber", this.phoneNumber));
            cmd.Parameters.Add(new SqlParameter("@userName", this.userName));
            cmd.Parameters.Add(new SqlParameter("@password", this.password));
            cmd.Parameters.Add(new SqlParameter("@yrsInComp", this.yrsInCompany));
            cmd.Parameters.Add(new SqlParameter("@hrlyWage", this.hrlyWage));
            cmd.ExecuteNonQuery();

            conn.Close();

        }
        public void Delete()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Librarians WHERE Username = @username", conn);
            cmd.Parameters.Add(new SqlParameter("@username", this.userName));
            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public void Update<T>(T updateColumn, T newVal)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Librarians SET "+updateColumn+" = @newVal WHERE Username = @userName", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", this.userName));
            cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void SearchByName(string letter)
        {
            base.SearchByName("Librarians", letter);
        }

        public void SearchByPhoneNumber(string phoneNumber)
        {
            base.SearchByPhoneNumber("Librarians", phoneNumber);
        }


        public int CountLibrarians()
        {
           return CountPeople("Librarians");
        }
        public bool ConfirmPswd(string currentPswd)
        {

            //Get CurrentPswd
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Password FROM Librarians WHERE userName = @userName", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", this.userName));
            if (currentPswd == Convert.ToString(cmd.ExecuteScalar()))
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }
        public void UpdatePassword(string newPswd)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("UPDATE Librarians SET Password = @newPswd  WHERE UserName = @userName", conn);
            cmd2.Parameters.Add(new SqlParameter("@newPswd", newPswd));
            cmd2.Parameters.Add(new SqlParameter("@userName", this.userName));
            cmd2.ExecuteNonQuery();
            conn.Close();

        }
    }
}
