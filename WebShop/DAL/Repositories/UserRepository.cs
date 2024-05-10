using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context) { }
}