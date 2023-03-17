using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace ProyectoWebApi.App_Start
{
    public class AccessPolicyCors : Attribute, ICorsPolicyProvider
    {

        public async Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var corsRequestContext = request.GetCorsRequestContext();
            var originRequested = corsRequestContext.Origin;

            if (await IsOriginFromCustomer(originRequested))
            {
                var policy = new CorsPolicy
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true
                };

                policy.Origins.Add(originRequested);

                //IP ESPECIFICA

                return policy;
            }
            return null;
        }

        private async Task<bool> IsOriginFromCustomer(string originRequested)
        {
            return true;
        }
    }
}