using Microsoft.EntityFrameworkCore;
namespace BookCat

{
    class ApplicationContext : DbContext
    {
        public DbSet<Book> BookList { get; set; } = null;
        public DbSet<Person> AuthorList { get; set; } = null;
        public ApplicationContext()  
        {
            /*Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationContext>());
            BookList = Set<Book>();
            AuthorList = Set<Person>();*/
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bookcat.db");
        }
    }
}
