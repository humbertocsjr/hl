namespace hl.Nos
{
    public class SeNaoNo : No
    {
        public SeNaoNo(Trecho trecho) : base(trecho)
        {
        }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            throw new NotImplementedException();
        }

    }
}