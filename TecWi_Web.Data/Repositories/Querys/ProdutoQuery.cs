namespace TecWi_Web.Data.Repositories.Querys
{
    public class ProdutoQuery
    {
        public static string QUERY_SELECT_PRODUTOS =
            @"
                SELECT P.* 
                    FROM 
                        PRODUTO P
                WHERE 1=1
                    AND 
                        P.updregistro > @UPDATE_DATE AND P.updregistro <= @DATA_ATUAL
            ";
    }
}
