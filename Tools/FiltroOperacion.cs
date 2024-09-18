using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using LegacyTest.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace LegacyTest.Tools
{
    public class FiltroOperacion : AuthorizeAttribute, IAsyncAuthorizationFilter
    {

        private int idOperacion;

        public FiltroOperacion(int _idOperacion = 0)
        {
            idOperacion = _idOperacion;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var service = (IAuthorizationService)context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService));
            int idRole = int.Parse(SessionHelper.GetIdRol(context.HttpContext.User));
            var roleRequirement = new RoleRequirement(idOperacion);
            var result = await service.AuthorizeAsync(context.HttpContext.User, null, roleRequirement);
            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }


        public class RoleRequirement : IAuthorizationRequirement
        {
            public RoleRequirement(int _operacion = 0)
            {
                operacion = _operacion;
            }
            public int operacion { get; private set; }
        }

        public class CustomRoleRequirementHandler : AuthorizationHandler<RoleRequirement>
        {
            private readonly LegacyDBContext _context = null;

            public CustomRoleRequirementHandler(LegacyDBContext context)
            {
                _context = context;
            }
            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
            {
                var roleId = Int32.Parse(SessionHelper.GetIdRol(context.User));

                var lstOperaciones = await _context.AdminPermissions
                                            .Where(x=> x.IdRole== roleId )
                                            .ToListAsync();

                if (lstOperaciones.SingleOrDefault(x => x.IdOperation == requirement.operacion) != null)
                {
                    context.Succeed(requirement);
                }
            }

        }
    }
}

