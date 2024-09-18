using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LegacyTest.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SictegWarehouse.Filters
{
    public class FilterOperation : AuthorizeAttribute, IAsyncAuthorizationFilter
    {

        private int idOperacion;

        public FilterOperation(int _idOperacion = 0)
        {
            this.idOperacion = _idOperacion;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var service = (IAuthorizationService)context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService));
            int idRole = Int32.Parse(SessionHelper.GetIdRol(context.HttpContext.User));
            var roleRequirement = new RoleRequirement(this.idOperacion);
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
                this.operacion = _operacion;
            }

            public int operacion { get; private set; }
        }

        public class CustomRoleRequirementHandler : AuthorizationHandler<RoleRequirement>
        {
            private readonly LegacyDBContext _contexto = null;

            public CustomRoleRequirementHandler(LegacyDBContext contexto)
            {
                this._contexto = contexto;
            }
            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
            {
                var idPerfil = Int32.Parse(SessionHelper.GetIdRol(context.User));

                var param = new SqlParameter[] {
                   new SqlParameter {ParameterName = "@idPerfil", SqlDbType = SqlDbType.Int, Value= idPerfil }
                };
                           
                var lstOperaciones = await _contexto.AdminPermissions.FromSqlRaw($"GetPermisos @idPerfil", param).ToListAsync();

                if (lstOperaciones.SingleOrDefault(x => x.IdOperation == requirement.operacion) != null)
                {
                    context.Succeed(requirement);
                }

            }
        }
    }
}
