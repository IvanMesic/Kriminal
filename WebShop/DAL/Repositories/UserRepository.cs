using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using DAL.Model.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

public class UserRepository : IUserRepository
{

    private readonly DataContext ctx;

    public UserRepository(DataContext context) 
    {
        ctx = context;
    }

    public bool Authenticate(LoginDTO login)
    {
        var target = GetUser(login.Username);

        if (target == null)
        {
            return false;
        }

        byte[] salt = Convert.FromBase64String(target.PasswordSalt);
        byte[] hash = Convert.FromBase64String(target.PasswordHash);

        byte[] calcHash = GetHash(login.Password, salt);

        return hash.SequenceEqual(calcHash);
    }

    public User? GetUser(int id)
    {
        return ctx.User.FirstOrDefault(user => user.UserId == id);
    }
        
    public User? GetUser(string username)
    {
        return ctx.User.FirstOrDefault(user => user.Username.Equals(username));
    }

    public void Register(RegisterDTO register)
    {
        if (ctx.User.Any(x => x.Username.Equals(register.Username.ToLower().Trim())))
            throw new InvalidOperationException("Username already exists.");

        if (ctx.User.Any(x => x.Email.Equals(register.Email.ToLower().Trim())))
            throw new InvalidOperationException("This E-Mail is already in use.");

        string hash;
        string salt;
        ComputeHashAndSalt(register.Password, out salt, out hash);

        byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
        string b64SecToken = Convert.ToBase64String(securityToken);

        var newUser = new User
        {
            Username = register.Username,
            Email = register.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = register.Role,
        };

        ctx.User.Add(newUser);
        ctx.SaveChanges();
    }

    public bool UserExists(int id)
    {
        return GetUser(id) != null;
    }

    public bool UserExists(string username)
    {
        return GetUser(username) != null;
    }

    private byte[] GetHash(string password, byte[] bytesSalt)
    {
        byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: bytesSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);
        return hash;
    }

    private void ComputeHashAndSalt(string password, out string salt, out string hash)
    {
        byte[] bytesSalt = RandomNumberGenerator.GetBytes(128 / 8); 
        string b64Salt = Convert.ToBase64String(bytesSalt);

        byte[] bytesHash = GetHash(password, bytesSalt);
        string b64Hash = Convert.ToBase64String(bytesHash);

        salt = b64Salt;
        hash = b64Hash;
    }

    public bool CanCreate(RegisterDTO register)
    {
        if (ctx.User.Any(x => x.Username.Equals(register.Username.ToLower().Trim())))
            return false;

        if (ctx.User.Any(x => x.Email.Equals(register.Email.ToLower().Trim())))
            return false;

        return true;
    }



    public IList<User> GetAll()
    {
        return ctx.User.ToList();
    }

    public User GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(User entity)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }
}
