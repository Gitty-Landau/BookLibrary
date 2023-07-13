using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinalProject
{
    internal class Member : Person
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private  DateTime signupDate;
        private  long creditCardInfo;
    
        public Member()
        {

        }
        
        public Member(string name, int age, string phoneNumber, long creditCardInfo,string userName, string password, DateTime ?signupDate) : base(name, age, phoneNumber, userName, password)
        {
            if(signupDate == null) { this.signupDate = DateTime.Now; }
            this.creditCardInfo  = creditCardInfo;
          
        }

        public string GetName()
        {
            return base.name;
        }
        public void Add()
        {
            
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Members (Name, Age, PhoneNumber, UserName, Password, SignupDate, CreditCardInfo) VALUES (@name, @age, @phoneNumber, @userName , @password, @signupDate, @creditCardInfo)", conn);
            cmd.Parameters.Add(new SqlParameter("@name", this.name));
            cmd.Parameters.Add(new SqlParameter("@age", this.age));
            cmd.Parameters.Add(new SqlParameter("@phoneNumber", this.phoneNumber));
            cmd.Parameters.Add(new SqlParameter("@userName", this.userName));
            cmd.Parameters.Add(new SqlParameter("@password", this.password));
            cmd.Parameters.Add(new SqlParameter("@signupDate", this.signupDate));
            cmd.Parameters.Add(new SqlParameter("@creditCardInfo", this.creditCardInfo));
            cmd.ExecuteNonQuery();
           
            conn.Close();
        }

        public void Delete()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Members WHERE Username = @username", conn);
            cmd.Parameters.Add(new SqlParameter("@username", this.userName));
            cmd.ExecuteNonQuery();
            conn.Close();
          
        }

        public void Update<T>(string updateColumn, T newVal)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Members SET "+updateColumn+" = @newVal WHERE Username = @userName", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", base.userName));
            cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int CountMembers()
        {
            return CountPeople("Members");
        }
        public void GetAllMembers()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, PhoneNumber, SignupDate, Age FROM Members", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0) + "\t" + dr.GetString(1) + "\t" + dr.GetDateTime(2) + "\t" + dr.GetInt32(3));
                Console.WriteLine();
            }
            conn.Close();
        }
        public void SetByUserNamePassword(string username, string password)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name, PhoneNumber, SignupDate, Age FROM Members WHERE UserName = @userName AND Password = @password", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", username));
            cmd.Parameters.Add(new SqlParameter("@password", password));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                base.name= dr.GetString(0); 
                base.phoneNumber=dr.GetString(1);
                this.signupDate = dr.GetDateTime(2);
                base.age=dr.GetInt32(3);
                base.userName = username;
                base.password = password;
            }
            conn.Close();
        }
        public void PrintMember()
        {
            Console.WriteLine("Name: " + name + "\t age: " + age + "\t Phone Number: "+phoneNumber+"\t SignUpDate: "+signupDate+"\t Credit Card #: "+creditCardInfo+"\t UserName: "+userName+"\t Password: "+password);
        }
        public  void  SearchByName(string letter)
        {
            base.SearchByName("Members", letter); 
        }

        public void SearchByPhoneNumber(string phoneNumber)
        {
            base.SearchByPhoneNumber("Members", phoneNumber);
        }
        public bool ConfirmPswd(string currentPswd)
        {

            //Get CurrentPswd
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Password FROM Members WHERE userName = @userName", conn);
            cmd.Parameters.Add(new SqlParameter("@userName", base.userName));
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
                SqlCommand cmd2 = new SqlCommand("UPDATE Members SET Password = @newPswd  WHERE UserName = @userName", conn);
                cmd2.Parameters.Add(new SqlParameter("@newPswd", newPswd));
                cmd2.Parameters.Add(new SqlParameter("@userName", this.userName));
                cmd2.ExecuteNonQuery();
                conn.Close();

        }
       

    }
}
