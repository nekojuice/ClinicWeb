using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ClinicWeb.Areas.Identity
{
    public class ClinicAuthorizeFiltersControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            string apiArea;

            var authCodePolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("backend")
                .Build();
            var clientCredentialsPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("frontend", "Google")
                .Build();
            //var allPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .AddAuthenticationSchemes("backend", "frontend")
            //    .Build();

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Employee"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Appointment"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Attend"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }
            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Cases"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }
            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Chatroom"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("ClinicRoomSys"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Drugs"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Employee"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Member"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Room"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }

            if (controller.RouteValues.Any()
                && controller.RouteValues.TryGetValue("area", out apiArea)
                && apiArea.Equals("Schedule"))
            {
                controller.Filters.Add(new AuthorizeFilter("backendpolicy"));
            }
            //別動default default沒人用
            //else
            //{
            //    controller.Filters.Add(new AuthorizeFilter("defaultpolicy"));
            //}
        }
    }
}
