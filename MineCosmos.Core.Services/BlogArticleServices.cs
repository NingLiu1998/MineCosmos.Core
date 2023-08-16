using AutoMapper;
using MineCosmos.Core.Common;
using MineCosmos.Core.IRepository.Base;
using MineCosmos.Core.IServices;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels;
using MineCosmos.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineCosmos.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IMapper _mapper;
        public BlogArticleServices(IMapper mapper)
        {
            this._mapper = mapper;
        }
        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModels> GetBlogDetails(int id)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var bloglist = await base.Query(a => a.IsDeleted==false, a => a.bID);
            var blogArticle = (await base.Query(a => a.Id == id && a.Category == "技术博文")).FirstOrDefault();

            BlogViewModels models = null;

            if (blogArticle != null)
            {
                models = _mapper.Map<BlogViewModels>(blogArticle);

                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextBlogs = await base.Query(a => a.Id >= id && a.IsDeleted == false && a.Category == "技术博文", 2, "bID");
                if (nextBlogs.Count == 2)
                {
                    models.next = nextBlogs[1].Title;
                    models.nextID = nextBlogs[1].Id;
                }
                var prevBlogs = await base.Query(a => a.Id <= id && a.IsDeleted == false && a.Category == "技术博文", 2, "bID desc");
                if (prevBlogs.Count == 2)
                {
                    models.previous = prevBlogs[1].Title;
                    models.previousID = prevBlogs[1].Id;
                }

                blogArticle.Traffic += 1;
                await base.Update(blogArticle, new List<string> { "btraffic" });
            }

            return models;

        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            var bloglist = await base.Query(a => a.Id > 0, a => a.Id);

            return bloglist;

        }
    }
}
