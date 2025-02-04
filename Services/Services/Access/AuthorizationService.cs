using Services.Interfaces.Access;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Domain.Security;
using AutoMapper;
using Domain.Interfaces.Data.Repositories;
using Domain.Entities;
using Services.DTOs.Access;


namespace Services.Services.Access
{
    public class AuthorizationService : IAuthorizations
    {
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IAccessRepository _accessRepository;
        private readonly IMapper _mapper;
        public AuthorizationService(SigningConfigurations signingConfigurations, IMapper mapper, IAccessRepository accessRepository)
        {
            this._accessRepository = accessRepository;
            this._mapper = mapper;
            this._signingConfigurations = signingConfigurations;
        }

        public async Task<object> CheckAccess(UsuarioDTO objUsuario)
        {
            object retorno = null;

            try
            {
                var objUsuarioModel = _mapper.Map<UsuarioModel>(objUsuario);
                var dadosLoginModel= await _accessRepository.GetUsuario(objUsuarioModel);
                var dadosLoginDTO = _mapper.Map<UsuarioDTO>(dadosLoginModel);

                if (dadosLoginDTO != null)
                {
                        string sUniqueName = $"{dadosLoginDTO.nome}";
                        ClaimsIdentity identity = new ClaimsIdentity(
                             new GenericIdentity(dadosLoginDTO.nome),
                             new[]
                             {
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             new Claim(JwtRegisteredClaimNames.UniqueName, sUniqueName),
                             new Claim("perfilAcesso", "Admin")
                             }
                         );

                        DateTime createDate = DateTime.Now;
                        DateTime expirationDate = createDate + TimeSpan.FromDays(1); // o token expira em 1 dia

                        var handler = new JwtSecurityTokenHandler();
                        string token = CreateToken(identity, createDate, expirationDate, handler);
                        retorno = SuccessObject(createDate, expirationDate, token, dadosLoginDTO.nome);
                }
                else
                {
                    retorno = new
                    {
                        authenticated = false,
                        create = "",
                        expiration = "",
                        accessToken = "",
                        userName = objUsuario.usuario,
                        message = "Acesso Negado"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(">> (Serviço API Youtube) Erro ao tentar gerar o token :: " + ex.Message);
            }

            return retorno;
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "sistema_api",
                Audience = "api_youtube",
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate,
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, string usuario)
        {
            return new
            {
                authenticated = true,
                create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = usuario,
                message = "Usuário Logado com sucesso"
            };
        }
    }
}
