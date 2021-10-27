using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Interfaces
{
    public interface IAtendenteRepository
    {
        Task<List<AtendimentoDetalhesDTO>> ListaAtendimentosDetalhados(PesquisaDTO pesquisaDTO);
    }
}
