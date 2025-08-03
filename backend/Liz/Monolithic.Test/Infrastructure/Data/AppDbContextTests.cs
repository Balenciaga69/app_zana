using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Test.Infrastructure.Data
{
    public class AppDbContextTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Can_Add_And_Retrieve_User()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var user = new User { IsOnline = true, LastActiveAt = DateTime.UtcNow };

            // Act
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var retrieved = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            Assert.NotNull(retrieved);
            Assert.Equal(user.Id, retrieved.Id);
            Assert.True(retrieved.IsOnline);
        }
    }
}
