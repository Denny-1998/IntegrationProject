namespace UserService.Models
{
    public class User
    {
        public string UserID { get; set; }  
        public string Username { get; set; }
        public string Email { get; set; }

        public List<int> Following { get; set; } = new List<int>();
        public List<int> Followers { get; set; } = new List<int>();
    }
}
