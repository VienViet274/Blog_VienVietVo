namespace Blog_API.Models.DTO
{
    public class UserWithRolesDTO: UserDTO
    {
        public List<string> Roles { get; set; }
    }
}
