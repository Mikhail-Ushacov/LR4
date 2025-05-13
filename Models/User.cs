namespace LR4.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Passport { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{Name};{Surname};{Passport};{Password}";
        }
    }
}
