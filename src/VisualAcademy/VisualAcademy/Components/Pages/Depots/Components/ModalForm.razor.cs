using Microsoft.AspNetCore.Components;
using Azunt.DepotManagement;

namespace Azunt.Web.Components.Pages.Depots.Components;

public partial class ModalForm : ComponentBase
{
    #region Properties

    /// <summary>
    /// 모달 다이얼로그 표시 여부
    /// </summary>
    public bool IsShow { get; set; } = false;

    #endregion

    #region Public Methods

    public void Show() => IsShow = true;

    public void Hide()
    {
        IsShow = false;
        StateHasChanged();
    }

    #endregion

    #region Parameters

    [Parameter]
    public string UserName { get; set; } = "";

    [Parameter]
    public RenderFragment EditorFormTitle { get; set; } = null!;

    [Parameter]
    public Depot ModelSender { get; set; } = null!;

    public Depot ModelEdit { get; set; } = null!;

    [Parameter]
    public Action CreateCallback { get; set; } = null!;

    [Parameter]
    public EventCallback<bool> EditCallback { get; set; }

    [Parameter]
    public string ParentKey { get; set; } = "";

    #endregion

    #region Lifecycle

    protected override void OnParametersSet()
    {
        if (ModelSender != null)
        {
            ModelEdit = new Depot
            {
                Id = ModelSender.Id,
                Name = ModelSender.Name,
                Active = ModelSender.Active,
                CreatedAt = ModelSender.CreatedAt,
                CreatedBy = ModelSender.CreatedBy
            };
        }
        else
        {
            ModelEdit = new Depot();
        }
    }

    #endregion

    #region Injectors

    [Inject]
    public IDepotRepository RepositoryReference { get; set; } = null!;

    #endregion

    #region Event Handlers

    protected async Task HandleValidSubmit()
    {
        ModelSender.Active = true;
        ModelSender.Name = ModelEdit.Name;
        ModelSender.CreatedBy = UserName ?? "Anonymous";

        if (ModelSender.Id == 0)
        {
            ModelSender.CreatedAt = DateTime.UtcNow;
            await RepositoryReference.AddAsync(ModelSender);
            CreateCallback?.Invoke();
        }
        else
        {
            await RepositoryReference.UpdateAsync(ModelSender);
            await EditCallback.InvokeAsync(true);
        }

        Hide();
    }

    #endregion
}
