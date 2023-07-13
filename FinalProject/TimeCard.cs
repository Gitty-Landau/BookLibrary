using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace FinalProject
{
    class TimeCard
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private int librarianID;
        private DateTime? clockIn;
        private DateTime? clockOut;

        public TimeCard(int librarianID)
        {
            this.librarianID = librarianID; 
        }
        public void ClockIn()
        {
            Console.WriteLine("INSERT INTO TimeCard (ClockIn) VALUES ('" + DateTime.Now+ "') WHERE LibrarianID = ");
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TimeCard (LibrarianID, ClockIn) VALUES ("+librarianID+", '"+DateTime.Now.ToString()+"')", conn);
            cmd.ExecuteNonQuery();
        }
        public void ClockOut()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 ClockOut FROM TimeCard  WHERE LibrarianID = " + librarianID+ "ORDER BY TimeCardID DESC", conn);
            string lastClockOut = Convert.ToString(cmd.ExecuteScalar());
            Console.WriteLine( lastClockOut);
                if (lastClockOut==string.Empty)
                    {
               
                    SqlCommand cmd2 = new SqlCommand("UPDATE TimeCard SET ClockOut = '"+ DateTime.Now + "' WHERE LibrarianID="+librarianID+ " AND TimeCardID =(select max(TimeCardID) from TimeCard where LibrarianID =" + librarianID + ")", conn);
                    cmd2.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine("You have not clocked in today.");
                }
         
            conn.Close();
           
        }

        public void GetLibrarianTimeCards(int librarianID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT ClockIn, ClockOut FROM TimeCard WHERE LibrarianID = @librarianID", conn);
            cmd.Parameters.Add(new SqlParameter("@librarianID", librarianID));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[1] == DBNull.Value)
                {
                    Console.WriteLine("Clocked In: " + dr.GetDateTime(0) + "\t Clocked Out: NULL");
                }
                else if (dr[0] == DBNull.Value)
                { 
                    Console.WriteLine("Clocked In: NULL \t Clocked Out: NULL");
                }
                else
                {
                    Console.WriteLine("Clocked In: " + dr.GetDateTime(0) + "\t Clocked Out: "+ dr.GetDateTime(1));

                }
                  

            }
            conn.Close();

        }

    }
}
