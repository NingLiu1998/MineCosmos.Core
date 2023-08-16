using SqlSugar;

namespace MineCosmos.Core.Model.Models
{
    public class Advertisement : RootEntityTkey<int>
    {
        /// <summary>
        /// 广告图片
        /// </summary>
        [SugarColumn(Length = 512, IsNullable = true)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 广告标题
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string Title { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true)]
        public string Url { get; set; }
    }
}
