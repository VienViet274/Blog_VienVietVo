using System.ComponentModel.DataAnnotations;

namespace Blog_Client.Models.DTO
{
    public class UserWithRolesDTO: UserDTO
    {
        //public string id { get; set; }
        //public string username { get; set; }
        //public string name { get; set; }
        [MinLength(1)]
        public List<string> Roles { get; set; } = new List<string>();
    }
}
