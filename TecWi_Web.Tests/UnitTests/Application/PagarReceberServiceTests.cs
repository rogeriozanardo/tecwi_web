using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Application.Services;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Tests.UnitTests.Application
{
    
    class PagarReceberServiceTests
    {
        private PagarReceberService _pagarReceberService;

        [SetUp]
        public void Setup()
        {
            Mock<IMapper> _iMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _iUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IPagarReceberRepository> _iPagarReceberRepository = new Mock<IPagarReceberRepository>();
            Mock<IClienteRepository> _iClienteRepository = new Mock<IClienteRepository>();
            _pagarReceberService = new PagarReceberService(_iMapper.Object, _iUnitOfWork.Object, _iPagarReceberRepository.Object, _iClienteRepository.Object);
        }

        [Test]
        public void RandonlyAssingUsuarioToCliente_WithValidParameters_ReturnsEqualyDistributed(IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            List<Cliente> cliente = CreateClient();
            List<Usuario> usuario = CreateUsuario();
            _pagarReceberService.AssingUsuarioToClienteRandonly(cliente, usuario);
            Assert.IsTrue(cliente.Any(x => x.IdUsuario != Guid.Empty));
        }
        private List<Cliente> CreateClient()
        {
            List<Cliente> cliente = new List<Cliente>();
            for(int i = 1; i > 10; i++)
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

        private List<Usuario> CreateUsuario()
        {
            List<Usuario> usuario = new List<Usuario>();
            for (int i = 1; i > 10; i++)
            {
                usuario.Add(new Usuario(
                     new Guid(),
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
