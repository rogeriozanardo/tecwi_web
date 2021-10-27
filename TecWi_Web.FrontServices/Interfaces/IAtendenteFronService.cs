using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.FrontServices.Interfaces
{
    public interface IAtendenteFronService
    {
        Task<ServiceResponse<List<AtendenteDTO>>> ListaPerformanceAtendentes(PesquisaDTO pesquisaDTO);
    }
}
