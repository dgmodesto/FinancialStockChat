using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Domain.Models
{
    public class Message
    {
        public Message(string content, string userNameSender, string userNameReceive)
        {
            Content = content;
            UserNameSender = userNameSender;
            UserNameReceive = userNameReceive;
        }

        public string UserNameSender { get; set; }
        public string UserNameReceive { get; set; }
        public string Content { get; set; }
    }
}
