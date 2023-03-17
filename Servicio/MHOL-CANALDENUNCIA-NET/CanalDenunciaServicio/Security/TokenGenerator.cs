using CanalDenunciaServicio.Helper;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Web;

namespace CanalDenunciaServicio.Security
{
    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(string usuario, string email, string nombreUsuario, int duracionToken)
        {
            Util util = new Util();

            var token = new JwtBuilder()
                   .WithAlgorithm(new HMACSHA256Algorithm())
                   .WithSecret(util.Decrypt(util.SecretKey.ToString()))
                   .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(duracionToken).ToUnixTimeSeconds())
                   .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                   .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                   .AddClaim("iss", HttpContext.Current.Request.Url.Host.ToString())
                   .AddClaim("usuario", usuario)
                   .AddClaim("nombreUsuario", nombreUsuario)
                   .AddClaim("email", email)
                   .Encode();

            return token;
        }
    }
}