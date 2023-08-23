using System;

namespace MineCosmos.Core.Common
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class CustomException: Exception
    {
        public CustomException(string message) : base(message) { }

        public override string Message
        {
            get { return "业务异常: " + base.Message; }
        }
    }
}
