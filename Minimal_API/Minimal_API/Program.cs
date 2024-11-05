using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minimal_API.Properties;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do banco de dados em memória
            builder.Services.AddDbContext<AppPessoasDb>(options => options.UseInMemoryDatabase("AppPessoasDb"));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.MapPost("/pessoas", async (Pessoa pessoa, AppPessoasDb db) =>
            {
                db.Pessoas.Add(pessoa);
                await db.SaveChangesAsync();
                return Results.Created($"/pessoas/{pessoa.Id}", pessoa);
            });

            app.MapPost("/pessoa/{pessoaId}/enderecos", async (int pessoaId, Endereco endereco, AppPessoasDb db) =>
            {
                var pessoa = await db.Pessoas.FindAsync(pessoaId);
                if (pessoa == null) return Results.NotFound("Pessoa não encontrada.");
        
                endereco.PessoaId = pessoaId;
                db.Enderecos.Add(endereco);
                await db.SaveChangesAsync();
        
                return Results.Created($"/pessoas/{pessoaId}/enderecos/{endereco.Id}", endereco);
            });


            app.MapGet("/pessoas/{id}", async (int id, AppPessoasDb db) =>
            {
                var pessoa = await db.Pessoas
                    .Include(p => p.Enderecos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return pessoa != null ? Results.Ok(pessoa) : Results.NotFound("Pessoa não encontrada.");
            });

            app.MapGet("/pessoa/{idPessoa}/enderecos/{idEndereco}", async(int idPessoa, int idEndereco, AppPessoasDb db) =>
            {
                var endereco = await db.Enderecos
                    .FirstOrDefaultAsync(e => e.Id == idEndereco && e.PessoaId == idPessoa);

                return endereco != null ? Results.Ok(endereco) : Results.NotFound("Endereco não encontrado");

            });

            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
