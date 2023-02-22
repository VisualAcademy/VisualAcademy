using Dul.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// Repository Interface
    /// </summary>
    public interface IArticleRepository
    {
        Task<Article> AddArticleAsync(Article model);                           // 입력
        Task<List<Article>> GetArticlesAsync();                                 // 출력
        Task<Article> GetArticleByIdAsync(int id);                              // 상세
        Task<Article> EditArticleAsync(Article model);                          // 수정
        Task DeleteArticleAsync(int id);                                        // 삭제
        Task<PagingResult<Article>> GetAllAsync(int pageIndex, int pageSize);   // 페이징
    }
}
