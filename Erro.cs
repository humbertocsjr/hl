namespace hl
{
    public class Erro : Exception
    {
        public Trecho Trecho { get; set; }
        public Erro(Trecho trecho, string msg) : base($"{trecho.Fonte.Endereco}:{trecho.Linha}:{trecho.Coluna}: Erro: {msg}")
        {
            Trecho = trecho;
        }
    }
}