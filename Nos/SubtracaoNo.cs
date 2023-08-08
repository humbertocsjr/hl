namespace hl.Nos
{
    public class SubtracaoNo : No
    {
        public SubtracaoNo(Trecho trecho) : base(trecho)
        {
        }
        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            foreach (var item in Abaixo)
            {
                item.Compilar(amb, arq);
                if(item != PrimeiroAbaixo)
                {
                    arq.EmiteCopiaAParaB(amb.Bits);
                    arq.EmiteDesempilhaA(amb.Bits);
                    arq.EmiteSubtraiBEmA(amb.Bits);
                }
                if(item != UltimoAbaixo) arq.EmiteEmpilhaA(amb.Bits);
            }
        }

        public override No OtimizaImplementa(Ambiente amb, Arquitetura arq)
        {
            NumeroNo? numeroNo = null;
            foreach (var item in Abaixo.ToList())
            {
                if(item is NumeroNo no)
                {
                    if(numeroNo == null)
                    {
                        numeroNo = no;
                    }
                    else
                    {
                        numeroNo.Numero -= no.Numero;
                        Abaixo.Remove(item);
                    }
                }
            }
            if(Abaixo.Count == 1 && PrimeiroAbaixo != null) return PrimeiroAbaixo;
            return this;
        }

    }
}