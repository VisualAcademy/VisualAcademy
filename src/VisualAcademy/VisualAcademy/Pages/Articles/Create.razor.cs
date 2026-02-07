using Azunt.ArticleManagement;
using Microsoft.AspNetCore.Components;

namespace ArticleApp.Pages.Articles
{
    public partial class Create
    {
        [Inject]
        public NavigationManager Nav { get; set; } = default!;

        [Inject]
        public IArticleRepository ArticleRepository { get; set; } = default!;

        public Article Model { get; set; } = new Article();

        protected async Task btnSubmit_Click()
        {
            // 저장 로직 
            await ArticleRepository.AddArticleAsync(Model);

            // 리스트 페이지로 이동
            Nav.NavigateTo("/Articles");
        }
    }
}
