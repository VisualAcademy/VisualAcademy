using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Azunt.Web.Components.Pages.Depots.Components;

public partial class SearchBox : ComponentBase, IDisposable
{
    #region Fields
    private string searchQuery = "";
    private System.Timers.Timer? debounceTimer;
    #endregion

    #region Parameters

    /// <summary>
    /// �߰� HTML �Ӽ� (placeholder ��) ó��
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// �θ� ������Ʈ�� �˻��� ����
    /// </summary>
    [Parameter]
    public EventCallback<string> SearchQueryChanged { get; set; }

    /// <summary>
    /// ��ٿ �ð� (�⺻��: 300ms)
    /// </summary>
    [Parameter]
    public int Debounce { get; set; } = 300;

    #endregion

    #region Properties

    /// <summary>
    /// �˻��� ���ε� �Ӽ� (�Է� �� ��ٿ ����)
    /// </summary>
    public string SearchQuery
    {
        get => searchQuery;
        set
        {
            searchQuery = value;
            debounceTimer?.Stop();    // �Է� ���̸� ���� Ÿ�̸� ����
            debounceTimer?.Start();   // �� Ÿ�̸� ���� (�Է� �Ϸ� �� ����)
        }
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// ������Ʈ �ʱ�ȭ �� ��ٿ Ÿ�̸� ����
    /// </summary>
    protected override void OnInitialized()
    {
        debounceTimer = new System.Timers.Timer
        {
            Interval = Debounce,
            AutoReset = false // �� ���� ����ǵ��� ����
        };
        debounceTimer.Elapsed += SearchHandler!;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Search ��ư ���� Ŭ�� �� ��� �˻� ����
    /// </summary>
    protected void Search()
    {
        SearchQueryChanged.InvokeAsync(SearchQuery);
    }

    /// <summary>
    /// ��ٿ Ÿ�̸� ���� �� �̺�Ʈ �߻�
    /// </summary>
    protected async void SearchHandler(object source, ElapsedEventArgs e)
    {
        await InvokeAsync(() => SearchQueryChanged.InvokeAsync(SearchQuery));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// ���ҽ� ����
    /// </summary>
    public void Dispose()
    {
        debounceTimer?.Dispose();
    }

    #endregion
}