using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IAtendenteService
    {
        Task<ServiceResponse<List<AtendenteDTO>>> ListaPerformanceAtendentes(PesquisaDTO pesquisaDTO);
    }
}
