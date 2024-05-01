using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class Person
    {
        
        private string ip;
        private string name;
        public List<string> LastMessages { get; set; }
        public bool IsOnline { get; set; }
        public DateTime receiveDate { get; set; }
        public int UnreadMessage { get; set; }




        public Person(string _ip,string _name)
        {
            ip = _ip;
            name = _name;

        }

        public string Ip
        {
            get { return ip; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        public override string ToString()
        {
            return name;
        }


    }
}
