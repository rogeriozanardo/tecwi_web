using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Data.Repositories.Querys
{
    public class ClienteQuerys
    {
        public static string BuscaListaClienteTotalMatriz()
        {
            return @"select c.cdcliente as cdclifor, e.Inscrifed, e.Fantasia, e.Razao, e.DDD, e.Fone1, e.Fone2, e.Email, ltrim(rtrim(e.Cidade)) + ' - ' + e.uf as Cidade
                    from cliente c with(nolock) 

                    inner join empresa e with(nolock) on c.empresaid = e.empresaid
                    where isnumeric(c.cdcliente )=1";
        }

        public static string BuscaListaClienteTotalZ4()
        {
            return @"select * from cliente with(nolock)";
        }

        public static string BuscaProximoClienteNaFilaPorIdUsuario(Guid idUsuario)
        {
            return $@"select top 1 * from Cliente c with(nolock)

                    inner join pagarreceber pr with(nolock)  on pr.Cdclifor = c.Cdclifor

                    where IdUsuario = '{idUsuario.ToString()}'
                    and pr.Dtpagto is null
                    and DATEDIFF(DAY,pr.Dtvencto, getdate()) < 90

                    and c.Cdclifor not in(select Cdclifor from ContatoCobranca with(nolock) where DtAgenda > FORMAT(GETDATE(), 'yyyy-MM-dd 00:00:00'))

                    order by c.Razao";
        }
    }
}
