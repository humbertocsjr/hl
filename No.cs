using hl.Nos;

namespace hl
{
    public abstract class No
    {
        public Trecho Trecho { get; set; }

        public No? Acima {get; set;}

        public List<No> Abaixo { get; set; } = new List<No>();

        public No? PrimeiroAbaixo => Abaixo.Count > 0 ? Abaixo[0] : null;
        public No? SegundoAbaixo => Abaixo.Count > 1 ? Abaixo[1] : null;
        public No? TerceiroAbaixo => Abaixo.Count > 2 ? Abaixo[2] : null;
        public No? QuartoAbaixo => Abaixo.Count > 3 ? Abaixo[4] : null;
        public No? UltimoAbaixo => Abaixo.Count > 0 ? Abaixo.Last() : null;

        protected No(Trecho trecho)
        {
            Trecho = trecho;
        }

        public void Compilar(Ambiente amb, Arquitetura arq)
        {
            arq.EmiteComentario($"{Trecho.Fonte.Endereco}:{Trecho.Linha}:{Trecho.Coluna}: {this.GetType().Name}");
            CompilarImplementa(amb, arq);
        }

        protected abstract void CompilarImplementa(Ambiente amb, Arquitetura arq);

        protected void CompilarAbaixo(Ambiente amb, Arquitetura arq)
        {
            foreach (var item in Abaixo)
            {
                item.Compilar(amb, arq);
            }
        }

        public No Otimiza(Ambiente amb, Arquitetura arq)
        {
            List<No> nos = new();
            foreach (var item in Abaixo)
            {
                var no = item.Otimiza(amb, arq);
                if(no != null) nos.Add(no);
            }
            Abaixo = nos;
            return OtimizaImplementa(amb, arq);
        }

        public virtual No OtimizaImplementa(Ambiente amb, Arquitetura arq)
        {
            return this;
        }

        public void BuscarReferencias(Ambiente amb, Arquitetura arq)
        {
            var publicas = from f in amb.Funcoes where f.Publica && !amb.FuncoesUsadas.Contains(f) select f;
            foreach (var item in publicas)
            {
                amb.FuncoesUsadas.Add(item);
                item.BuscarReferencias(amb, arq);
            }
            if(this is FuncaoNo func && !amb.FuncoesUsadas.Contains(func)) amb.FuncoesUsadas.Add(func);
            if(this is ChamarFuncNo no)
            {
                var cons = from f in amb.Funcoes where f.Modulo == no.Modulo && f.Nome == no.Nome select f;
                if(cons.Any() && !amb.FuncoesUsadas.Contains(cons.First()))
                {
                    amb.FuncoesUsadas.Add(cons.First());
                    cons.First().BuscarReferencias(amb, arq);
                }
            }
            foreach (var item in Abaixo)
            {
                item.BuscarReferencias(amb, arq);
            }
        }

        public int SomaVariaveis(Ambiente amb, Arquitetura arq, int atual)
        {
            int ret = 0;
            if(this is DeclaraVariavelNo no)
            {
                ret += (int)no.CalculaBits(amb, arq);
                no.DesvioPtrBase = -(atual + ret);
            }
            if(this is FuncaoNo funcao)
            {
                int args = arq.PadraoDesvioArgumentos();
                foreach (var item in funcao.Argumentos)
                {
                    item.DesvioPtrBase = args;
                    args += (int)item.CalculaBits(amb, arq);
                }
            }
            foreach (var item in Abaixo)
            {
                ret += item.SomaVariaveis(amb, arq, atual + ret);
            }
            return ret;
        }
    }
}