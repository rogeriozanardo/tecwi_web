using System;

namespace TecWi_Web.Domain.Exceptions
{
    public class PedidoNaoEncontradoException : Exception
    {
        public PedidoNaoEncontradoException() : base() { }
        public PedidoNaoEncontradoException(string message) : base(message) { }
        public PedidoNaoEncontradoException(string message, Exception inner) : base(message, inner) { }
    }
}
