using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TheThanh_WebAPI_Flight.Authorization
{
    public class CustomAuthorizationAttribute
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class CustomAuthorizeAttribute : TypeFilterAttribute
        {
            public CustomAuthorizeAttribute(string[] permission) : base(typeof(PermissionFilter)) // PermissionFilter sẽ được sử dụng để thực thi logic kiểm tra quyền.
            {
                Arguments = new object[] { new PermissionRequirement(permission) };
            }
        }

        public class PermissionFilter : IAuthorizationFilter
        {
            private readonly IAuthorizationService _authorization;
            private readonly PermissionRequirement _requirement;

            public PermissionFilter(IAuthorizationService authorization, PermissionRequirement requirement)
            {
                _authorization = authorization;
                _requirement = requirement;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                // Kiểm tra xác thực
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Lấy TypeID từ route hoặc query parameter
                if (context.RouteData.Values.TryGetValue("typeId", out object? typeIdValue)
                    && int.TryParse(typeIdValue.ToString(), out int typeId))
                {
                    _requirement.TypeID = typeId; // Gán TypeID vào yêu cầu
                }
                if (context.RouteData.Values.TryGetValue("documentId", out object? documentIdValue)
                    && int.TryParse(documentIdValue.ToString(), out int documentId))
                {
                    _requirement.DocumentID = documentId; // Gán TypeID vào yêu cầu
                }

                // Thực hiện kiểm tra quyền
                AuthorizationResult result = _authorization.AuthorizeAsync(context.HttpContext.User, null, _requirement).Result;
                if (!result.Succeeded)
                {
                    context.Result = new ForbidResult();
                }
            }
        }


    }
}
