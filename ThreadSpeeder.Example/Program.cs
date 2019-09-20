using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ThreadSpeeder.Example.Application;
using ThreadSpeeder.Example.Business;
using ThreadSpeeder.Example.DTO;

namespace ThreadSpeeder.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Make objects for testing, don't focus on that :)
            var timeToMakeObjectsBefore = DateTime.Now;
            List<User> users = new List<User>();
            for(int i = 0; i < 1000; i++)
            {
                users.Add(new User()
                {
                    Address = new Address() {
                        Street = "Fake address " + i,
                    },
                    Age = (byte)(i % 39),
                    Email = "fakeEmail" + i + " @email.com",
                    IdUser = i,
                    Name = "Fake name " + i,
                    Salary = i
                });
            }
            var timeToMakeObjectsAfter = DateTime.Now;
            var secondsToMakeObjects = timeToMakeObjectsAfter.Subtract(timeToMakeObjectsBefore);
            #endregion

            #region Thread Speeder
            var dateBeforeProcessWithThreadSpeeder = DateTime.Now;
            new UserApplication().ProcessWithThreadSpeeder(users);
            var dateAfterProcessWithThreadSpeeder = DateTime.Now;
            var secondsWithThreadSpeeder = dateAfterProcessWithThreadSpeeder.Subtract(dateBeforeProcessWithThreadSpeeder);
            #endregion
            
            #region Without Thread Speeder
            var dateBeforeProcessWithoutThreadSpeeder = DateTime.Now;
            new UserApplication().ProcessWithoutThreadSpeeder(users);
            var dateAfterProcessWithoutThreadSpeeder = DateTime.Now;
            var secondsWithoutThreadSpeeder = dateAfterProcessWithoutThreadSpeeder.Subtract(dateBeforeProcessWithoutThreadSpeeder);
            #endregion
            

            Console.WriteLine("it took to create the objets(that does'nt mean nothing to us, don't worry: " + secondsToMakeObjects.ToString());
            Console.WriteLine("With ThreadSpeeder.Core it took: " + secondsWithThreadSpeeder.ToString());
            Console.WriteLine("Without ThreadSpeeder.Core it took: " + secondsWithoutThreadSpeeder.ToString());

            Console.ReadLine();



        }


    }
}
