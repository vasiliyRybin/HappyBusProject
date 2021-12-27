using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace HappyBusProject.AuthLayer.Common
{
    public class AuthOptions
    {
        public AuthOptions()
        {
            Secret = Environment.GetEnvironmentVariable("MeineSekretischeKey", EnvironmentVariableTarget.Machine);
        }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int TokenLifetime { get; set; }      //seconds

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}
