using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PLANTILLA.Controllers
{
    public class VC_NavBarViewComponent : ViewComponent
    {

        public VC_NavBarViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
