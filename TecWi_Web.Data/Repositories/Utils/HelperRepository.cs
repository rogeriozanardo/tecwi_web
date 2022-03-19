using Microsoft.Extensions.Configuration;

namespace TecWi_Web.Data.Repositories.Utils
{
    public abstract class HelperRepository
    {
        private readonly IConfiguration _Configuration;

        public HelperRepository(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        protected string BuscarConnectionStringFilial01()
        {
            return _Configuration.GetConnectionString("DbTecwiFilial01");
        }

        protected string BuscarConnectionStringFilial02()
        {
            return _Configuration.GetConnectionString("DbTecwiFilial02");
        }

        protected string BuscarConnectionStringMatriz()
        {
            return _Configuration.GetConnectionString("DbTecWiMatriz");
        }

        protected string RetornarCNPJPorFilial(string filial)
        {
            filial = filial ?? string.Empty;

            filial = filial.Trim().ToUpper();

            switch (filial)
            {
                case "BA":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Filial01").Value;
                case "ES":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Filial02").Value;
                case "SP":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Matriz").Value;
                case "HOMOLOGACAO":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_HOMOLOGACAO_TESTE").Value;
                default:
                    break;
            }


            return string.Empty;
        }

    }
}
