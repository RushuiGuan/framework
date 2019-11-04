using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Mapping.UnitTest
{
    [TestFixture]
    public class AutoMapperBehaviorTest
    {
        public class A {
            public int Number1 { get; set; }
            public int Number2 { get; set; }
        }

        public class AA
        {
            public int Number1 { get; set; }
        }

        [Test]
        public void Run() {
			var config = new MapperConfiguration(cfg => cfg.CreateMap<A, AA>());
			var mapper = config.CreateMapper();
			mapper.Map(new AA(), new A());
			mapper.Map(new A(), new AA());
        }
    }
}
