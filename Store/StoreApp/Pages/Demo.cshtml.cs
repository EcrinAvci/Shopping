using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreApp.Pages
{
    public class DemoModel : PageModel
    {
        public String? FullName => HttpContext?.Session?.GetString("name") ?? "";

        public DemoModel()
        {
          
        }
        public void OnGet()
        {

        }
        public void OnPost([FromForm]string name)
        {
            HttpContext.Session.SetString("name",name);
            
        }
    }
}