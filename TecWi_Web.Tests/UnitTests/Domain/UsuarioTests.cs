using System;
using FluentAssertions;
using NUnit.Framework;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Tests.UnitTests.Domain
{
    class UsuarioTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        [TestCase("Login", "Nome", "email@email", new byte[] { 0x20 }, new byte[] { 0x20 })]
        public void CreateUsuarioWithValidParameters(string login, string Nome, string eMail, byte[] senhaHash, byte[] senhaSalt)
        {
            Action action = () => new Usuario(Guid.NewGuid(), login, Nome, eMail, senhaHash, senhaSalt);
            action.Should().NotThrow<DomainValidadorException>();
        }

        [Test]
        [TestCase("", "Nome", "email@email", new byte[] { 0x20 }, new byte[] { 0x20 })]
        [TestCase("Login", "", "email@email", new byte[] { 0x20 }, new byte[] { 0x20 })]
        [TestCase("Login", "Nome", "", new byte[] { 0x20 }, new byte[] { 0x20 })]
        [TestCase("Login", "Nome", "email@email", new byte[0], new byte[] { 0x20 })]
        [TestCase("Login", "Nome", "email@email", new byte[] { 0x20 }, new byte[0])]
        public void CreateUsuarioWithInvalidParameters(string login, string Nome, string eMail, byte[] senhaHash, byte[] senhaSalt)
        {
            Action action = () => new Usuario(new Guid(), login, Nome, eMail, senhaHash, senhaSalt);
            action.Should().Throw<DomainValidadorException>();
        }
    }
}
