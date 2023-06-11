using ArticleApp.Models;
using Dul.Domain.Common;
using DulPager;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ArticleApp.Pages.Articles;

public partial class Manage
{
    [Inject]
    public IArticleRepository ArticleRepository { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    private void btnCreate_Click()
    {
        editorFormTitle = "글쓰기";
        _article = new Article(); // 모델 클리어
    }

    private string editorFormTitle = "";

    private void EditBy(Article article)
    {
        editorFormTitle = "수정하기";
        _article = article;
    }

    /// <summary>
    /// 저장 또는 수정 후 데이터 다시 읽어오기
    /// </summary>
    private async void SaveOrUpdated()
    {
        var pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
        pager.RecordCount = pagingData.TotalRecords;
        articles = pagingData.Records.ToList();

        StateHasChanged();
    }

    /// <summary>
    /// 삭제 -> 모달 폼 닫기 -> 선택했던 데이터 초기화 -> 전체 데이터 다시 읽어오기 -> Refresh
    /// </summary>
    private async void btnDelete_Click()
    {
        // BS 5 모달
        await ArticleRepository.DeleteArticleAsync(_article.Id); // 삭제 
        await JSRuntime.InvokeAsync<object>("hideModal", "articleDeleteDialog"); // _Host.cshtml
        _article = new Article(); // 선택 항목 초기화 

        var pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
        pager.RecordCount = pagingData.TotalRecords;
        articles = pagingData.Records.ToList();

        StateHasChanged();
    }

    private void DeleteBy(Article article) => _article = article;

    /* Notice Modal */
    private Article _article = new Article(); // 선택한 항목 관리

    private bool isShow = false; // Notice Modal

    private void PinnedBy(Article article)
    {
        _article = article;
        isShow = true; // 창 열기 
        //JSRuntime.InvokeAsync<object>("alert", $"{article.Id}를 공지글로 설정합니다.");
    }

    private void btnClose_Click()
    {
        isShow = false; // 창 닫기 
        _article = new Article(); // 선택했던 모델 초기화
    }

    private async void btnTogglePinned_Click()
    {
        _article.IsPinned = !_article.IsPinned; // Toggle
        await ArticleRepository.EditArticleAsync(_article);

        PagingResult<Article> pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
        pager.RecordCount = pagingData.TotalRecords; // 총 레코드 수
        articles = pagingData.Records.ToList(); // 페이징 처리된 레코드

        isShow = false; // Modal Close
        StateHasChanged(); // Refresh
    }
    /* Notice Modal */

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

        var pagingData = await ArticleRepository.GetAllAsync(pager.PageIndex, pager.PageSize);
        pager.RecordCount = pagingData.TotalRecords; // 총 레코드 수
        articles = pagingData.Records.ToList(); // 페이징 처리된 레코드

        StateHasChanged(); // 현재 컴포넌트 재로드(Pager Refresh)
    }
}
