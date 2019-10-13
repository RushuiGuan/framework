using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model
{
    public class Class
    {
        public bool Export { get; set; }
        public string Name { get; set; }

        public IEnumerable<Property> Properties { get; set; }
    }
}
