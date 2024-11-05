namespace Minimal_API.Properties;

public class Pessoa
{
    public int Id { get; set; }
    public String Nome { get; set; }
    public int Idade { get; set; }
    public String email { get; set; }
    public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
}