using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Data.Repositories.Querys
{
    public class PagarReceberQuerys
    {
        public static string BuscaListaPagarReceberSymphony()
        {
            return @"select p.SeqID,  ltrim(rtrim(p.numlancto)) as numlancto, p.sq, p.cdfilial, p.dtemissao, p.dtvencto, p.dtpagto, p.valorr, string_agg(nf.NumNota, ', ') as NotasFiscais,  
            p.cdclifor, e.inscrifed,  e.fantasia, e.razao, e.ddd, e.fone1, e.fone2, e.email, trim(e.cidade)+ ' - ' + e.uf as cidade from 
            PagarReceber p with(nolock)
            left join ntfiscal nf with(nolock) on nf.NumPedido=p.numpedido
            inner join cliente c with(nolock) on c.cdcliente = p.cdclifor
            inner join empresa e with(nolock) on e.empresaid = c.empresaid
            where p.tppagrec='R'
			and p.stprevisao='R'
			and p.dtpagto is null
            and p.numtransf is null
            and p.tpdocquit = '4'
            and p.dtvencto < getdate()-2
			and isnumeric(p.cdclifor) = 1
            group by p.SeqID, p.numlancto, p.sq, p.cdfilial, p.dtemissao, p.dtvencto, p.dtpagto, p.valorr, p.cdclifor, e.inscrifed, e.fantasia, e.razao, e.ddd,
            e.fone1, e.fone2, e.email, e.cidade, e.uf";
        }
    }
}
