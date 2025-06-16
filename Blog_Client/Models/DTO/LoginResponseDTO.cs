namespace Blog_Client.Models.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO user { get; set; }
        public string token { get; set; }
    }
}
