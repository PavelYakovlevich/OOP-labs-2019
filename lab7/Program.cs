using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OOP_lab7
{
    partial class Program
    {

        public static Dictionary<Gender, string> genderList = new Dictionary<Gender, string>();
        public static bool Parsing;
        public static bool callbackFunction(User user, string emailTemplate, uint minSkillsCount)
        {
            Regex regex = new Regex(emailTemplate);
            Match match = regex.Match(user.Email);

            uint userSkillsCount = (uint)user.Skills.Count;

            return match.Success && userSkillsCount >= minSkillsCount;
        }
        static void Main(string[] args)
        {
            Console.Write("Enter min skills count: ");
            bool parseResult = uint.TryParse(Console.ReadLine(), out uint skillesCount);
            if (!parseResult)
                return;

            string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            List<User> result = User.GetAllUsers(currentDirectory + "\\feed.xml", @".+@.+\.biz", skillesCount, new Func<User, string, uint, bool>(callbackFunction));
            Console.WriteLine("Result: ");
            Console.WriteLine("-----------------------------------------------------");
            PrintResult(result);
        }

        private static void PrintResult(List<User> users)
        {
            foreach (User user in users)
            {
                Console.WriteLine("{");
                Console.WriteLine(String.Format("\tUID    : {0}\n\tName   : {1}\n\tEmail  : {2}\n\tCity   : {3}\n\tAddress: {4}\n\tPhone  : {5}\n\tGender : {6}\n\tSkills :\n\t[", user.UID, user.Name, user.Email, user.City,
                    user.Address, user.Phone, user.Gender));
                foreach (string skill in user.Skills)
                {
                    Console.WriteLine("\t\t" + skill + ',');
                }

                Console.WriteLine("\t]\n}");
            }
        }
    }
}
