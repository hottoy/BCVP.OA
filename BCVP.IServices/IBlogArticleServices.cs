using BCVP.IServices.BASE;
using BCVP.Model.Models;
using BCVP.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCVP.IServices
{
    public interface IBlogArticleServices :IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);

    }

}
