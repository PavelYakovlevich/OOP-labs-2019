using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OOP_lab7
{
    [Serializable]
    public class User
    {
        public User()
        {
            Skills = new List<string>();
        }
        [XmlElement("uid")]
        public string UID;
        [XmlElement("name")]
        public string Name;
        [XmlElement("email")]
        public string Email;
        [XmlElement("city")]
        public string City;
        [XmlElement("address")]
        public string Address;
        [XmlElement("phone")]
        public string Phone;
        [XmlElement("gender")]
        public string Gender;
        [XmlArray("skills")]
        [XmlArrayItem(typeof(string), ElementName = "skill")]
        public List<string> Skills;
        private static Dictionary<string, Action<User>> assignments;


        public static List<User> GetAllUsers(string XMLFile, string templateString, uint minSkills, Func<User, string, uint, bool> callbackFunc)
        {
            List<User> result = new List<User>();
            List<User> allUsersList = new List<User>();
            string fileContent = "";

            Dictionary<string, Action<User>> assignments = new Dictionary<string, Action<User>>();
            

            using (FileStream fileStream = new FileStream(XMLFile, FileMode.Open))
            {
                
                XmlReader xmlReader = XmlReader.Create(fileStream);
                User tempUser = new User();

                assignments.Add("uid", (User x) => { x.UID = xmlReader.ReadElementContentAsString(); });
                assignments.Add("name", (User x) => { x.Name = xmlReader.ReadElementContentAsString(); });
                assignments.Add("email", (User x) => { x.Email = xmlReader.ReadElementContentAsString(); });
                assignments.Add("city", (User x) => { x.City = xmlReader.ReadElementContentAsString(); });
                assignments.Add("address", (User x) => { x.Address = xmlReader.ReadElementContentAsString(); });
                assignments.Add("phone", (User x) => { x.Phone = xmlReader.ReadElementContentAsString(); });
                assignments.Add("gender", (User x) => { x.Gender = xmlReader.ReadElementContentAsString(); });
                assignments.Add("skill", (User x) => { x.Skills.Add(xmlReader.ReadElementContentAsString()); });

                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement() && xmlReader.NodeType == XmlNodeType.Element && assignments.ContainsKey(xmlReader.Name))
                    {
                        assignments[xmlReader.Name](tempUser);
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "user")
                    { 
                        if (callbackFunc(tempUser, templateString, minSkills))
                            result.Add(tempUser);

                        tempUser = new User();
                    }
                }
            }


            return result;
        }
        
        
    }
}
