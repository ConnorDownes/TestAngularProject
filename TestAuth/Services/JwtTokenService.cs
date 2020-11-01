using AngularShop.Services.Interfaces;
using AngularShop.ViewModels.ConfigurationOptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AngularShop.Services
{
    public class JwtTokenService : ITokenService
    {
        private SecurityTokenHandler _tokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;
        private readonly AppSettingsOptions _appSettings;
        private readonly JwtConfigOptions _jwtConfig;
        public JwtTokenService(SecurityTokenHandler tokenHandler, SecurityTokenDescriptor tokenDescriptor, IOptions<AppSettingsOptions> appSettings, IOptions<JwtConfigOptions> jwtConfig)
        {
            _tokenHandler = tokenHandler;
            _tokenDescriptor = tokenDescriptor;
            _appSettings = appSettings.Value;
            _jwtConfig = jwtConfig.Value;
        }

        public string Generate(IIdentity user, int duration)
        {
            // generate token that is valid {duration} seconds
            var key = getKey();

            _tokenDescriptor.Issuer = _jwtConfig.Issuer;
            _tokenDescriptor.Audience = _jwtConfig.Audience;
            _tokenDescriptor.Subject = (ClaimsIdentity) user;
            _tokenDescriptor.Expires = DateTime.UtcNow.AddSeconds(duration);
            _tokenDescriptor.IssuedAt = DateTime.UtcNow;
            _tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var token = _tokenHandler.CreateToken(_tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        private byte[] getKey()
        {
            return Encoding.ASCII.GetBytes(_appSettings.Secret);
        }
    }
}
