using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FinalProject
{
    class Rating
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private int booksInOrderID;
        private int rating;
        private string comments;


       public Rating()
        {

        }
       public Rating( int rating, string comments, int orderID)
        {
            //Get bookOrderID
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Title, Books.BookID FROM Books INNER JOIN BooksInOrders ON Books.BookID = BooksInOrders.BookID WHERE OrderID = @orderID", conn);
            cmd.Parameters.Add(new SqlParameter("@orderID", orderID));
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("Select the book you choose to rate: ");
            int counter = 1;
            while (dr.Read())
            {
                Console.Write(counter + ":" + dr.GetString(0));
                Console.WriteLine();
                counter++;
            }
            conn.Close();
            conn.Open();
            int chosenBook = Convert.ToInt32(Console.ReadLine());
            List<int> list = new List<int>();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Convert.ToInt32(dr.GetInt32(1)));
            }
            int userbookID = list[chosenBook - 1];
            conn.Close();
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT BooksInOrderID FROM BooksInOrders INNER JOIN Books ON Books.BookID = BooksInOrders.BookID WHERE Books.BookID = @bookID and BooksInOrders.OrderID = @orderID; ", conn);
            cmd2.Parameters.Add(new SqlParameter("@bookID", userbookID));
            cmd2.Parameters.Add(new SqlParameter("@orderID", orderID));
            booksInOrderID = Convert.ToInt32(cmd2.ExecuteScalar());
            conn.Close();
            //Other attributes
            this.rating = rating;
            this.comments = comments;
            
        }
        public void SetRatingAndComment(int rating, string comments)
        {
            this.rating = rating;
            this.comments = comments;
        }
        public void SetBookID(int orderID)
        {
            //Get bookOrderID
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Title, Books.BookID FROM Books INNER JOIN BooksInOrders ON Books.BookID = BooksInOrders.BookID WHERE OrderID = @orderID", conn);
            cmd.Parameters.Add(new SqlParameter("@orderID", orderID));
            SqlDataReader dr = cmd.ExecuteReader();
            Console.WriteLine("Select the book you choose to rate: ");
            int counter = 1;
            while (dr.Read())
            {
                Console.Write(counter + ":" + dr.GetString(0));
                Console.WriteLine();
                counter++;
            }
            conn.Close();
            conn.Open();
            int chosenBook = Convert.ToInt32(Console.ReadLine());
            List<int> list = new List<int>();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Convert.ToInt32(dr.GetInt32(1)));            
            }
            int userbookID = list[chosenBook - 1];          
            conn.Close();
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT BooksInOrderID FROM BooksInOrders INNER JOIN Books ON Books.BookID = BooksInOrders.BookID WHERE Books.BookID = @bookID and BooksInOrders.OrderID = @orderID; ", conn);
            cmd2.Parameters.Add(new SqlParameter("@bookID", userbookID));
            cmd2.Parameters.Add(new SqlParameter("@orderID", orderID));
            booksInOrderID = Convert.ToInt32(cmd2.ExecuteScalar());
            conn.Close();
        }

        public void Add()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            Console.WriteLine("INSERT INTO Ratings (BooksInOrderID, Rating, Comments) VALUES (" + booksInOrderID + ", " + rating + ", '" + comments + "')");
            SqlCommand cmd = new SqlCommand("INSERT INTO Ratings (BooksInOrderID, Rating, Comments) VALUES ("+booksInOrderID+", "+rating+", '"+comments+"')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        
        //Deleting by the BooksInOrderID
        public void Delete()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Ratings WHERE BooksInOrderID = "+booksInOrderID, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Update<T>(string updateColumn, T newVal)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Ratings SET  "+updateColumn+ " = " +newVal +" WHERE BooksInOrderID = "+booksInOrderID, conn);
           // cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
           
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void SearchByTitle(string letter)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Title, Rating, comments FROM Books  INNER JOIN BooksInOrders ON BooksInOrders.BookID = Books.BookID INNER JOIN Ratings ON BooksInOrders.BooksInOrderID = Ratings.BooksInOrderID WHERE Title LIKE @ltr+'%'", conn);
            cmd.Parameters.Add(new SqlParameter("@ltr", letter));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0)+"\t"+dr.GetInt32(1)+"\t" + dr.GetString(2));
                Console.WriteLine();
            }
            conn.Close();
        }
        public void GetRatingRange(int rating)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Title, Rating, comments FROM Books  INNER JOIN BooksInOrders ON BooksInOrders.BookID = Books.BookID INNER JOIN Ratings ON BooksInOrders.BooksInOrderID = Ratings.BooksInOrderID WHERE Rating <= "+rating, conn);
            cmd.Parameters.Add(new SqlParameter("@ltr", rating));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0) + "\t" + dr.GetInt32(1) + "\t" + dr.GetString(2));
                Console.WriteLine();
            }
            conn.Close();
        }

        public void PrintRating()
        {
            Console.WriteLine("BooksInOrderID: "+booksInOrderID +", Rating: "+rating+", Comments: "+comments);
        }

    }
}
