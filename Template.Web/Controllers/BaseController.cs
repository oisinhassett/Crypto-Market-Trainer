using Microsoft.AspNetCore.Mvc;

namespace Template.Web.Controllers
{
    public enum AlertType { success, danger, warning, info }

    // Implements Alert functionality which is then accessible to any 
    // Controller inheriting from BaseController
    public class BaseController : Controller
    {
        public void Alert(string message, AlertType type = AlertType.info)
        {
            TempData["Alert.Message"] = message;
            TempData["Alert.Type"] = type.ToString();
        }

    }
       
}