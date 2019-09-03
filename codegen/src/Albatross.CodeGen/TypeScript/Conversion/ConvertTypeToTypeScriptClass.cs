using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Conversion
{
    public class ConvertTypeToTypeScriptClass: IConvertObject<Type, TypeScript.Model.Class>
    {
        ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
        public ConvertTypeToTypeScriptClass(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty) {
            this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
        }
        public TypeScript.Model.Class Convert(Type type)
        {
            return new Class
            {
                Name = type.Name,
                Export = true,
                Properties = from property in type.GetProperties() select convertPropertyInfoToTypeScriptProperty.Convert(property),
            };
        }

        object IConvertObject<Type>.Convert(Type from)
        {
            return this.Convert(from);
        }
    }
}
