namespace hl.Nos
{
    public class SeAsmNo : No
    {
        public SeAsmNo(Trecho trecho, string comparacao, No sim, No? nao) : base(trecho)
        {
            Comparacao = comparacao;
            Abaixo.Add(sim);
            if(nao != null)Abaixo.Add(nao);

        }

        public string Comparacao { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            if
            (
                arq.Nome.ToLower() == Comparacao.ToLower() || 
                arq.NomeCompleto.ToLower() == Comparacao.ToLower()
            )
                PrimeiroAbaixo?.Compilar(amb, arq);
            else
                SegundoAbaixo?.Compilar(amb, arq);
        }

    }
}