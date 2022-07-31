using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.Entities;

namespace RToora.DemoApi.Web.DB;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {

    }

    public DbSet<TodoItem> TodoItems { get; set; }
}
