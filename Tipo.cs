namespace hl
{
    public enum Tipo
    {
        Desconhecido,
        String,
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Struct,
        ParametrosVariaveis
    }

    public class TipoInfo
    {
        public static Bits TipoParaBits(Arquitetura arq, Tipo tipo, bool ponteiro)
        {
            if(ponteiro)
            {
                return arq.BitsPonteiro();
            }
            else
            {
                return arq.BitsTipo(tipo);
            }
        }
    }
}