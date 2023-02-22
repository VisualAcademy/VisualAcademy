using ArticleApp.Models;
using Dul.Domain.Common;
using DulPager;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleApp.Pages.Articles
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IArticleRepository ArticleRepository { get; set; }

        // 페이저 기본값 설정
        private DulPagerBase pager = new DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 2,
            PagerButtonCount = 3
        };

        private List<Article> articles;

        protected override async Task OnInitializedAsync()
        {
            await DisplayData();
        }

        private async Task DisplayData()
        {
            //[1] 전체 데이터 모두 출력
            //articles = await ArticleRepository.GetArticlesAsync();

            //[2] 페이징 처리된 데이터만 출력
            PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = pagingData.TotalRecords; // 총 레코드 수
            articles = pagingData.Records.ToList(); // 페이징 처리된 레코드
        }

        // Pager 버튼 클릭 콜백 메서드
        private async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            await DisplayData(); 

            StateHasChanged(); // 현재 컴포넌트 재로드(Pager Refresh) 
        }

        /// <summary>
        /// 제목의 td 태그 영역을 클릭했을 때 해당 상세보기 페이지로 이동
        /// </summary>
        private void btnTitle_Click(int id)
        {
            NavigationManager.NavigateTo($"/Articles/Details/{id}");
        }
    }
}
