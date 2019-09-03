using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen.TypeScript.Conversion
{
    public class ConvertPropertyInfoToTypeScriptProperty : IConvertObject<PropertyInfo, Property>
    {
        ConvertTypeToTypeScriptType convertToTypeScriptType;
        public ConvertPropertyInfoToTypeScriptProperty(ConvertTypeToTypeScriptType convertToTypeScriptType) {
            this.convertToTypeScriptType = convertToTypeScriptType;
        }

        public Property Convert(PropertyInfo from)
        {
            return new Property
            {
                Name = from.Name.VariableName(),
                Type = convertToTypeScriptType.Convert(from.PropertyType),
            };
        }

        object IConvertObject<PropertyInfo>.Convert(PropertyInfo from)
        {
            return this.Convert(from);
        }
    }
}
