using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen.Core {
	public static class Extension {

        public static TextWriter Run<T>(this TextWriter writer, ICodeGenerator<T> codeGenerator, T t) {
            codeGenerator.Run(writer, t);
            return writer;
        }

        public static CodeGeneratorScope BeginScope(this TextWriter writer, string text = null) {
            return new CodeGeneratorScope(writer, args => args.AppendLine($"{text} {{"), args => args.Append("}"));
        }


		public static string Proper(this string text) {
			if (!string.IsNullOrEmpty(text)) {
				string result = text.Substring(0, 1).ToUpper();
				if (text.Length > 1) {
					result = result + text.Substring(1);
				}
				return result;
			} else {
				return text;
			}
		}

		public static string VariableName(this string text) {
			if (!string.IsNullOrEmpty(text)) {
				string result = text.Substring(0, 1).ToLower();
				if (text.Length > 1) {
					result = result + text.Substring(1);
				}
				return result;
			} else {
				return text;
			}
		}

		public static void AddRange<T>(this HashSet<T> list, IEnumerable<T> items) {
			if (items != null) {
				foreach (var item in items) {
					list.Add(item);
				}
			}
		}

		public static IEnumerable<T> Merge<T, K>(this IEnumerable<T> src, IEnumerable<T> dst, Func<T, K> getKey) {
			Dictionary<K, T> dict = new Dictionary<K, T>();
			if(src != null) {
				foreach(var item in src) {
					dict.Add(getKey(item), item);
				}
			}
			if(dst != null) {
				foreach(var item in dst) {
					dict[getKey(item)] = item;
				}
			}
			return dict.Values;	
		}
	}
}
