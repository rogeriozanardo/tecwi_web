namespace TecWi_Web.Shared.Filters
{
    public class PagarReceberFilter : FilterBase
    {
        public int SeqID { get; set; }
        public string Numlancto { get; set; }
        public int Sq { get; set; }
        public bool? Stcobranca { get; set; }
    }
}
