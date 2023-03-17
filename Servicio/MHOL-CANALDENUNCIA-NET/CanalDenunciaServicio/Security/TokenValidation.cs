using CanalDenunciaServicio.Helper;
using CanalDenunciaServicio.Models;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Newtonsoft.Json;

namespace CanalDenunciaServicio.Security
{
    internal static class TokenValidation
    {
        public static Token ValidaToken(string token)
        {
            Util util = new Util();

            try
            {
                //var json = new JwtBuilder()
                //    .WithSecret(util.Decrypt(util.SecretKey.ToString()))
                //    .MustVerifySignature()
                //    .Decode(token);

                var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                     .WithSecret(util.Decrypt(util.SecretKey.ToString()))
                     .MustVerifySignature()
                     .Decode(token);

                return JsonConvert.DeserializeObject<Token>(json.ToString());
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
        }
    }
}