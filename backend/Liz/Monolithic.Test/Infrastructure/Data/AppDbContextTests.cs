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
        public async Task SaveChangesAsync_ShouldSetCreatedAtAndUpdatedAt_OnAddAndModify()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var user = new User { DeviceFingerprint = "testDeviceFingerprint", Nickname = "testNickname1", IsActive = true };
            db.Users.Add(user);

            // Act
            await db.SaveChangesAsync();

            // Assert
            Assert.True(user.CreatedAt != default);
            Assert.True(user.UpdatedAt != default);
            var createdAt = user.CreatedAt;
            var updatedAt = user.UpdatedAt;

            // Modify
            user.Nickname = "testNickname2";
            await db.SaveChangesAsync();

            Assert.Equal(createdAt, user.CreatedAt); // CreatedAt 不變
            Assert.True(user.UpdatedAt > updatedAt); // UpdatedAt 會更新
        }

        [Fact]
        public void OnModelCreating_ShouldApplyEntityConfigurations()
        {
            var db = GetInMemoryDbContext();
            var model = db.Model;

            // 驗證 User 實體的 DeviceFingerprint 欄位有 MaxLength 128 且 Required
            var userEntity = model.FindEntityType(typeof(User));
            Assert.NotNull(userEntity);
            var deviceFingerprintProp = userEntity!.FindProperty("DeviceFingerprint");
            Assert.NotNull(deviceFingerprintProp);
            Assert.Equal(128, deviceFingerprintProp!.GetMaxLength());
            Assert.False(deviceFingerprintProp.IsNullable);

            // 驗證 Room 實體的 InviteCode 欄位有 MaxLength 32 且 Required
            var roomEntity = model.FindEntityType(typeof(Room));
            Assert.NotNull(roomEntity);
            var inviteCodeProp = roomEntity!.FindProperty("InviteCode");
            Assert.NotNull(inviteCodeProp);
            Assert.Equal(32, inviteCodeProp!.GetMaxLength());
            Assert.False(inviteCodeProp.IsNullable);
        }
    }
}
