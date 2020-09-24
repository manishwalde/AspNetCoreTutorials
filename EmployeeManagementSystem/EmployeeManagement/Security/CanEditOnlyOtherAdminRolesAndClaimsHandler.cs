using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : 
        AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ManageAdminRolesAndClaimsRequirement requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

            if (context.User.IsInRole("Admin") &&
           context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
           adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            /* 1.In general, do not return failure from a handler, as other handlers for the same requirement may succeed.
               2.Only return an explicit failure, when you want to guarantee the failure of the policy even when the other handlers succeed.
               3.By default, all handlers are called, irrespective of what a handler returns (success, failure or nothing). 
                This is because in the other handlers, there might be something else going on besides evaluating requirements, 
                may be logging for example.
             * */
            //else
            //{
            //    context.Fail();
            //}

            return Task.CompletedTask;
        }
    }
}
