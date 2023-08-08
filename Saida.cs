namespace hl
{
    public class Saida
    {
        StreamWriter _gravador;
        public string Endereco { get; set; }
        public Saida(string endereco)
        {
            Endereco = endereco;
            _gravador = new StreamWriter(new FileStream(Endereco, FileMode.Create));
        }

        public void Escrever(string texto, params object[] args)
        {
            _gravador.Write(String.Format(texto, args));
        }

        public void EscreverLinha(string texto, params object[] args)
        {
            _gravador.WriteLine(String.Format(texto, args));
        }

        public void Fechar()
        {
            _gravador.Flush();
            _gravador.Close();
        }
    }
}