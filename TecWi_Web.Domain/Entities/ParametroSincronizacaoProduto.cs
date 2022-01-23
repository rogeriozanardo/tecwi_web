using System;
using System.ComponentModel.DataAnnotations;

namespace TecWi_Web.Domain.Entities
{
    public class ParametroSincronizacaoProduto
    {
        public int ID { get; set; }
        public DateTime DataHoraSincronizacao { get; set; }
        public DateTime UltimoUpdateDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
