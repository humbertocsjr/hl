namespace hl.Nos
{
    public class FuncaoNo : No
    {
        public bool Publica { get; set; } = false;
        public string Modulo => Trecho.Fonte.Nome.ToUpper();
        public string Nome { get; set; }
        public Tipo Retorno { get; set; }
        public bool RetornoPonteiro { get; set; }
        public List<DeclaraVariavelNo> Argumentos { get; set; } = new();

        public FuncaoNo(Trecho trecho, bool publica, string nome, Tipo ret, bool retPtr, List<DeclaraVariavelNo> args) : base(trecho)
        {
            Publica = publica;
            Nome = nome;
            Retorno = ret;
            RetornoPonteiro = retPtr;
            Argumentos = args;
        }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            if(Abaixo.Any())
            {
                amb.SeletorDeArquitetura = "";
                int nivel = amb.Nivel;
                List<DeclaraVariavelNo> vars = new();
                amb.Nivel = 1;
                amb.Locais = new List<DeclaraVariavelNo>();
                arq.EmiteFuncao(Publica, Modulo + "_" + Nome, TipoInfo.TipoParaBits(arq, Retorno, RetornoPonteiro));
                arq.EmiteSubtraiValorEmPtrPilha(SomaVariaveis(amb, arq, 0));
                foreach (var item in Argumentos)
                {
                    item.Inicializada = true;
                    item.Compilar(amb, arq);
                }
                CompilarAbaixo(amb, arq);
                arq.EmiteFimFuncao(Modulo + "_" + Nome, TipoInfo.TipoParaBits(arq, Retorno, RetornoPonteiro));
                amb.Locais = vars;
                amb.Nivel = nivel;
            }
            else
            {
                throw new NotImplementedException("Falta implementar declaracao externa, verificando se realmente não existe uma outra funcao implementada com este nome, se houver valida");
            }
        }

    }
}