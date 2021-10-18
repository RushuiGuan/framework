using Albatross.CodeGen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen {
	public interface ICodeGenFactory {
		ICodeGenerator Get(Type type);
	}
	public class CodeGenFactory: ICodeGenFactory {
		Dictionary<Type, ICodeGenerator> dict = new Dictionary<Type, ICodeGenerator>();
		public CodeGenFactory(IEnumerable<ICodeGenerator> codeGenerators) {
			foreach(var item in codeGenerators) {
				dict[item.Type] = item;
			}
		}

		public ICodeGenerator Get(Type type) {
			if(dict.TryGetValue(type, out var codeGenerator)) {
				return codeGenerator;
			} else {
				throw new InvalidOperationException($"Code generator for {type.FullName} is not registered");
			}
		}
	}
}
