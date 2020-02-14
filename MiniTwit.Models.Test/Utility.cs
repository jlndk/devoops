using MiniTwit.Entities;
using System.Threading.Tasks;

namespace MiniTwit.Models.Tests
{
    static class Utility
    {
        

        public static async Task Add_dummy_data(UserRepository repository)
        {
            for (int i = 1; i < 10; i++)
            {
                var user1 = new User
                {
                    Username = "user" + i,
                    Email = "user" + i + "@kanban.com"
                };
                var (_, _) = await repository.CreateAsync(user1);
            }
        }
    }
}
