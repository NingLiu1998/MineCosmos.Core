using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCosmos.Core.Common
{
    public class CustomException: Exception
    {
        public CustomException(string message) : base(message) { }

        public override string Message
        {
            get { return "业务异常: " + base.Message; }
        }
    }
}
