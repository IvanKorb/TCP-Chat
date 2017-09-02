using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DataModel
{
    public class Message
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserMessage { get; set; }

        public DateTime DateTimeMessage { get; set; }

    }
}
