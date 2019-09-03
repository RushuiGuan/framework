using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.Core {
    public interface IConvertObject<From, To>:IConvertObject<From>
    {
        new To Convert(From from);
    }

    public interface IConvertObject<From> {
        object Convert(From from);
    }
}
