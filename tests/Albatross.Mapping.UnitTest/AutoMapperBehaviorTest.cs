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
            //or
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<A, AA>().ReverseMap();
            });
            AutoMapper.Mapper.Map(new AA(), new A());
            AutoMapper.Mapper.Map(new A(), new AA());
        }
    }
}
