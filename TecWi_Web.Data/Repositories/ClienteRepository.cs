using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories.Querys;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _iConfiguration;

        public ClienteRepository(DataContext dataContext, IConfiguration iConfiguration)
        {
            _dataContext = dataContext;
            _iConfiguration = iConfiguration;
        }

        public async Task<bool> BulkInsertAsync(List<Cliente> cliente)
        {
            await _dataContext.AddRangeAsync(cliente);

            return true;
        }

        public async Task<bool> BulkUpdateAsync(List<Cliente> cliente)
        {
            cliente.ForEach(x => x.Usuario = null);
            await Task.Run(() =>
            {
                _dataContext.UpdateRange(cliente);
            });

            return true;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            await Task.Run(() =>
            {
                _dataContext.Update(cliente);
                
            });

            return true;
        }

        public async Task<List<Cliente>> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilter)
        {
            IQueryable<Cliente> _cliente = _dataContext.Cliente
                .Include(x => x.Usuario)
                .Include(x => x.PagarReceber)
                .Include(x => x.ClienteContato)
                .Include(x => x.ContatoCobranca).ThenInclude(x => x.Usuario).ThenInclude(x => x.ContatoCobrancaLancamento)
                .Where(x => clientePagarReceberFilter.IdUsuario != Guid.Empty ? x.PagarReceber.Any(y => y.Stcobranca && y.Dtpagto == null) : true)
                .Where(x => clientePagarReceberFilter.IdUsuario != Guid.Empty ? (x.IdUsuario == clientePagarReceberFilter.IdUsuario || x.ContatoCobranca.Any(y => y.DtAgenda.Date <= DateTime.Now.Date)) : true)
                .Where(x => clientePagarReceberFilter.cdclifor != null ? x.Cdclifor == clientePagarReceberFilter.cdclifor : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.fantasia) && !string.IsNullOrWhiteSpace(clientePagarReceberFilter.razao) ? (x.Fantasia.Contains(clientePagarReceberFilter.fantasia) || x.Razao.Contains(clientePagarReceberFilter.razao)) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.fantasia) && string.IsNullOrWhiteSpace(clientePagarReceberFilter.razao) ? (x.Fantasia.Contains(clientePagarReceberFilter.fantasia)) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.razao) && string.IsNullOrWhiteSpace(clientePagarReceberFilter.fantasia) ? (x.Razao.Contains(clientePagarReceberFilter.razao)) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.inscrifed) ? x.Inscrifed.Contains(clientePagarReceberFilter.inscrifed) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.numlancto) ? x.PagarReceber.Where(y => y.Numlancto.Contains(clientePagarReceberFilter.numlancto)).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtemissaoStart != null ? x.PagarReceber.Where(y => y.Dtemissao >= (DateTime)clientePagarReceberFilter.dtemissaoStart).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtemissaoEnd != null ? x.PagarReceber.Where(y => y.Dtemissao <= (DateTime)clientePagarReceberFilter.dtemissaoEnd).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtvenctoStart != null ? x.PagarReceber.Where(y => y.Dtvencto >= (DateTime)clientePagarReceberFilter.dtvenctoStart).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtvenctoEnd != null ? x.PagarReceber.Where(y => y.Dtvencto <= (DateTime)clientePagarReceberFilter.dtvenctoEnd).ToList().Count > 0 : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.NotasFiscais) ? x.PagarReceber.Where(y => y.NotasFiscais.Contains(clientePagarReceberFilter.NotasFiscais)).ToList().Count > 0 : true);

            List<Cliente> cliente = await _cliente
            .AsNoTracking()
            .ToListAsync();

            return cliente;
        }

        public async Task<Cliente> GetNextInQueueAsync(ClientePagarReceberFilter clientePagarReceberFilter)
        {
            Cliente cliente = new Cliente();
            
            try
            {
                string stringConexao = _iConfiguration.GetConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(stringConexao))
                {
                    connection.Open();
                    string query = ClienteQuerys.BuscaProximoClienteNaFilaPorIdUsuario(clientePagarReceberFilter.IdUsuario);
                    var result = await connection.QueryAsync<Cliente>(query);
                    cliente = result.FirstOrDefault();
                    connection.Close();
                }
            }
            catch(Exception e)
            {
                cliente = null;
            }

            if(cliente == null)
            {
                return cliente;
            }

            cliente.PagarReceber = _dataContext.PagarReceber.Where(p => p.Cdclifor == cliente.Cdclifor && p.Stcobranca == true && p.Dtpagto == null && p.Dtvencto >= DateTime.Now.AddDays(-90)).ToList();
            cliente.ClienteContato = _dataContext.ClienteContato.Where(x => x.Cdclifor == cliente.Cdclifor).ToList();

            //IQueryable<Cliente> _cliente = _dataContext.Cliente
            //   .Include(x => x.PagarReceber)
            //   .Include(x => x.ClienteContato)
            //   .Include(x => x.ContatoCobranca).ThenInclude(x => x.ContatoCobrancaLancamento)
            //   .Where(x => x.IdUsuario == clientePagarReceberFilter.IdUsuario)
            //   .Where(x => x.PagarReceber.Any(y => y.Stcobranca && y.Dtpagto == null))
            //   .Where(x => x.ContatoCobranca == null || x.ContatoCobranca.Any(y => y.DtAgenda.Date <= DateTime.Now.Date));

            //Cliente cliente = await _cliente.OrderBy(x => x.ContatoCobranca.Count).ThenBy(x => x.ContatoCobranca.OrderBy(x => x.DtAgenda).FirstOrDefault()).FirstOrDefaultAsync();
            return cliente;
        }

        public async Task<List<Cliente>> BuscaListaClienteTotalZ4()
        {
            List<Cliente> clientes = await _dataContext.Cliente.ToListAsync();

            return clientes;
        }

        public async Task<bool> InsereCliente(List<Cliente> cliente)
        {
            bool retorno = true;
            try
            {
                foreach(var item in cliente)
                {
                    Cliente registro = await _dataContext.Cliente.Where(x => x.Cdclifor == item.Cdclifor).FirstOrDefaultAsync();
                    if(registro == null)
                    {
                        await _dataContext.Cliente.AddAsync(item);
                        await _dataContext.SaveChangesAsync();
                    }
                }
            }catch
            {
                retorno = false;
            }
            return retorno;
        }
    }
}
