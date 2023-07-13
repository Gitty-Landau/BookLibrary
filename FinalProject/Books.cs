using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace FinalProject
{
    class Books
    {
        enum Condition
        {Excellent,Good,Bad};
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";

        private string title;
        private string author;
        private int genreID;
        private int numOfPages;
        private int recommendedAge;
        private string currentCondition;
        
        public Books()
        {

        }
        public Books(string currentCondition, string title, string author, string genre, int numOfPages, int recommendedAge)
        {
            this.title = title;
            this.author = author;
            //Convert GenreName to GenreID
            SqlConnection conn = new SqlConnection("Data Source="+serverName+"; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT GenreID FROM Genres WHERE Name = '" + genre + "'", conn);
            
            genreID = Convert.ToInt32(cmd.ExecuteScalar());
            
            if (genreID <= 0)
            {
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Genres (Name) VALUES(@genre)", conn);
                cmd2.Parameters.Add(new SqlParameter("@genre", genre));
                cmd2.ExecuteNonQuery();
                genreID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            //
            this.numOfPages = numOfPages;
            this.recommendedAge = recommendedAge;
          
            this.currentCondition = currentCondition;
            
        }
        public void Add()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Books (Title, Author, GenreID, NumOfPages, RecommendedAge,CurrentCondition)VALUES(@title , @author , @genreID , @numOfPages , @recommendedAge , @currentCondition)", conn);
            cmd.Parameters.Add(new SqlParameter("@title", this.title));
            cmd.Parameters.Add(new SqlParameter("@author", this.author));
            cmd.Parameters.Add(new SqlParameter("@genreID", this.genreID));
            cmd.Parameters.Add(new SqlParameter("@numOfPages", this.numOfPages));
            cmd.Parameters.Add(new SqlParameter("@recommendedAge", this.recommendedAge));
            cmd.Parameters.Add(new SqlParameter("@currentCondition", this.currentCondition));
            cmd.ExecuteNonQuery();
            

            //Set BookID Attribute

            conn.Close();  
        }
        public void GetBookByTitle(string title)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT bookID,title,author, (SELECT Name FROM Genres WHERE genreID = "+genreID+") AS Genre, NumOfPages, RecommendedAge, CurrentCondition FROM Books WHERE Title = @title", conn);
            cmd.Parameters.Add(new SqlParameter("@title", title));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                Console.WriteLine("Book ID: " + dr.GetInt32(0) + "\t Title: " + dr.GetString(1) + "\t Author: " + dr.GetString(2) + "\t Genre: " + dr.GetString(3) + "\t Number of Pages: " + dr.GetInt32(4) + "\t Recommended Age: " + dr.GetInt32(5) + "\t Current Condition: " + dr.GetString(6));
            }
            conn.Close();


        }
        
      
        public void GetBookByAuthor(string author)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT bookID,title,author, (SELECT Name FROM Genres WHERE genreID = " + genreID + ") AS Genre, NumOfPages, RecommendedAge, CurrentCondition FROM Books WHERE Author = @author", conn);
            cmd.Parameters.Add(new SqlParameter("@author", author));
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                Console.WriteLine("Book ID: " + dr.GetInt32(0) + "\t Title: " + dr.GetString(1) + "\t Author: " + dr.GetString(2) + "\t Genre: " + dr.GetString(3) + "\t Number of Pages: " + dr.GetInt32(4) + "\t Recommended Age: " + dr.GetInt32(5) + "\t Current Condition: " + dr.GetString(6));
            }
            conn.Close();


        }
        public void Delete()
        {
           
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE TOP(1) FROM Books WHERE title = @title", conn);
            cmd.Parameters.Add(new SqlParameter("@title", this.title));
            cmd.ExecuteNonQuery();
            conn.Close();
            
        }

       
        public void Update<T>(string updateColumn, T newVal, int bookID)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Books SET "+updateColumn+" = @newVal WHERE BookID = "+bookID, conn);
          
          
            cmd.Parameters.Add(new SqlParameter("@newVal", newVal));
           
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void printBook()
        {
            Console.Write(title + ", " + author + ", " + genreID + ", " + numOfPages + ", " + recommendedAge + ", " + currentCondition);
            Console.WriteLine();
        }
      
        
    }
}
