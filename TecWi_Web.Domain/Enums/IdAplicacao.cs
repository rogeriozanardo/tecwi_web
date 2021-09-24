using System.ComponentModel.DataAnnotations;

namespace TecWi_Web.Domain.Enums
{
    public enum IdAplicacao
    {
        [Display(Name = "Gestão de Cobrança")]
        Cobranca = 1,
        [Display(Name = "Gestão Comercial")]
        Comercial = 2,
        [Display(Name = "Gestão Financeira")]
        Financeiro = 3,
        [Display(Name = "Gestão de Logística")]
        Logistica = 4,
        [Display(Name = "Configurações")]
        Configuracoes =5
    }
}
