using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Services;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string username;
            string password;
            int action;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Hello. Would you like to login as \n1. A Member or \n2. An Employee?");
                int userType = Convert.ToInt16(Console.ReadLine());
                if(userType != 1 && userType!=2)
                {
                    Console.WriteLine("Invalid input. You must login in as either a User or an Employee. ");
                    continue;
                }
                else
                {
                   

                    switch (userType)
                    {
                        case 1://Member
                            while(true)
                            {
                                Console.WriteLine("Login\nUsername:\t");
                                username = Console.ReadLine();
                                Console.WriteLine("Password:\t");
                                password = Console.ReadLine();
                                Member member = new Member();
                                member.SetByUserNamePassword(username, password);
                                if (member.GetName() == null)
                                {
                                    Console.WriteLine("Invalid Username or Password.");
                                }
                                else
                                {
                                    Console.WriteLine("Hi " + member.GetName() + "!");

                                    while (true)
                                    {
                                        Console.WriteLine("Are you here for\n1. Account assistance or\n2. Order Assistance");
                                        int assistanceType = Convert.ToInt16(Console.ReadLine());
                                        switch (assistanceType)
                                        {
                                            case 1://Account Assistance
                                                Console.WriteLine("Of course! What would you like to update?\n1. Password\n2. Credit Card Information\n3. Name on file\n4. Telephone number on file");
                                                action =Convert.ToInt32(Console.ReadLine());
                                                switch (action)
                                                {
                                                    case 1://Update Password
                                                        while(true)
                                                        {
                                                            Console.WriteLine("Please enter your current password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (member.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter new password: ");
                                                                string newPswd = Console.ReadLine();
                                                                member.UpdatePassword(newPswd);
                                                                Console.WriteLine("Your password has been updated successfully.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                    case 2: //Update Credit Card
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter your current password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (member.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter new credit card number: ");
                                                                long newNum = Convert.ToInt64(Console.ReadLine());
                                                                member.Update<long>("CreditCardInfo", newNum);
                                                                Console.WriteLine("Your credit card number has been successfully updated.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                    case 3: //Update Name
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter your current password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (member.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter your new full name");
                                                                string newName =Console.ReadLine();
                                                                member.Update<string>("Name", newName);
                                                                Console.WriteLine("Your name has been successfully updated.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                    case 4: //Update Telephone Number
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter your current password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (member.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter your new number:");
                                                                string newName = Console.ReadLine();
                                                                member.Update<string>("PhoneNumber", newName);
                                                                Console.WriteLine("Your telephone number has been successfully updated.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                }
                                                break;

                                            case 2://Order Assistance
                                                Console.WriteLine("Of course! How can we help you with your order today?\n1. Return a book\n2. Rate a book\n3. Get information on previous orders");
                                                action = Convert.ToInt32(Console.ReadLine());
                                                switch (action)
                                                {
                                                    case 1: //Return a book
                                                        while (true)
                                                        {
                                                            Orders order = new Orders();
                                                            Console.Write("Please enter the orderID this book is part of: ");
                                                            int orderID = Convert.ToInt32(Console.ReadLine());  
                                                            order.SetWithOrderID(orderID);
                                                            Console.WriteLine("Please enter the book ID you wish to return:"); 
                                                            int bookID = Convert.ToInt32(Console.ReadLine());
                                                            Console.WriteLine("Please choose the condition you're returning the book with:\n1. Excellent\n2. Good\n3. Bad");
                                                            int conditionNum = Convert.ToInt32(Console.ReadLine());
                                                            
                                                            switch(conditionNum) { case 1: order.ReturnBook(bookID, "excellent"); Console.WriteLine("Your book has been successfully returned.") ; break; case 2: order.ReturnBook(bookID, "good"); Console.WriteLine("Your book has been successfully returned."); break; case 3: order.ReturnBook(bookID, "bad") ; Console.WriteLine("Your book has been successfully returned."); break; default: Console.WriteLine("Invalid selection. Let's try again.");break; }

                                                            break;
                                                        }
                                                        break;
                                                    case 2: //Rate a book
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter the orderID the book you wish is part of: ");
                                                            int orderID = Convert.ToInt32(Console.ReadLine());
                                                            Rating rating = new Rating();
                                                            rating.SetBookID(orderID);
                                                            Console.WriteLine("On a scale of 1-5, how would you rate this book? ");
                                                            int ratingNum = Convert.ToInt32(Console.ReadLine());
                                                            while(ratingNum<1 || ratingNum > 5)
                                                            {
                                                                Console.WriteLine("You have entered an invalid rating. Rating must be between 1 and 5.\nOn a scale of 1-5, how would you rate this book? ");
                                                                ratingNum = Convert.ToInt32(Console.ReadLine());
                                                            }
                                                            Console.WriteLine("We would appreciate if you can share your reading experience in a few words: ");
                                                            string comments = Console.ReadLine();
                                                            rating.SetRatingAndComment(ratingNum, comments);
                                                            rating.Add();
                                                            Console.WriteLine("Thank you for submitting. Your feedback is appreciated.");
                                                            break;
                                                        }
                                                        break;
                                                    case 3://Get info on previous order
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Most definitely. Please specify which report you need:\n1. See all the books in a specific order\n2. See all my previous orders\n3. See non-returned orders");
                                                            int infoType = Convert.ToInt32(Console.ReadLine());
                                                            switch (infoType)
                                                            {
                                                                case 1://Get all books in order
                                                                    Console.WriteLine("Enter the OrderID you are inquiring about: ");
                                                                    int orderID = Convert.ToInt32(Console.ReadLine());
                                                                    Orders order = new Orders();
                                                                    order.GetBooksInOrder(orderID);
                                                                    break;
                                                                case 2://Get all orders
                                                                    Orders order2 = new Orders();
                                                                    order2.GetMemberOrders(username);
                                                                    break;
                                                                case 3://Get non-returned orders
                                                                    Orders order3 = new Orders();
                                                                    order3.GetNonReturnedOrders(username);
                                                                    break;

                                                            }
                                                        }
                                                }
                                                break;

                                            default:
                                                Console.WriteLine("Invalid Input. You must select a valid assistance type. ");
                                                break;
                                        }
                                    }


                                }
                            }
                            
                           
                        case 2://Librarian
                            while (true)
                            {
                                Console.WriteLine("Login\nUsername:\t");
                                username = Console.ReadLine();
                                Console.WriteLine("Password:\t");
                                password = Console.ReadLine();
                                Librarians librarian = new Librarians();
                                librarian.SetByUserNamePassword(username, password);
                                if (librarian.GetName() == null)
                                {
                                    Console.WriteLine("Invalid Username or Password.");
                                }
                                else
                                {
                                    Console.WriteLine("Hi " + librarian.GetName() + "!");
                                    while (true)
                                    {
                                        Console.WriteLine("Are you here for\n1. Employee assistance or\n2. Member Assistance");
                                        int assistanceType = Convert.ToInt16(Console.ReadLine());
                                        switch (assistanceType)
                                        {
                                            case 1://Employee Assistance
                                                Console.WriteLine("Of course! What would you like to update?\n1. Employee Password\n2. Salary\n3. Employee Name\n4. Employee Telephone number");
                                                action = Convert.ToInt32(Console.ReadLine());
                                                switch(action)
                                                {
                                                    case 1://Update Password
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter your current password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (librarian.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter new password: ");
                                                                string newPswd = Console.ReadLine();
                                                                librarian.UpdatePassword(newPswd);
                                                                Console.WriteLine("Your password has been updated successfully.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                    case 2://Update salary
                                                            decimal hrlyWage = librarian.GetSalary();
                                                            Console.WriteLine("You're currently being paid $" + hrlyWage + " per hour. If you wish to request a raise, please speak to Human Resources.");
                                                        break;
                                                    case 3://Update name
                                                        while (true)
                                                        {
                                                            Console.WriteLine("For user verification, please enter your password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (librarian.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter your new name: ");
                                                                string newName = Console.ReadLine();
                                                                librarian.Update("Name", newName);
                                                                Console.WriteLine("Your name has been updated successfully.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;
                                                    case 4://Update Phone#
                                                        while (true)
                                                        {
                                                            Console.WriteLine("For user verification, please enter your password: ");
                                                            string currentPswd = Console.ReadLine();
                                                            if (librarian.ConfirmPswd(currentPswd))
                                                            {
                                                                Console.WriteLine("Enter your new number: ");
                                                                string newNumber = Console.ReadLine();
                                                                librarian.Update("PhoneNumber", newNumber);
                                                                Console.WriteLine("Your Phone Number has been updated successfully.");
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("The password you have entered is incorrect.");
                                                            }
                                                        }
                                                        break;

                                                }
                                                break;
                                            case 2://Member Assistance
                                                Console.WriteLine("Of course! How can we help you today?\n1. Return a member's book\n2. Enter a member's Rating\n3. Enter a new member order\n4. Get information on members' orders\n5. Add a member");
                                                int asstType = Convert.ToInt32(Console.ReadLine());
                                                switch (asstType)
                                                {
                                                    case 1://Return book
                                                        while (true)
                                                        {
                                                            Orders order = new Orders();
                                                            Console.Write("Please enter the orderID this book is part of: ");
                                                            int orderID = Convert.ToInt32(Console.ReadLine());
                                                            order.SetWithOrderID(orderID);
                                                            Console.WriteLine("Please enter the book ID being returned:");
                                                            int bookID = Convert.ToInt32(Console.ReadLine());
                                                            Console.WriteLine("Please choose the condition member returned this book in:\n1. Excellent\n2. Good\n3. Bad");
                                                            int conditionNum = Convert.ToInt32(Console.ReadLine());

                                                            switch (conditionNum) { case 1: order.ReturnBook(bookID, "excellent"); Console.WriteLine("The book has been successfully returned."); break; case 2: order.ReturnBook(bookID, "good"); Console.WriteLine("Your book has been successfully returned."); break; case 3: order.ReturnBook(bookID, "bad"); Console.WriteLine("The book has been successfully returned."); break; default: Console.WriteLine("Invalid selection. Let's try again."); break; }

                                                            break;
                                                        }
                                                        break;
                                                    case 2://Enter Rating
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please enter the orderID the book is part of: ");
                                                            int orderID = Convert.ToInt32(Console.ReadLine());
                                                            Rating rating = new Rating();
                                                            rating.SetBookID(orderID);
                                                            Console.WriteLine("On a scale of 1-5, how would member rate this book? ");
                                                            int ratingNum = Convert.ToInt32(Console.ReadLine());
                                                            while (ratingNum < 1 || ratingNum > 5)
                                                            {
                                                                Console.WriteLine("Invalid rating has been entered. Rating must be between 1 and 5.\nOn a scale of 1-5, how would member rate this book? ");
                                                                ratingNum = Convert.ToInt32(Console.ReadLine());
                                                            }
                                                            Console.WriteLine("Please enter member's comments on this book: ");
                                                            string comments = Console.ReadLine();
                                                            rating.SetRatingAndComment(ratingNum, comments);
                                                            rating.Add();
                                                            Console.WriteLine("The rating has been submitted successfully.");
                                                            break;
                                                        }
                                                        break;
                                                    case 3://New order
                                                        while (true)
                                                        {
                                                            Console.WriteLine("What is the member's username? ");
                                                            string membersUserName = Console.ReadLine();
                                                            Orders order = new Orders(membersUserName, username);
                                                            Console.WriteLine("How many books does " + membersUserName + " wish to take out?");
                                                            int bookAmt = Convert.ToInt32(Console.ReadLine());
                                                            if (bookAmt > 3)
                                                            {
                                                                Console.WriteLine("Member has a limit of 3 books per order.");
                                                            }
                                                            else
                                                            {
                                                                int[] bookIDArr = new int[bookAmt];

                                                                for (int i = 0; i < bookAmt; i++)
                                                                {
                                                                    Console.WriteLine("Please enter the bookID member will be taking out: ");
                                                                    bookIDArr[i] = Convert.ToInt32(Console.ReadLine());
                                                                }

                                                                foreach(int book in bookIDArr)
                                                                {
                                                                    Console.Write(book + ",");
                                                                }

                                                                order.Add(bookIDArr);
                                                                Console.WriteLine("Thank you for completing the order. The member has two weeks to return borrowed books.");

                                                            }


                                                        }
                                                        break;

                                                    case 4://Get info on previous order
                                                        while (true)
                                                        {
                                                            Console.WriteLine("Please specify which report you need:\n1. Generate a report on a specific member\n2. Generate a general report\n3. Complete a search");
                                                            int infoType = Convert.ToInt32(Console.ReadLine());
                                                            switch (infoType)
                                                            {
                                                                case 1:
                                                                    Console.WriteLine("Please enter which information you'd like to get:\n1. Get information on an order\n2. Get all orders\n3. Get non-returned orders");
                                                                    int reportType = Convert.ToInt32(Console.ReadLine());
                                                                    switch (reportType)
                                                                    {
                                                                        case 1://Get all books in order
                                                                            Console.WriteLine("Enter the OrderID you are inquiring about: ");
                                                                            int orderID = Convert.ToInt32(Console.ReadLine());
                                                                            Orders order = new Orders();
                                                                            order.GetBooksInOrder(orderID);
                                                                            break;
                                                                        case 2://Get all orders
                                                                            Console.WriteLine("Please enter the member's username: ");
                                                                            string membersUsername = Console.ReadLine();
                                                                            Orders order2 = new Orders();
                                                                            order2.GetMemberOrders(membersUsername);
                                                                            break;
                                                                        case 3://Get non-returned orders
                                                                            Console.WriteLine("Please enter the member's username: ");
                                                                            string membersUsername1 = Console.ReadLine();
                                                                            Orders order3 = new Orders();
                                                                            order3.GetNonReturnedOrders(membersUsername1);
                                                                            break;
                                                                    }
                                                                    break;
                                                                case 2:
                                                                    Console.WriteLine("Please enter which information you'd like to get:\n1. Get all orders\n2.  Get all members\n3. Get all books\n4. Get all authors");
                                                                    int infotype = Convert.ToInt32(Console.ReadLine());
                                                                    switch (infotype)
                                                                    {
                                                                       

                                                                    }
                                                                    break;
                                                                case 3:
                                                                    Console.WriteLine("Please enter the category you'd like to search:\n1. Search for a member by phone number\n2. Search for a member by name\n3. Search for books by title\n4. Search for books by authors\n5. Search for books by genre");
                                                                    int type = Convert.ToInt32(Console.ReadLine());
                                                                    break;

                                                            }
                                                        }
                                                    case 5://Add member
                                                        while (true)
                                                        {
                                                            //Get member info
                                                            Console.WriteLine("Please enter the member's personal information.\nName: ");
                                                            string name = Console.ReadLine();
                                                            Console.WriteLine("Age: ");
                                                            int age = Convert.ToInt32(Console.ReadLine());
                                                            Console.WriteLine("Telephone Number: ");
                                                            string phoneNumber = Console.ReadLine();
                                                            Console.WriteLine("Credit Card Information");
                                                            long cc = Convert.ToInt64(Console.ReadLine());
                                                            Console.WriteLine("Username: ");
                                                            string membersUserName = Console.ReadLine();
                                                            Console.WriteLine("Password: ");
                                                            string membersPswd = Console.ReadLine();
                                                            //Add
                                                            Member member = new Member(name, age, phoneNumber, cc, membersUserName, membersPswd,null);
                                                            member.Add();
                                                            Console.WriteLine("Thank you for the information, "+name+" was successfully added.");
                                                        }

                                                }
                                                break;
                                            default:
                                                Console.WriteLine("Invalid Input. You must select a valid assistance type. ");
                                                break;
                                        }
                                    }

                                }
                            }
                           
                      


                    }
                }
               

                Console.ReadKey();

            }




        }
    }
}
