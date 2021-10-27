using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories.Querys;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Repositories
{
    public class AtendenteRepository : IAtendenteRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _iConfiguration;

        public AtendenteRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _iConfiguration = configuration;
        }

        public async Task<List<AtendimentoDetalhesDTO>> ListaAtendimentosDetalhados(PesquisaDTO pesquisaDTO)
        {
            List<AtendimentoDetalhesDTO> atendimentoDetalhesDTO = new List<AtendimentoDetalhesDTO>();
            
            try
            {
                string stringConexao = _iConfiguration.GetConnectionString("DefaultConnection");
                using(var connection = new SqlConnection(stringConexao))
                {
                    connection.Open();
                    string query = AtendenteQuerys.ListaPerformanceAtendentes(pesquisaDTO.DtInicio, pesquisaDTO.DtFim);
                    atendimentoDetalhesDTO = (await connection.QueryAsync<AtendimentoDetalhesDTO>(query)).ToList();
                    connection.Close();
                }

            }
            catch(Exception e)
            {

            }
            return atendimentoDetalhesDTO;
        }
    }
}
