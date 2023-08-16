using MineCosmos.Core.IServices.BASE;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MineCosmos.Core.IServices
{
    public interface IBlogArticleServices :IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);

    }

}
