using Microsoft.AspNetCore.Mvc.RazorPages;
using RedPlus.Models;
using RedPlus.Services;
using System.Collections.Generic;

namespace RedPlus.Pages.Portfolios
{
    public class IndexModel : PageModel
    {
        private readonly PortfolioServiceJsonFile _service;

        // �������� �Ű� ������ ����(�������丮) Ŭ���� ����
        public IndexModel(PortfolioServiceJsonFile service)
        {
            this._service = service;
        }

        public IEnumerable<Portfolio> Portfolios { get; private set; }

        public void OnGet()
        {
            Portfolios = _service.GetPortfolios(); 
        }
    }
}
