using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace HelpDeskTeamProject.Tests
{
    [TestFixture]
    class FirstTest
    {
        [Test]
        public void ShouldPass()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
