using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PLANTILLA.Helpers;
using PLANTILLA.Models;
using PLANTILLA.Services;

namespace PLANTILLA.Controllers
{
    public class VC_HeaderViewComponent : ViewComponent
    {
        public VC_HeaderViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
        
    }
}
