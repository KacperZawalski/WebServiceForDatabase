using System.Security.Cryptography.X509Certificates;

namespace WebServiceForDatabase.Models
{
    public class UserModel
    {
        public int iD { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string birthDate { get; set; }
        public string Address { get; set; }
    }
}
