namespace DevLogix.Controllers;

using Microsoft.AspNetCore.Mvc;

public class ThemeController : Controller
{
    [HttpPost]
    public IActionResult Toggle(string returnUrl = "/")
    {
        var currentTheme = Request.Cookies["theme"] ?? "light";
        var newTheme = currentTheme == "light" ? "dark" : "light";

        Response.Cookies.Append("theme", newTheme, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = false,
            IsEssential = true
        });

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }
}
