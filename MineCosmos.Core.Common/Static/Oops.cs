using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineCosmos.Core.Common.Static
{
    public class Oops
    {
        /// <summary>
        /// 抛出业务异常信息
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="args">String.Format 参数</param>
        /// <returns>异常实例</returns>
        public static CustomException Bah(string errorMsg)
        {
            return new CustomException(errorMsg);
        }
    }
}
