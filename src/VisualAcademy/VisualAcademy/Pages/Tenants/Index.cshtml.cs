namespace VisualAcademy.Pages.Tenants
{
    public class IndexModel : PageModel
    {
        // IndexModel�� ���� �α��� ����ϴ� ILogger �ʵ�
        private readonly ILogger<IndexModel> _logger;

        // �����ͺ��̽� ���ؽ�Ʈ�� ����ϴ� ApplicationDbContext �ʵ�
        private readonly ApplicationDbContext _context;

        // ������: ������ �������� ILogger�� ApplicationDbContext�� �޾� �ʱ�ȭ
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            // ���� logger�� null�̸� NullLogger �ν��Ͻ��� ����Ͽ� ���� ����
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<IndexModel>.Instance;

            // ApplicationDbContext�� �ʱ�ȭ
            _context = context;
        }

        // TenantModel ����� �����ϴ� �Ӽ�
        public required IList<TenantModel> TenantModel { get; set; }

        // �������� GET ��û�� ���� �� �񵿱�� ȣ��Ǵ� �޼���
        public async Task OnGetAsync()
        {
            // �α�: GET ��û�� ���۵��� �˸�
            _logger.LogInformation("OnGetAsync �޼��尡 ȣ��Ǿ����ϴ�.");

            try
            {
                // �����ͺ��̽����� Tenant ����� ������ TenantModel �Ӽ��� ����
                TenantModel = await _context.Tenants.ToListAsync();
                // �α�: ������ ��ȸ ���� �޽���
                _logger.LogInformation("Tenant �����͸� ���������� ��ȸ�߽��ϴ�. �� {Count}���� �׸��� �ֽ��ϴ�.", TenantModel.Count);
            }
            catch (Exception ex)
            {
                // �α�: ������ ��ȸ ���� �� ���� ������ ���
                _logger.LogError(ex, "Tenant �����͸� ��ȸ�ϴ� ���� ������ �߻��߽��ϴ�.");
            }
        }
    }
}
