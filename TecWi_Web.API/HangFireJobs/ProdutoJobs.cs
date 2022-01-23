using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;

namespace TecWi_Web.API.HangFireJobs
{
    public class ProdutoJobs
    {
        private readonly IProdutoSincronizacaoService _produtoSincronizacaoService;

        public ProdutoJobs(IProdutoSincronizacaoService produtoSincronizacaoService)
        {
             _produtoSincronizacaoService = produtoSincronizacaoService;
        }

        public async Task SincronizarProdutosAsync()
        {
            await _produtoSincronizacaoService.Sincronizar();
        }

        public async Task EnviarProdutosMercocampAsync()
        {
            //Gerar os selects e fazer o json para integração.

           // await _produtoSincronizacaoService.Sincronizar();
        }
    }
}
