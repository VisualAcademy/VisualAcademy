using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Azunt.Web.Components.Pages.Depots.Components;

public partial class DeleteDialog
{
    #region Parameters
    /// <summary>
    /// �θ𿡼� OnClickCallback �Ӽ��� ������ �̺�Ʈ ó���� ����
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }
    #endregion

    #region Properties
    /// <summary>
    /// ��� ���̾�α׸� ǥ���Ұ��� ���� 
    /// </summary>
    public bool IsShow { get; set; } = false;
    #endregion

    #region Public Methods
    /// <summary>
    /// �� ���̱� 
    /// </summary>
    public void Show() => IsShow = true;

    /// <summary>
    /// �� �ݱ�
    /// </summary>
    public void Hide() => IsShow = false;
    #endregion
}