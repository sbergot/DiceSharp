namespace DiceSharp.WebApp.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static User Create(string username)
        {
            return new User
            {
                Name = username,
                Id = System.Guid.NewGuid().ToString()
            };
        }
    }
}