using Albatross.Collections;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class Class : CompositeModuleCodeElement, IComparable<Class> {
		public Class(string name) : base(name, string.Empty) { }
		public Class(string name, string module) : base(name, module) { }

		public bool UseDataClass { get; set; }
		
		public Constructor? Constructor {
			get => SingleOrDefault<Constructor>(nameof(Constructor));
			set => SetNullable(value, nameof(Constructor));
		}
		
		public IEnumerable<Class> BaseClass => Collection<Class>(nameof(BaseClass));
		public void AddBaseClass(Class @class) => AddCodeElement(@class, nameof(BaseClass));
		public void RemoveBaseClass(Class @class) => RemoveCodeElement(@class, nameof(BaseClass));

		public IEnumerable<Method> Methods => Collection<Method>(nameof(Methods));
		public void AddMethod(Method method) => AddCodeElement(method, nameof(Methods));
		public void RemoveMethod(Method method) => RemoveCodeElement(method, nameof(Methods));

		public IEnumerable<Property> Properties => Collection<Property>(nameof(Properties));
		public void AddProperty(Property property) => AddCodeElement(property, nameof(Properties));
		public void RemoveProperty(Property property) => RemoveCodeElement(property, nameof(Properties));

		public IEnumerable<Field> Fields => Collection<Field>(nameof(Fields));
		public void AddField(Field field) => AddCodeElement(field, nameof(Fields));
		public void RemoveField(Field field) => RemoveCodeElement(field, nameof(Fields));

		public IEnumerable<Decorator> Decorators => Collection<Decorator>(nameof(Decorators));
		public void AddDecorator(Decorator decorator) => AddCodeElement(decorator, nameof(Decorators));
		public void RemoveDecorator(Decorator decorator) => RemoveCodeElement(decorator, nameof(Decorators));
		public bool IsEnum => BaseClass.Any(x => x.Name == My.Classes.EnumName);

		public override void Build() {
			if (UseDataClass) {
				AddDecorator(My.Decorators.DataClass());
				Fields.ForEach(x => x.Static = true);
			} else {
				Constructor = new Constructor();
				foreach(var field in Fields.Where(x=>!x.Static)) {
					Constructor.InitFields.Add(field);
				}
			}
			base.Build();
		}
		public override TextWriter Generate(TextWriter writer) {
			this.Decorators.ForEach(item => writer.Code(item));
			writer.AppendLine().Append("class ").Append(Name);
			if (BaseClass.Any()) {
				writer.OpenParenthesis().WriteItems(BaseClass.Select(x => x.Name), ", ").CloseParenthesis();
			}
			using (var scope = writer.BeginPythonScope()) {
				foreach (var field in Fields.Where(x => x.Static)) {
					scope.Writer.Code(field);
				}
				Constructor?.Generate(scope.Writer);
				foreach (var item in Properties) {
					scope.Writer.Code(item);
				}
				foreach (var item in Methods) {
					scope.Writer.Code(item);
				}
			}
			return writer;
		}

		public int CompareTo(Class? other) {
			if (other == null) {
				return 1;
			} else {
				if(this.IsEnum != other.IsEnum) {
					return this.IsEnum ? -1 : 1;
				}
				if(this.BaseClass.Any(x=>x.Name == other.Name)) {
					return 1;
				}else if(other.BaseClass.Any(x=>x.Name == this.Name)) {
					return -1;
				}
				return Name.CompareTo(other.Name);
			}
		}
	}
}