using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Data.Repositories.Querys
{
    public class AtendenteQuerys
    {
        public static string ListaPerformanceAtendentes(DateTime dtInicio, DateTime dtFim)
        {
  
            return $@"select cc.IdUsuario, u.Nome as NomeUsuario, c.Cdclifor, Razao, cc.IdContato, cc.DtContato, cc.TipoContato, cc.Anotacao

                     from ContatoCobranca cc with(nolock) 

                    inner join usuario u with(nolock) on u.IdUsuario = cc.IdUsuario
                    inner join cliente c with(nolock) on c.Cdclifor = cc.Cdclifor


                    where DtContato between '{dtInicio.ToString("yyyy-MM-dd 00:00:00")}' and '{dtFim.ToString("yyyy-MM-dd 23:59:00")}'";
        }
    }
}
