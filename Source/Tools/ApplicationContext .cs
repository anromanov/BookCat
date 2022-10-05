using Microsoft.EntityFrameworkCore;
namespace BookCat

{
    class ApplicationContext : DbContext //Соединение с БД
    {
        public DbSet<Book> BookList { get; set; } = null; //список книг в БД
        public DbSet<Person> AuthorList { get; set; } = null; //список авторов в БД
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bookcat.db"); //Проверить подключение, создать БД если ещё не
        }
    }
}
