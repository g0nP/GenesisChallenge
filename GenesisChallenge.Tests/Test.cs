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

        protected delegate void VoidFunction();
    }
}
