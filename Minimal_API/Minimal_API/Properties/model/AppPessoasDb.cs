using Microsoft.EntityFrameworkCore;

namespace Minimal_API.Properties;

public class AppPessoasDb : DbContext
{
    public AppPessoasDb(DbContextOptions<AppPessoasDb> options)
        : base(options){ }
    
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
}