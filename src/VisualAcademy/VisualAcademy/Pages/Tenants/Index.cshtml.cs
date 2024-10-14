namespace VisualAcademy.Pages.Tenants
{
    public class IndexModel : PageModel
    {
        // IndexModel에 대한 로깅을 담당하는 ILogger 필드
        private readonly ILogger<IndexModel> _logger;

        // 데이터베이스 컨텍스트를 담당하는 ApplicationDbContext 필드
        private readonly ApplicationDbContext _context;

        // 생성자: 의존성 주입으로 ILogger와 ApplicationDbContext를 받아 초기화
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            // 만약 logger가 null이면 NullLogger 인스턴스를 사용하여 예외 방지
            _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<IndexModel>.Instance;

            // ApplicationDbContext를 초기화
            _context = context;
        }

        // TenantModel 목록을 저장하는 속성
        public required IList<TenantModel> TenantModel { get; set; }

        // 페이지가 GET 요청을 받을 때 비동기로 호출되는 메서드
        public async Task OnGetAsync()
        {
            // 로깅: GET 요청이 시작됨을 알림
            _logger.LogInformation("OnGetAsync 메서드가 호출되었습니다.");

            try
            {
                // 데이터베이스에서 Tenant 목록을 가져와 TenantModel 속성에 저장
                TenantModel = await _context.Tenants.ToListAsync();
                // 로깅: 데이터 조회 성공 메시지
                _logger.LogInformation("Tenant 데이터를 성공적으로 조회했습니다. 총 {Count}개의 항목이 있습니다.", TenantModel.Count);
            }
            catch (Exception ex)
            {
                // 로깅: 데이터 조회 실패 시 예외 정보를 기록
                _logger.LogError(ex, "Tenant 데이터를 조회하는 동안 오류가 발생했습니다.");
            }
        }
    }
}
