namespace Chat.Common.Entities.User.DB
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
    }

    public class Company
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public List<User>? Users { get; set; }
    }
}
