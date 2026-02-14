using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAcademy.Models.TextTemplates;

namespace VisualAcademy.Pages.TextTemplates;

public partial class Manage : ComponentBase
{
    #region Parameters
    [Parameter] public int ParentId { get; set; } = 0;

    [Parameter] public string ParentKey { get; set; } = "";
    #endregion

    #region Injectors
    [Inject] public NavigationManager? Nav { get; set; }

    [Inject] public IJSRuntime? JSRuntimeInjector { get; set; }

    [Inject] public ITextTemplateRepository? RepositoryReference { get; set; }
    #endregion

    #region Properties
    /// <summary>
    /// 글쓰기 또는 수정하기 폼의 제목에 전달할 문자열(태그 포함 가능)
    /// </summary>
    public string EditorFormTitle { get; set; } = "CREATE";
    #endregion

    /// <summary>
    /// EditorForm에 대한 참조: 모달로 글쓰기 또는 수정하기
    /// </summary>
    public Components.ModalForm? EditorFormReference { get; set; }

    /// <summary>
    /// DeleteDialog에 대한 참조: 모달로 항목 삭제하기 
    /// </summary>
    public Components.DeleteDialog? DeleteDialogReference { get; set; }

    /// <summary>
    /// 현재 페이지에서 리스트로 사용되는 모델 리스트 
    /// </summary>
    protected List<TextTemplateModel> models = new();

    /// <summary>
    /// 현재 페이지에서 선택된 단일 데이터를 나타내는 모델 클래스 
    /// </summary>
    protected TextTemplateModel model = new();

    /// <summary>
    /// 페이저 설정
    /// </summary>
    protected DulPager.DulPagerBase pager = new()
    {
        PageNumber = 1,
        PageIndex = 0,
        PageSize = 10,
        PagerButtonCount = 5
    };

