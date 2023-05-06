using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class HelperToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secretkey { get; set; }

        public HelperToken(IConfiguration configuration)
        {
            Issuer = configuration["ApiOAuth:Issuer"];
            Audience = configuration["ApiOAuth:Audience"];
            Secretkey = configuration["ApiOAuth:SecretKey"];
        }

        //CREAMOS UN METODO PARA GENERAR UNA CLAVE
        //SIMETRICA A PARTIR DE NUESTRO SecretKey
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data =
                System.Text.Encoding.UTF8.GetBytes(Secretkey);
            return new SymmetricSecurityKey(data);
        }

        //METODO PARA CONFIGURAR LAS OPCIONES DE SEGURIDAD DEL TOKEN
        //LOS METODOS DE CONFIGURACION SON Action
        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = GetKeyToken(),
                };
            }
            );
            return options;
        }
        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            );
            return options;
        }

    }
}
