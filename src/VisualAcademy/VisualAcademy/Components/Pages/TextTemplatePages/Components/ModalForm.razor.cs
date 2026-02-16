using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using VisualAcademy.Models.TextTemplates;

namespace VisualAcademy.Pages.TextTemplates.Components;

public partial class ModalForm : ComponentBase
{
    #region Properties
    /// <summary>
    /// (글쓰기/글수정)모달 다이얼로그를 표시할건지 여부 
    /// </summary>
    public bool IsShow { get; set; } = false;
    #endregion

    #region Public Methods
    /// <summary>
    /// 폼 보이기 
    /// </summary>
    public void Show() => IsShow = true; // 현재 인라인 모달 폼 보이기

    /// <summary>
    /// 폼 닫기
    /// </summary>
    public void Hide() => IsShow = false; // 현재 인라인 모달 폼 숨기기
    #endregion

    #region Parameters
    [Parameter] public string? UserName { get; set; }

    /// <summary>
    /// 폼의 제목 영역
    /// </summary>
    [Parameter] public RenderFragment? EditorFormTitle { get; set; }

    /// <summary>
    /// 넘어온 모델 개체 
    /// </summary>
    [Parameter] public TextTemplateModel ModelSender { get; set; } = new();

    /// <summary>
    /// 편집 전용 모델(복사본)
    /// </summary>
    public TextTemplateModel ModelEdit { get; set; } = new();

    /// <summary>
    /// 부모 컴포넌트에게 생성(Create)이 완료되었다고 보고하는 목적으로 부모 컴포넌트에게 알림
    /// (강의용) Action 대리자 사용
    /// </summary>
    [Parameter] public Action? CreateCallback { get; set; }

    /// <summary>
    /// 부모 컴포넌트에게 수정(Edit)이 완료되었다고 보고하는 목적으로 부모 컴포넌트에게 알림
    /// (강의용) EventCallback 구조체 사용
    /// </summary>
    [Parameter] public EventCallback<bool> EditCallback { get; set; }

    [Parameter] public string ParentKey { get; set; } = "";
    #endregion

    #region Injectors
    /// <summary>
    /// 리포지토리 클래스에 대한 참조 
    /// </summary>
    [Inject] public ITextTemplateRepository? RepositoryReference { get; set; }
    #endregion

    #region Lifecycle Methods
    // 넘어온 Model 값을 수정 전용 ModelEdit에 담기 
    protected override void OnParametersSet()
    {
        // ModelSender를 기반으로 편집용 복사본을 구성
        ModelEdit = new TextTemplateModel
        {
            Id = ModelSender.Id,
            Message = ModelSender.Message,
            Title = ModelSender.Title
            // 더 많은 정보는 여기에서...
        };
    }
    #endregion

    #region Event Handlers
    // Blazor 이벤트 핸들러는 async void 대신 async Task 권장
    protected async Task CreateOrEditClick()
    {
        if (RepositoryReference is null)
            throw new InvalidOperationException($"{nameof(RepositoryReference)} was not injected.");

        // 편집용 복사본(ModelEdit) -> 실제 저장 모델(ModelSender)로 반영
        ModelSender.Message = ModelEdit.Message;
        ModelSender.Title = ModelEdit.Title;
        // ModelSender.CreatedBy = UserName ?? "Anonymous";

        if (ModelSender.Id == 0)
        {
            // Create
            await RepositoryReference.AddAsync(ModelSender);

            // (강의용) Action 콜백 호출: 부모 쪽이 Task 메서드이면 래퍼(Bridge)로 연결
            CreateCallback?.Invoke();
        }
        else
        {
            // Edit
            await RepositoryReference.UpdateAsync(ModelSender);
            await EditCallback.InvokeAsync(true);
        }
    }
    #endregion
}
