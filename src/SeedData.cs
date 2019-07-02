namespace src
{
    using Bogus;
    using src.Domain;
    using System.Linq;

    /// <summary>
    /// Using Bogus for fake data, refer the link: https://github.com/bchavez/Bogus
    /// </summary>
    public class SeedData
    {
        private readonly TestDbContext _context;

        public SeedData(TestDbContext context)
        {
            _context = context;
        }

        public void SeedUser()
        {

            if (!_context.Users.Any())
            {
                for (var i = 0; i <= 1; i++)
                {
                    var testUsers = new Faker<User>()
                    .CustomInstantiator(f => new User())
                    .RuleFor(u => u.DisplayName, (f, u) => $"{f.Name.FirstName()} {f.Name.LastName()}")
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.DisplayName));

                    _context.Users.Add(testUsers.Generate());
                }

                _context.SaveChanges();
            }
          
        }
    }
}
