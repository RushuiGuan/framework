using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Model
{
    public class TypeScriptType
    {
        public TypeScriptType(string name) {
            this.Name = name;
        }
        public TypeScriptType() { }

        public string Name { get; set; }
        public bool IsArray { get; set; }

        public static TypeScriptType Date() => new TypeScriptType("Date");
        public static TypeScriptType String() => new TypeScriptType("string");
        public static TypeScriptType Boolean()  => new TypeScriptType("boolean");
        public static TypeScriptType Number() => new TypeScriptType("number");
        public static TypeScriptType Any() => new TypeScriptType("any");
        public static TypeScriptType Void() => new TypeScriptType("void");
    }
}
