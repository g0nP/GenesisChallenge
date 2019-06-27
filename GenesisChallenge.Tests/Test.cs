using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenesisChallenge.Tests
{
    public class Test
    {
        protected void ThenExceptionIsThrown<T>(VoidFunction method) where T : Exception
        {
            Assert.Throws<T>(() => method());
        }

        protected void ThenExceptionIsThrown<T>(VoidFunction method, string exceptionMessage) where T : Exception
        {
            var ex = Assert.Throws<T>(() => method());
            Assert.That(ex.Message, Is.EqualTo(exceptionMessage));
        }

        protected delegate void VoidFunction();
    }
}
