namespace hl.Nos
{
    public class BlocoNo : No
    {
        public BlocoNo(Trecho trecho) : base(trecho)
        {
        }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            CompilarAbaixo(amb, arq);
        }

    }
}