using SqlSugar;
using System;
using System.Collections.Generic;

namespace MineCosmos.Core.Model
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class SysUserInfoRoot<Tkey>: RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {

        [SugarColumn(IsIgnore = true)]
        public List<Tkey> RIDs { get; set; }

    }
}
