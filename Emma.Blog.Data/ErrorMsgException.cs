using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Data
{
    public class ErrorMsgException:Exception
    {
        public int Status = 1;

        public ErrorMsgException(string msg):base(msg)
        {
            
        }
    }
}
