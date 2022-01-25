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
            string dtAgenda = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            return $@"select * from 
                    (
                    select distinct null as DtUltimoContato, c.* from Cliente c with(nolock)

                    inner join pagarreceber pr with(nolock)  on pr.Cdclifor = c.Cdclifor

                    where c.IdUsuario = '{idUsuario.ToString()}'
                    and pr.Dtpagto is null
                    and DATEDIFF(DAY,pr.Dtvencto, getdate()) < 90

                    and c.Cdclifor not in(select Cdclifor from ContatoCobranca with(nolock) where DtAgenda >='{dtAgenda}')

					union all

					select distinct 
					(select max(cnt.DtContato) from ContatoCobranca cnt with(nolock) where cnt.Cdclifor = c.Cdclifor) as DtUltimoContato,
					c.* from Cliente c with(nolock)

					inner join ContatoCobranca cc with(nolock) on cc.Cdclifor = c.Cdclifor
					where cc.DtAgenda = '{dtAgenda}'
					and c.Cdclifor not in(select Cdclifor from ContatoCobranca  with(nolock) where DtAgenda >'{dtAgenda}')
					) x order by x.DtUltimoContato asc";
        }
    }
}
