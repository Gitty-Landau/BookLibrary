using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FinalProject
{
    internal class BooksInOrder
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private int orderID;
        private int bookID;
        private string originalCondition=null;
        private string returnedCondtion=null;
        private DateTime? dateReturned;

        public BooksInOrder()
        {

        }
        public BooksInOrder(int orderID, int bookID, DateTime? dateReturned=null, string originalCondition = null, string returnedCondtion = null)
        {
            this.orderID = orderID;
            this.bookID = bookID;
            if (originalCondition == null)
            {
                SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CurrentCondition FROM Books WHERE bookID = @bookID;", conn);
                cmd.Parameters.Add(new SqlParameter("@bookID", bookID));
                this.originalCondition = Convert.ToString(cmd.ExecuteScalar());
                conn.Close();

            }
            else
            {
                this.originalCondition = originalCondition;
            }
            this.returnedCondtion = returnedCondtion;
            if (dateReturned != null) { this.dateReturned = Convert.ToDateTime(dateReturned); }

        }

        public void Add()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            //set original condition
            string currentCondtion = "";
            if (originalCondition == null)
            {
                SqlCommand cmd1 = new SqlCommand("SELECT Books.CurrentCondition FROM Books WHERE BookID = "+bookID, conn);
                currentCondtion  = Convert.ToString(cmd1.ExecuteScalar());
            }
            conn.Close();
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO BooksInOrders (orderID, BookID, originalCondition, ReturnedCondition, DateReturned) VALUES ("+orderID+","+bookID+ ", @originalCondition, @returnedCondition, @dateReturned)", conn);
            if (originalCondition != null) { cmd.Parameters.Add(new SqlParameter("@originalCondition", originalCondition)); }
            else { cmd.Parameters.Add(new SqlParameter("@originalCondition", currentCondtion)); }
           
            if(returnedCondtion != null) { cmd.Parameters.Add(new SqlParameter("@returnedCondition", returnedCondtion)); }
            else { cmd.Parameters.Add(new SqlParameter("@returnedCondition", DBNull.Value)); }

            if (dateReturned != null) { cmd.Parameters.Add(new SqlParameter("@dateReturned", dateReturned)); }
            else { cmd.Parameters.Add(new SqlParameter("@dateReturned", DBNull.Value)); }

            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Delete()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM BooksInOrders WHERE OrderID = "+orderID+" AND BookID = "+bookID, conn);
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public void Update<T>(string updateColumn, T newVal, int bookID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE BooksInOrders SET " + updateColumn + " = @newVal WHERE BookID = " + bookID+" And OrderID ="+orderID, conn);
            cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
            cmd.ExecuteNonQuery();
            conn.Close();

        }

      
    }
}
