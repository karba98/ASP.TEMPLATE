using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace PLANTILLA.Helpers
{
    public class HelperToken
    {
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public String Secretkey { get; set; }

        public HelperToken(IConfiguration configuration)
        {
            this.Issuer = configuration["ApiAuth:Issuer"];
            this.Audience = configuration["ApiAuth:Audience"];
            this.Secretkey = configuration["ApiAuth:SecretKey"];
        }

        //CREAMOS UN METODO PARA GENERAR UNA CLAVE
        //SIMETRICA A PARTIR DE NUESTRO SecretKey
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data =
                System.Text.Encoding.UTF8.GetBytes(this.Secretkey);
            return new SymmetricSecurityKey(data);
        }

        //METODO PARA CONFIGURAR LAS OPCIONES DE SEGURIDAD DEL TOKEN
        //LOS METODOS DE CONFIGURACION SON Action
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> jwtoptions =
                new Action<JwtBearerOptions>(options =>
                {
                    options.TokenValidationParameters =
                    new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
,
                        ValidateIssuerSigningKey = true
,
                        ValidIssuer = this.Issuer
,
                        ValidAudience = this.Audience
,
                        IssuerSigningKey = this.GetKeyToken()
                    };
                });
            return jwtoptions;
        }
        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> authoptions =
                new Action<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme
                    = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                });
            return authoptions;
        }

    }
}
