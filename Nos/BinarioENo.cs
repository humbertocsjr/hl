namespace hl.Nos
{
    public class BinarioENo : No
    {
        public BinarioENo(Trecho trecho) : base(trecho)
        {
        }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmiteEmpilhaA(amb.Bits);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteCopiaAParaB(amb.Bits);
            arq.EmiteDesempilhaA(amb.Bits);
            arq.EmiteEBEmA(amb.Bits);
        }

    }
}