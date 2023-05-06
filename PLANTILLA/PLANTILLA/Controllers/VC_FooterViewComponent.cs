using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PLANTILLA.Controllers
{
    public class VC_FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
