using System;

namespace TecWi_Web.Domain.Entities
{
    public class LogSincronizacaoProdutoMercoCamp
    {
        public int ID { get; set; }
        public DateTime InicioSincronizacao { get; set; }
        public DateTime PeriodoInicialEnvio { get; set; }
        public DateTime PeriodoFinalEnvio { get; set; }
        public string JsonEnvio { get; set; }
    }
}
