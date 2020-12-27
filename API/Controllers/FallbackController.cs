using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  // Controller got view support, and that controller will return view.
  public class FallbackController : Controller
  {
    // Everything has to be authenticated except things with tag like that.
    [AllowAnonymous]
    public IActionResult Index()
    {
      return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
    }
  }
}