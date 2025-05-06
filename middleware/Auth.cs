
namespace Thoughts.middleware {
    public class Auth  {
        private readonly RequestDelegate _next ;
        public Auth(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var token = context.Request.Cookies["AuthToken"];
            
            if(!string.IsNullOrEmpty(token)) {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);

            if(context.Response.StatusCode == 401 && 
            !context.Request.Path.StartsWithSegments("/Auth", StringComparison.OrdinalIgnoreCase) && 
            !context.Request.Path.StartsWithSegments("/Login", StringComparison.OrdinalIgnoreCase)){
                context.Response.Redirect("/Auth/Login");
            }
        }
    }
}