using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;

namespace MVC_Projekt_Elearning.ViewComponents
{
    public class FooterViewComponent :ViewComponent
    {
        private readonly ISettingService _settingService;
        public FooterViewComponent( ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var datas = new FooterVMVC
            {
                Settings = await _settingService.GetAllAsync(),
            };

            return View(datas);
        }
    }
    public class FooterVMVC
    {
        public IDictionary<string, string> Settings { get; set; }
    }
}