    #region Lifecycle Methods
    /// <summary>
    /// 페이지 초기화 이벤트 처리기
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId) && string.IsNullOrWhiteSpace(UserName))
        {
            await GetUserIdAndUserName();
        }

        await DisplayData();
    }
    #endregion

    private async Task DisplayData()
    {
        // DI 안전성 보장
        if (RepositoryReference is null)
            throw new InvalidOperationException($"{nameof(RepositoryReference)} was not injected.");

        // ParentKey와 ParentId를 사용하는 목적은 특정 부모의 Details 페이지에서 리스트로 표현하기 위함
        if (!string.IsNullOrWhiteSpace(ParentKey))
        {
            var articleSet = await RepositoryReference.GetArticlesAsync<string>(
                pager.PageIndex, pager.PageSize, "", searchQuery, sortOrder, ParentKey);

            pager.RecordCount = articleSet.TotalCount;
            models = articleSet.Items.ToList();
        }
        else if (ParentId != 0)
        {
            var articleSet = await RepositoryReference.GetArticlesAsync<int>(
                pager.PageIndex, pager.PageSize, "", searchQuery, sortOrder, ParentId);

            pager.RecordCount = articleSet.TotalCount;
            models = articleSet.Items.ToList();
        }
        else
        {
            var articleSet = await RepositoryReference.GetArticlesAsync<int>(
                pager.PageIndex, pager.PageSize, searchField: "", searchQuery, sortOrder, parentIdentifier: 0);

            pager.RecordCount = articleSet.TotalCount;
            models = articleSet.Items.ToList();
        }

        // Refresh
        StateHasChanged();
    }

    protected async Task PageIndexChanged(int pageIndex)
    {
        pager.PageIndex = pageIndex;
        pager.PageNumber = pageIndex + 1;

        await DisplayData();
    }

    #region Event Handlers
    /// <summary>
    /// 글쓰기 모달 폼 띄우기 
    /// </summary>
    protected void ShowEditorForm()
    {
        if (EditorFormReference is null)
            throw new InvalidOperationException($"{nameof(EditorFormReference)} was not initialized.");

        EditorFormTitle = "CREATE";
        model = new TextTemplateModel(); // 모델 초기화
        EditorFormReference.Show();
    }

    /// <summary>
    /// 관리자 전용: 모달 폼으로 선택 항목 수정
    /// </summary>
    protected void EditBy(TextTemplateModel model)
    {
        if (EditorFormReference is null)
            throw new InvalidOperationException($"{nameof(EditorFormReference)} was not initialized.");

        EditorFormTitle = "EDIT";
        this.model = model; // 선택 모델로 교체
        EditorFormReference.Show();
    }

    /// <summary>
    /// 관리자 전용: 모달 폼으로 선택 항목 삭제
    /// </summary>
    protected void DeleteBy(TextTemplateModel model)
    {
        if (DeleteDialogReference is null)
            throw new InvalidOperationException($"{nameof(DeleteDialogReference)} was not initialized.");

        this.model = model;
        DeleteDialogReference.Show();
    }
    #endregion

    /// <summary>
    /// 모델 초기화 및 모달 폼 닫기
    /// </summary>
    protected async Task CreateOrEdit()
    {
        if (EditorFormReference is null)
            throw new InvalidOperationException($"{nameof(EditorFormReference)} was not initialized.");

        EditorFormReference.Hide();

        model = new TextTemplateModel();
        await DisplayData();
    }

    /// <summary>
    /// 삭제 모달 폼에서 현재 선택한 항목 삭제
    /// </summary>
    protected async Task DeleteClick()
    {
        if (RepositoryReference is null)
            throw new InvalidOperationException($"{nameof(RepositoryReference)} was not injected.");

        if (DeleteDialogReference is null)
            throw new InvalidOperationException($"{nameof(DeleteDialogReference)} was not initialized.");

        await RepositoryReference.DeleteAsync(model.Id);
        DeleteDialogReference.Hide();

        model = new TextTemplateModel(); // 선택했던 모델 초기화
        await DisplayData(); // 다시 로드
    }

    #region Toggle with Inline Dialog
    /// <summary>
    /// 인라인 폼을 띄울건지 여부 
    /// </summary>
    public bool IsInlineDialogShow { get; set; } = false;

    protected void ToggleClose()
    {
        IsInlineDialogShow = false;
        model = new TextTemplateModel();
    }

    /// <summary>
    /// 토글: Pinned
    /// </summary>
    protected async Task ToggleClick()
    {
        if (RepositoryReference is null)
            throw new InvalidOperationException($"{nameof(RepositoryReference)} was not injected.");

        // 변경된 내용 업데이트
        await RepositoryReference.UpdateAsync(model);

        IsInlineDialogShow = false;            // 표시 속성 초기화
        model = new TextTemplateModel();       // 선택한 모델 초기화

        await DisplayData();                   // 다시 로드
    }

    /// <summary>
    /// ToggleBy(PinnedBy)
    /// </summary>
    protected void ToggleBy(TextTemplateModel model)
    {
        this.model = model;
        IsInlineDialogShow = true;
    }
    #endregion

    #region Search
    private string searchQuery = "";

    protected async Task Search(string query)
    {
        pager.PageIndex = 0;
        pager.PageNumber = 1;

        searchQuery = query ?? "";
        await DisplayData();
    }
    #endregion

    #region Excel
    protected void DownloadExcelWithWebApi()
    {
        if (Nav is null)
            throw new InvalidOperationException($"{nameof(Nav)} was not injected.");

        // FileUtil.SaveAsExcel(JSRuntimeInjector, "/TextTemplateDownload/ExcelDown");
        Nav.NavigateTo($"/TextTemplates"); // 다운로드 후 현재 페이지 다시 로드
    }

    protected void DownloadExcel()
    {
        // 생략
    }
    #endregion

    #region Sorting
    private string sortOrder = "";

    protected async Task SortByName()
    {
        if (!sortOrder.Contains("Name"))
        {
            sortOrder = ""; // 다른 열을 정렬하고 있었다면, 다시 초기화
        }

        if (sortOrder == "")
        {
            sortOrder = "Name";
        }
        else if (sortOrder == "Name")
        {
            sortOrder = "NameDesc";
        }
        else
        {
            sortOrder = "";
        }

        await DisplayData();
    }
    #endregion

    #region Get UserId and UserName
    [Parameter] public string UserId { get; set; } = "";

    [Parameter] public string UserName { get; set; } = "";

    [Inject] public UserManager<VisualAcademy.Data.ApplicationUser>? UserManagerRef { get; set; }

    [Inject] public AuthenticationStateProvider? AuthenticationStateProviderRef { get; set; }

    private async Task GetUserIdAndUserName()
    {
        if (AuthenticationStateProviderRef is null)
            throw new InvalidOperationException($"{nameof(AuthenticationStateProviderRef)} was not injected.");

        if (UserManagerRef is null)
            throw new InvalidOperationException($"{nameof(UserManagerRef)} was not injected.");

        // 현재 인증 상태(AuthenticationState) 가져오기
        var authState = await AuthenticationStateProviderRef.GetAuthenticationStateAsync();

        // ClaimsPrincipal (현재 사용자)
        var user = authState.User;

        // user.Identity는 null일 수 있으므로 ?. 사용
        // IsAuthenticated == true 인 경우만 로그인된 사용자로 처리
        if (user.Identity?.IsAuthenticated == true)
        {
            // ClaimsPrincipal을 기반으로 실제 ApplicationUser 조회
            // (외부 로그인, 쿠키 만료 등으로 null이 될 가능성 있음)
            var currentUser = await UserManagerRef.GetUserAsync(user);

            // currentUser가 null일 수 있으므로 null 병합 연산자 사용
            UserId = currentUser?.Id ?? "";

            // 1) Claims의 Name
            // 2) ApplicationUser의 UserName
            // 3) 최종 fallback: "Anonymous"
            UserName = user.Identity?.Name
                       ?? currentUser?.UserName
                       ?? "Anonymous";
        }
        else
        {
            // 비인증 사용자(로그인 안 됨)
            UserId = "";
            UserName = "Anonymous";
        }
    }
    #endregion
}
