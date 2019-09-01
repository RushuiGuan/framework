using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.UnitTest
{
    public interface IWorker { }
    public class Class1 : IWorker { }
    public class Class2 : IWorker { }
    public class Class3 : IWorker { }
    public class Class4 : IWorker { }
    public class Class5 : IWorker { }

    public class TestJob1{
        public IEnumerable<IWorker> Workers { get; }
        public TestJob1(IEnumerable<IWorker> workers)
        {
            this.Workers = workers;
        }
    }

    public class TestJob2
    {
        public IEnumerable<IWorker> Workers { get; }
        public TestJob2(IEnumerable<IWorker> workers)
        {
            this.Workers = workers;
        }
    }

    public class TestJob3
    {
        public IWorker Worker { get; set; }
        public TestJob3(IWorker worker)
        {
            this.Worker = worker;
        }
    }
}
