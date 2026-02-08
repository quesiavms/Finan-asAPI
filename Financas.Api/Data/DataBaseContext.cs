using Microsoft.EntityFrameworkCore;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // schema dbo as default
        modelBuilder.HasDefaultSchema("dbo");
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Entradas> Entradas { get; set; }
    public DbSet<Categorias> Categorias { get;set; }
    public DbSet<Pagamentos> Pagamentos { get;set; }
    public DbSet<Saidas> Saidas { get;set; }
    public DbSet<Parcelas> Parcelas { get;set; }
}
