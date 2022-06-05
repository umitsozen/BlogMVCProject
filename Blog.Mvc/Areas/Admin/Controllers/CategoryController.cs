using Blog.Services.Abstract;
using Blog.Shared.Utilities.Results.ComplexType;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();
            return View(result.Data);

        }

        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }
    }
}
