using DAL.Model;
using DAL.Model.DTO;

namespace DAL.Interfaces
{
    public interface IUserRepository : IRepository<User> 
    {
        public User? GetUser(int id);
        public User? GetUser(string username);

        public bool UserExists(int id);
        public bool UserExists(string username);
        public bool Authenticate(LoginDTO login);
        public bool CanCreate(RegisterDTO register);
        public void Register(RegisterDTO register);


    }
}
