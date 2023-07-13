using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;

namespace FinalProject
{
    class Orders
    {

        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";
        private int memberID;
        private int librarianID;
        private DateTime borrowDate;
        private DateTime dueDate;

        public Orders()
        {

        }
        public Orders(string memberUserName, string librarianUserName, DateTime ?borrowDate=null, DateTime ?dueDate=null)
        {
            if (borrowDate == null)
            {
               this.borrowDate = DateTime.Now;
            }
            else
            {
               this.borrowDate = Convert.ToDateTime(borrowDate);
            }
            if (dueDate == null)
            {
                this.dueDate = DateTime.Now.AddDays(14);
            }
            else 
            {
                this.dueDate = Convert.ToDateTime(dueDate);
            }


            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            //MemberID
            SqlCommand cmd = new SqlCommand("SELECT memberID FROM Members WHERE UserName = @memberName;", conn);
            cmd.Parameters.Add(new SqlParameter("@memberName", memberUserName));
            memberID=Convert.ToInt32(cmd.ExecuteScalar());
            //LibrarianID
            SqlCommand cmd2 = new SqlCommand("SELECT LibrarianID FROM Librarians WHERE UserName = @librarianName;", conn);
            cmd2.Parameters.Add(new SqlParameter("@librarianName", librarianUserName));
            librarianID = Convert.ToInt32(cmd2.ExecuteScalar());

        }

        public void SetWithOrderID(int orderID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");

            conn.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT MemberID, LibrarianID, BorrowDate, DueDate  FROM Orders WHERE OrderID = @OrderID;", conn);
            cmd3.Parameters.Add(new SqlParameter("@OrderID", orderID));
            SqlDataReader dr = cmd3.ExecuteReader();
            while (dr.Read())
            {
                Console.WriteLine("Member ID: " + dr.GetInt32(0) + "\t LibrarianID: " + dr.GetInt32(1) + "\t Borrow Date : " + dr.GetDateTime(2)+ "\t Returned Date : " + dr.GetDateTime(3));
            }
            conn.Close();

        }
        public void GetNonReturnedOrders(string userName)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT Orders.OrderID FROM Orders INNER JOIN Members On Members.MemberID = Orders.MemberID  INNER JOIN BooksInOrders On BooksInOrders.OrderID = Orders.OrderID WHERE UserName = '"+userName+"' AND DateReturned is null", conn);
            SqlDataReader dr = cmd3.ExecuteReader();
            List<int> list = new List<int>();
            while (dr.Read())
            {
                list.Add(dr.GetInt32(0));
            }
            if (list.Count < 1)
            {
                Console.WriteLine("All your borrowed books have been returned. Feel free to approach one of our librarians to assist you in completing another order!");
            }
            else
            {
                Console.WriteLine("Please see below any orders containing non-returned books: ");
                foreach (int item in list)
                {
                    Console.WriteLine("Order# " + item + ": ");
                    GetBooksInOrder(item);
                }
            }
            

            conn.Close();
        }
        public void GetMemberOrders(string userName)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT OrderID FROM Orders INNER JOIN Members On Members.MemberID = Orders.MemberID WHERE UserName = '" + userName+"'", conn);
            SqlDataReader dr = cmd3.ExecuteReader();
            List<int> list = new List<int>();
            while (dr.Read())
            {
                list.Add(dr.GetInt32(0));
            }
            if (list.Count < 1)
            {
                Console.WriteLine("You have no orders yet. Feel free to approach one of our librarians to assist you in completing your very first order!");
            }
            else
            {
                Console.WriteLine("Please see below your orders: ");

                foreach (int item in list)
                {
                    Console.WriteLine("Order# " + item + ": ");
                    GetBooksInOrder(item);
                }
            }
             

