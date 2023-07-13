using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace FinalProject
{
    internal class Genre
    {
        string serverName = "DESKTOP-4FQVAJ6\\SQLEXPRESS";

        private string name;
        public Genre(string name)
        {
            this.name = name;
        }
        public void Add()
        {
            SqlConnection conn = new SqlConnection("Data Source="+serverName+"; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT GenreID FROM Genres WHERE Name = @name;", conn);
            cmd.Parameters.Add(new SqlParameter("@name", name));
            int genreID = Convert.ToInt16(cmd.ExecuteScalar());
            if (genreID>0)
            {
                throw new Exception("Genre is already in Database.");
            }
            else
            {
    
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Genres(Name) VALUES (@name)", conn);
                cmd2.Parameters.Add(new SqlParameter("@name", name));
                cmd2.ExecuteNonQuery();
               

            }

            conn.Close();
        }
        public void GetGenres()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name FROM Genres",conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Console.Write(dr.GetString(0));
                Console.WriteLine();
            }
            conn.Close();
        }
        public void Update(string newName)
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Genres SET Name = '"+newName+"' WHERE Name= @name;", conn);
            cmd.Parameters.Add(new SqlParameter("@name", this.name));
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Delete()
        {
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + "; Initial Catalog=FinalProject;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE Genres WHERE Name = @name", conn);
            cmd.Parameters.Add(new SqlParameter("@name", this.name));
            cmd.ExecuteNonQuery();
            conn.Close();
        }
       
    }
}
