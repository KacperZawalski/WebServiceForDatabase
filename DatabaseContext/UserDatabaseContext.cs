using Microsoft.EntityFrameworkCore;
using WebServiceForDatabase.Models;

namespace WebServiceForDatabase.DatabaseContext
{
    public class UserDatabaseContext : DbContext
    {
        public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options) : base(options)
        {

        }
        public void AddUserToDatabase(UserModel userModel)
        {
            if (CheckForDuplicateIds(userModel))
            {
                users.Add(userModel);
            }
            else
            {
                Console.WriteLine("Ids can't be identical");
            }
            
        }
        private bool CheckForDuplicateIds(UserModel userModel)
        {
            foreach (UserModel user in users)
            {
                if (user.iD.Equals(userModel.iD))
                {
                    return false;
                }
            }
            return true;
        }
        public DbSet<UserModel> users => Set<UserModel>();
    }
}
