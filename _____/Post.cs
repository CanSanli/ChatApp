using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    [Serializable]
    public class Post
    {
        public int MessageType { get; set; }
        public string Message { get; set; }
        public bool IsOnline { get; set; }

    }
}
