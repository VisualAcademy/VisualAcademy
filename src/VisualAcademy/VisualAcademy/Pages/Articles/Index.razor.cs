using Azunt.ArticleManagement;
using Dul.Domain.Common;
using DulPager;
using Microsoft.AspNetCore.Components;

namespace ArticleApp.Pages.Articles
{
    public partial class Index
    {
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public IArticleRepository? ArticleRepository { get; set; }

        // [1] 페이저 기본값 설정
        private DulPagerBase pager = new()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 2,
            PagerButtonCount = 3
        };

        // [2] 출력용 컬렉션
        private List<Article> articles = new();

        // [3] 페이지 초기화 시 데이터 로드
        protected override async Task OnInitializedAsync()
            => await DisplayData();

        private async Task DisplayData()
        {
            // DI 안전성 보장
            if (ArticleRepository is null)
                throw new InvalidOperationException($"{nameof(ArticleRepository)} was not injected.");

            // (A) 전체 데이터 출력 예제
            // articles = await ArticleRepository.GetArticlesAsync();

            // (B) 페이징 처리된 데이터만 출력
            PagingResult<Article> pagingData =
                await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);

            pager.RecordCount = pagingData.TotalRecords;   // 총 레코드 수
            articles = pagingData.Records.ToList();        // 현재 페이지 레코드
        }

        // [4] Pager 버튼 클릭 시 호출
        private async Task PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData();
        }

        /// <summary>
        /// 제목(td 영역) 클릭 시 상세보기 페이지로 이동
        /// </summary>
        private void btnTitle_Click(int id)
        {
            if (NavigationManager is null)
                throw new InvalidOperationException($"{nameof(NavigationManager)} was not injected.");

            NavigationManager.NavigateTo($"/Articles/Details/{id}");
        }
    }
}
