using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Interfaces;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
      private readonly SymmetricSecurityKey _key;
      public JwtGenerator(IConfiguration config)
      {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
      }

    public string CreateToken(AppUser user)
      {
        var claims = new List<Claim>
        {
          // When the Token will be created, userName of user will be a nameId
          new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
        };

        // generate signing credentials

        // this key MUST stay on server. It is used to sign Tokens.
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(claims),
          Expires = DateTime.Now.AddDays(7),
          SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Here Im creating a Token using a handler and based on a descriptor, that add 
        // claims, signing creds and duration of token.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
      }
  }
}