            conn.Close();
        }
        public void Add(int[] bookIDs, string originalCondition=null)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Orders (MemberID, LibrarianID, BorrowDate, DueDate)VALUES("+memberID +", "+librarianID+", '"+borrowDate+"', '"+dueDate+ "'); SELECT SCOPE_IDENTITY()", conn);
            int orderID = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            foreach(int bookID in bookIDs) 
            {
                BooksInOrder b1 = new BooksInOrder(orderID, bookID, null, originalCondition, null);
                b1.Add();
            }

        }
        public void Delete( int orderID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT  bookID FROM BooksInOrders INNER JOIN Orders ON Orders.OrderID = BooksInOrders.OrderID WHERE Orders.OrderID = " + orderID, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            Stack<int> stack = new Stack<int>();
            while (dr.Read())
            {
                stack.Push(Convert.ToInt32(dr.GetInt32(0)));
            }
            foreach (int bookID in stack)
            {
                BooksInOrder b1 = new BooksInOrder(orderID, bookID);
                b1.Delete();

            }
            conn.Close();
            conn.Open();
            SqlCommand cmd2 = new SqlCommand("DELETE FROM Orders WHERE OrderID = "+orderID, conn);
            cmd2.ExecuteNonQuery();
            conn.Close();

        }

        public void Update<T>(string updateColumn, T newVal, int orderID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Orders SET " + updateColumn + " = @newVal WHERE OrderID = " + orderID, conn);
            cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public void GetBooksInOrder(int orderID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT BookID FROM BooksInOrders WHERE OrderID = @OrderID;", conn);
            cmd.Parameters.Add(new SqlParameter("@OrderID", orderID));
            SqlDataReader dr = cmd.ExecuteReader();
            Stack<int> stack = new Stack<int>();
            while (dr.Read())
            {
                stack.Push(Convert.ToInt32(dr.GetInt32(0)));
            }
            conn.Close();
            conn.Open();
            string[] bookTitles = new string[stack.Count];
            int counter = 0;
            foreach (int bookID in stack)
            {
                SqlCommand cmd2 = new SqlCommand("SELECT title from Books where BookID = " + bookID, conn);
                bookTitles[counter]=Convert.ToString(cmd2.ExecuteScalar());
                counter++;
            }
            conn.Close();
            conn.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT OriginalCondition, ReturnedCondition, DateReturned  FROM BooksInOrders WHERE OrderID = @OrderID;", conn);
            cmd3.Parameters.Add(new SqlParameter("@OrderID", orderID));
            dr = cmd3.ExecuteReader();
            counter = bookTitles.Length-1;
            while (dr.Read())
            {
                Console.WriteLine(bookTitles[counter]+": ");
                if (!dr.IsDBNull(0))
                    Console.Write(" Original Condition: "+dr.GetString(0));
                else
                    Console.Write(" Original Condition: Note Entered ");
                if (!dr.IsDBNull(1))
                    Console.Write("\tReturned Condition: " + dr.GetString(1));
                else
                    Console.Write("\tReturned Condition: Not Entered");
                if (dr.IsDBNull(2))
                    Console.Write("\tDate Returned: Not returned");
                else
                    Console.Write("\tDate Returned: "+(dr.GetDateTime(2)));


                counter--;
                Console.WriteLine();
            }


            conn.Close();

        }

        public void ReturnBook(int bookID, string returnedCondition)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT CurrentCondition FROM Books WHERE BookID = " + bookID, conn);
            string inputedCondition = Convert.ToString(cmd.ExecuteScalar());
            if (inputedCondition != returnedCondition)
            {
                Books b = new Books();
                b.Update("CurrentCondition", returnedCondition, bookID);
            }
            BooksInOrder bo = new BooksInOrder();

           bo.Update("ReturnedCondition", returnedCondition, bookID);
           bo.Update("DateReturned", DateTime.Now, bookID);

        }
        public void PrintOrder()
        {
            Console.WriteLine(memberID+"\t"+librarianID+ "\t" +borrowDate+ "\t" +dueDate);
        }


    }
}
