using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Helpers;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;
public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<Usuario> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var user = new Usuario
        {
            Nombre = registerDto.Nombre,
            CorreoElectronico = registerDto.CorreoElectronico
        };

        user.Contraseña = _passwordHasher.HashPassword(user, registerDto.Contraseña); //Encrypt password
        Console.WriteLine("paso contra");
        var existingUser = _unitOfWork.Usuarios
                                    .Find(u => u.Nombre.ToLower() == registerDto.Nombre.ToLower())
                                    .FirstOrDefault();

        if (existingUser == null)
        {
            Console.WriteLine("1");
            var rolDefault = _unitOfWork.Roles
                                    .Find(u => u.Nombre == Authorization.rol_default.ToString())
                                    .First();
            try
            {
                            Console.WriteLine("2");
                            Console.WriteLine(Authorization.rol_default.ToString());
                user.Roles.Add(rolDefault);
                                            Console.WriteLine("3");

                _unitOfWork.Usuarios.Add(user);
                                            Console.WriteLine("4");

                await _unitOfWork.SaveAsync();

                return $"User  {registerDto.Nombre} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"User {registerDto.Nombre} already registered.";
        }
    }
    public async Task<DataUserDto> GetTokenAsync(LoginDto model)
    {
        Console.WriteLine("aaaa");
        DataUserDto dataUserDto = new DataUserDto();
        var user = await _unitOfWork.Usuarios
                    .GetByUsernameAsync(model.Nombre);

        if (user == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"User does not exist with username {model.Nombre}.";
            return dataUserDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Contraseña, model.Contraseña);

        if (result == PasswordVerificationResult.Success)
        {
            dataUserDto.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
                                                        Console.WriteLine("3");

            dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                                                        Console.WriteLine("4");

            dataUserDto.UserName = user.Nombre;
            dataUserDto.Roles = user.Roles
                .Select(u => u.Nombre)
                .ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                dataUserDto.RefreshToken = activeRefreshToken.Token;
                dataUserDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                dataUserDto.RefreshToken = refreshToken.Token;
                dataUserDto.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _unitOfWork.Usuarios.Update(user);
                await _unitOfWork.SaveAsync();
            }

            return dataUserDto;
        }
        dataUserDto.IsAuthenticated = false;
        dataUserDto.Message = $"Credenciales incorrectas para el usuario {user.Nombre}.";
        return dataUserDto;
    }
    public async Task<string> AddRoleAsync(AddRoleDto model)
    {

        var user = await _unitOfWork.Usuarios
                    .GetByUsernameAsync(model.Nombre);
        if (user == null)
        {
            return $"User {model.Nombre} does not exists.";
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Contraseña, model.Contraseña);

        if (result == PasswordVerificationResult.Success)
        {
            var rolExists = _unitOfWork.Roles
                                        .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
                                        .FirstOrDefault();

            if (rolExists != null)
            {
                var userHasRole = user.Roles.Any(u => u.Id == rolExists.Id);

                if (userHasRole == false)
                {
                    user.Roles.Add(rolExists);
                    _unitOfWork.Usuarios.Update(user);
                    await _unitOfWork.SaveAsync();
                }

                return $"Role {model.Role} added to user {model.Nombre} successfully.";
            }

            return $"Role {model.Role} was not found.";
        }
        return $"Invalid Credentials";
    }
    public async Task<DataUserDto> RefreshTokenAsync(string refreshToken)
    {
        var dataUserDto = new DataUserDto();

        var usuario = await _unitOfWork.Usuarios
                        .GetByRefreshTokenAsync(refreshToken);

        if (usuario == null)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not assigned to any user.";
            return dataUserDto;
        }

        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

        if (!refreshTokenBd.IsActive)
        {
            dataUserDto.IsAuthenticated = false;
            dataUserDto.Message = $"Token is not active.";
            return dataUserDto;
        }
        //Revoque the current refresh token and
        refreshTokenBd.Revoked = DateTime.UtcNow;
        //generate a new refresh token and save it in the database
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveAsync();
        //Generate a new Json Web Token
        dataUserDto.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        dataUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        /*         dataUserDto.CorreoElectronico = usuario.CorreoElectronico;
         */
        dataUserDto.UserName = usuario.Nombre;
        dataUserDto.Email = usuario.CorreoElectronico;
        dataUserDto.Roles = usuario.Roles
                                        .Select(u => u.Nombre)
                                        .ToList();
        dataUserDto.RefreshToken = newRefreshToken.Token;
        dataUserDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return dataUserDto;
    }
    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }
    private JwtSecurityToken CreateJwtToken(Usuario usuario)
    {
        var roles = usuario.Roles;
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("roles", role.Nombre));
        }
        var claims = new[]
        {
                                new Claim(JwtRegisteredClaimNames.Sub, usuario.Nombre),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                                new Claim("uid", usuario.Id.ToString())
                        }
        .Union(roleClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}
