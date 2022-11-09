using AjmeraAssignment.Models;
using Microsoft.EntityFrameworkCore;

namespace AjmeraAssignment.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){}

        public DbSet<Book> Books { get; set; }
    }
}
