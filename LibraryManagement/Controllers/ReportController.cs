using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen("Admin")]
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _reportService.GetTongQuanAsync();
            return View(model);
        }
    }
}