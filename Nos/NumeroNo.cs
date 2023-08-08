namespace hl.Nos
{
    public class NumeroNo : No
    {
        public NumeroNo(Trecho trecho, long numero) : base(trecho)
        {
            Numero = numero;
        }

        public long Numero { get; set; }

        
        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            arq.EmiteDefineA(amb.Bits, Numero);
        }
    }
}