namespace hl.Nos
{
    public class ChamarFuncNo : No
    {
        public ChamarFuncNo(Trecho trecho, string modulo, string nome, List<No> args) : base(trecho)
        {
            Modulo = modulo;
            Nome = nome;
            Abaixo.AddRange(args);
        }

        public string Modulo { get; set; }
        public string Nome { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            var cons = from f in amb.Funcoes where f.Nome == Nome && f.Modulo == Modulo select f;
            if(!cons.Any())
                cons = from f in amb.FuncoesDeclaradas where f.Nome == Nome && f.Modulo == Modulo select f;
            if(!cons.Any()) throw new Erro(Trecho, $"Função {Modulo}.{Nome} não encontrada.");
            List<No> parametros = new (Abaixo);
            parametros.Reverse();
            List<DeclaraVariavelNo> argumentos = new (cons.First().Argumentos);
            if(argumentos.Any() && argumentos.Last().Tipo == Tipo.ParametrosVariaveis)
            {
                var trecho = argumentos.Last().Trecho;
                argumentos.Remove(argumentos.Last());
                for (int i = 0; i < (parametros.Count - argumentos.Count); i++)
                {
                    argumentos.Add(new DeclaraVariavelNo(trecho, $"__PARAM{i}__", Tipo.ParametrosVariaveis, false, 0, false));
                }
            }
            argumentos.Reverse();
            if(parametros.Count != argumentos.Count) throw new Erro(Trecho, $"Quantidade de argumentos incompatível");
            for (int i = 0; i < argumentos.Count; i++)
            {
                var arg = argumentos[i];
                var param = parametros[i];
                Bits anteriorBits = amb.Bits;
                bool anteriorSinal = amb.Sinal;
                bool anteriorParVar = amb.CampoParametroVariavel;
                amb.Bits = arg.CalculaBits(amb, arq);
                amb.Sinal =arg.CalculaSinal(amb, arq);
                amb.CampoParametroVariavel = arg.Tipo == Tipo.ParametrosVariaveis;
                param.Compilar(amb, arq);
                arq.EmiteEmpilhaA(amb.Bits);
                amb.Bits = anteriorBits;
                amb.Sinal = anteriorSinal;
                amb.CampoParametroVariavel = anteriorParVar;
            }
            arq.EmiteChamar(Modulo + "_" + Nome);
        }

    }
}