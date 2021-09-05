using System;
using Microsoft.EntityFrameworkCore;

namespace CndBot.Core.Database
{
    public sealed class DataBaseProvider : DbContext
    {
        private const string DB_CON_STR = "Server=localhost;Database=cdndb;User=root;";

        public DbSet<FormDataModel> FormDataModels { get; set; }
        public DbSet<EventDataModel> EventDataModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(DB_CON_STR, new MariaDbServerVersion(new Version(10,5)));
        }
        
        public DataBaseProvider(DbContextOptions<DataBaseProvider> options) : base(options)
        {
            Database.OpenConnection();
            Database.EnsureCreated();
        }
    }
}