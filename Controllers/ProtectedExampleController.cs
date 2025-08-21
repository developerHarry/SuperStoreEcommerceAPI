using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SuperStoreEcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedExampleController : ControllerBase
    {
        // For Any Authenticated User
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me() => Ok(new { message = "You are authenticated." });

        // Only Admin
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly() => Ok(new { message = "Hello Admin" });

        // Only StoreManager
        [HttpGet("manager")]
        [Authorize(Roles = "StoreManager")]
        public IActionResult ManagerOnly() => Ok(new { message = "Hello Store Manager" });

        [HttpGet("customer")]
        [Authorize(Roles = "Customer")]
        public IActionResult CustomerOnly() => Ok(new { message = "Hello Customer" });
    }
}
