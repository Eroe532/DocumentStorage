using DocumentStorage.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentStorage.DAL.Context
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class AppPostgreContext : DbContext
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options">Опции</param>
        public AppPostgreContext(DbContextOptions<AppPostgreContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// При создании моделей
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentInfoModel>(DocumentInfoModelConfigure);
            modelBuilder.Entity<FileModel>(FileModelConfigure);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void DocumentInfoModelConfigure(EntityTypeBuilder<DocumentInfoModel> builder)
        {
            builder.HasKey(u => u.Id);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void FileModelConfigure(EntityTypeBuilder<FileModel> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
