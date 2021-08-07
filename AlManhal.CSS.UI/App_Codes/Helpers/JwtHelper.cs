using PWC.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Configuration;

namespace PWC.UI.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwt(Enums.Issuer Issuer, List<Claim> claims = null)
        {
            string JwtKey = GetJwtKey(Issuer);
            string JwtIssuer = GetJwtIssuer(Issuer);

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtKey));


            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(JwtIssuer,
              JwtIssuer,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static bool ValidateToken(Enums.Issuer Issuer, string authToken)
        {
            bool isValid = true;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                string JwtKey = GetJwtKey(Issuer);
                string JwtIssuer = GetJwtIssuer(Issuer);

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true, // Because there is no expiration in the generated token
                    ValidateAudience = true, // Because there is no audiance in the generated token
                    ValidateIssuer = true,   // Because there is no issuer in the generated token
                    ValidIssuer = JwtIssuer,
                    ValidAudience = JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JwtKey)) // The same key as the one that generate the token
                };
                SecurityToken tmp;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out tmp);
            }
            catch (Exception ex)
            {
                isValid = false;
            }
            return isValid;
        }
        public static string GetJwtIssuer(Enums.Issuer Issuer)
        {
            string JwtIssuer = string.Empty;
            switch (Issuer)
            {
                case Enums.Issuer.DC:
                    JwtIssuer = @WebConfigurationManager.AppSettings["DC_Issuer"].ToString();
                    break;
            }
            return JwtIssuer;
        }
        public static string GetJwtKey(Enums.Issuer Issuer)
        {
            string JwtKey = string.Empty;
            switch (Issuer)
            {
                case Enums.Issuer.DC:
                    JwtKey = @WebConfigurationManager.AppSettings["DC_Key"].ToString();
                    break;
            }
            return JwtKey;
        }
        public static HttpClient GetHttpClientAuthorization(List<Claim> claims = null)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            "Bearer", JwtHelper.GenerateJwt(Enums.Issuer.CSS));
            return client;
        }
    }
}
