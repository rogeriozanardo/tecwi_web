using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Utility;

namespace TecWi_Web.Data.Context
{
    public class DataContextSeed
    {
        public async static Task SeedAsync(DataContext _dataContext)
        {
            if (!_dataContext.Usuario.Any())
            {
                PasswordHashUtitlity.CreatePaswordHash("Z4@BestDevs", out byte[] senhaHash, out byte[] senhaSalt);
                List<Usuario> usuario = new List<Usuario>()
                {
                    new Usuario(new Guid(), "admin", "admin", "email@email", senhaHash, senhaSalt)
                };

                _dataContext.Usuario.AddRange(usuario);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
