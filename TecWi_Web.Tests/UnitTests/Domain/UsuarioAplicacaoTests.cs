using System;
using FluentAssertions;
using NUnit.Framework;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Tests.UnitTests.Domain
{
    public class UsuarioAplicacaoTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(IdAplicacao.Cobranca, IdPerfil.Gestor)]
        public void CreateUsuarioAplicacao_WithValidParameters_ShouldCreateUsuarioAplicacao(IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            Action action = () => new UsuarioAplicacao(new Guid(), idAplicacao, idPerfil);
            action.Should().NotThrow<DomainValidadorException>();
        }
    }
}
