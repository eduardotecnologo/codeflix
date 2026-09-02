using System;
using System.Collections.Generic;
using System.Text;

namespace Codeflix.Catalogo.Domain.Exceptions
{
    internal class EntityValidationException : Exception
    {
        public EntityValidationException(string message) : base(message)
        {
        }
    }
}
