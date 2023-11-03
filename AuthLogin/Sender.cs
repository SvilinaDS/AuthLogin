using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLogin
{   
    interface ISender
    {
         void Send(string message);//public
        
    }
    internal class Sender
    {
        public void Send(string message)
        {
            Console.WriteLine($"Fake sms-send: {message}");
        }
    }
}
