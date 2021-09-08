using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using TecWi_Web.Application.Services;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Tests.UnitTests.Application
{
    public class PagarReceberServiceTests
    {
        private PagarReceberService _pagarReceberService;

        [SetUp]
        public void Setup()
        {
            Mock<IMapper> _iMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _iUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IPagarReceberRepository> _iPagarReceberRepository = new Mock<IPagarReceberRepository>();
            Mock<IClienteRepository> _iClienteRepository = new Mock<IClienteRepository>();
            Mock<IUsuarioRepository> _iUsuarioRepository = new Mock<IUsuarioRepository>();
            _pagarReceberService = new PagarReceberService(_iMapper.Object, _iUnitOfWork.Object, _iPagarReceberRepository.Object, _iClienteRepository.Object, _iUsuarioRepository.Object);
        }

        [Test]
        [TestCase(12, 4)]
        public void RandonlyAssingUsuarioToCliente_WithValidParameters_ReturnsEqualyDistributed(int quantityCliente, int quantityUsuario)
        {
            List<Cliente> cliente = CreateClient(quantityCliente);
            List<Usuario> usuario = CreateUsuario(quantityUsuario);
            _pagarReceberService.AssingUsuarioToClienteRandonly(cliente, usuario);
            int clienteGroupPerUsuario = cliente.GroupBy(x => new { x.IdUsuario }).ToList().Count;
            
            Assert.IsTrue(clienteGroupPerUsuario == quantityUsuario);
        }

        private List<Cliente> CreateClient(int quantity)
        {
            List<Cliente> cliente = new List<Cliente>();
            for (int i = 1; i <= quantity; i++)
            {
                cliente.Add(new Cliente(
                     i,
                        $"inscrifed - {i}",
                        $"Fantasia - {i}",
                        $"Razao - {i}",
                        $"DDD - {i}",
                        $"Fone1 - {i}",
                        $"Fone2 - {i}",
                        $"Email{i}@email.com",
                        $"Cidade - {i}"
                    ));
            }

            return cliente;
        }

        private List<Usuario> CreateUsuario(int quantity)
        {
            List<Usuario> usuario = new List<Usuario>();
            for (int i = 1; i <= quantity; i++)
            {
                usuario.Add(new Usuario(
                     Guid.NewGuid(),
                     $"Login - {i}",
                     $"Nome - {i}",
                     $"Email{i}@email.com",
                     new byte[] { 0x20 },
                     new byte[] { 0x20 }
                    ));
            }

            return usuario;
        }
    }
}
