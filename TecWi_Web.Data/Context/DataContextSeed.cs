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
                PasswordHashUtitlity.CreatePaswordHash("1234", out byte[] senhaHash, out byte[] senhaSalt);
                List<Usuario> usuario = new List<Usuario>()
                {
                    new Usuario(new Guid(), "admin", "admin", "email@email", senhaHash, senhaSalt),
                    new Usuario(new Guid(), "teste1", "teste1", "teste1@email", senhaHash, senhaSalt),
                    new Usuario(new Guid(), "teste2", "teste2", "teste2@email", senhaHash, senhaSalt)
                };

                _dataContext.Usuario.AddRange(usuario);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
