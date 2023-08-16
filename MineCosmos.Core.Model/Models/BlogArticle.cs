using SqlSugar;
using System;

namespace MineCosmos.Core.Model.Models
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class BlogArticle: RootEntityTkey<int>
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(Length = 600, IsNullable = true)]
        public string Submitter { get; set; }

        /// <summary>
        /// 标题blog
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true)]
        public string Title { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Category { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string Content { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Traffic { get; set; }       
    }
}
