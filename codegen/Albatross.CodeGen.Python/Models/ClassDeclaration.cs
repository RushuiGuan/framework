using Albatross.Collections;
using Albatross.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public class ClassDeclaration : ICodeElement, IComparable<ClassDeclaration> {
		public ClassDeclaration(string name) {
			Name = name;
		}

		public bool UseDataClass { get; set; }

		public Constructor? Constructor {
			get => SingleOrDefault<Constructor>(nameof(Constructor));
			set => SetNullable(value, nameof(Constructor));
		}

		public IEnumerable<ClassDeclaration> BaseClass => Collection<ClassDeclaration>(nameof(BaseClass));
		public ClassDeclaration AddBaseClass(ClassDeclaration @class) => (ClassDeclaration)AddCodeElement(@class, nameof(BaseClass));
		public ClassDeclaration RemoveBaseClass(ClassDeclaration @class) => (ClassDeclaration)RemoveCodeElement(@class, nameof(BaseClass));

		public IEnumerable<Method> Methods => Collection<Method>(nameof(Methods));
		public ClassDeclaration AddMethod(Method method) => (ClassDeclaration)AddCodeElement(method, nameof(Methods));
		public ClassDeclaration RemoveMethod(Method method) => (ClassDeclaration)RemoveCodeElement(method, nameof(Methods));

		public IEnumerable<Property> Properties => Collection<Property>(nameof(Properties));
		public ClassDeclaration AddProperty(Property property) => (ClassDeclaration)AddCodeElement(property, nameof(Properties));
		public ClassDeclaration RemoveProperty(Property property) => (ClassDeclaration)RemoveCodeElement(property, nameof(Properties));

		public IEnumerable<Field> Fields => Collection<Field>(nameof(Fields));
		public ClassDeclaration AddField(Field field) => (ClassDeclaration)AddCodeElement(field, nameof(Fields));
		public ClassDeclaration RemoveField(Field field) => (ClassDeclaration)RemoveCodeElement(field, nameof(Fields));

		public IEnumerable<Decorator> Decorators => Collection<Decorator>(nameof(Decorators));
		public ClassDeclaration AddDecorator(Decorator decorator) => (ClassDeclaration)AddCodeElement(decorator, nameof(Decorators));
		public ClassDeclaration RemoveDecorator(Decorator decorator) => (ClassDeclaration)RemoveCodeElement(decorator, nameof(Decorators));
		public bool IsEnum => BaseClass.Any(x => x.Name == My.Classes.EnumName);

		public string Name { get; }

		public override void Build() {
			if (UseDataClass) {
				AddDecorator(My.Decorators.DataClass());
				Fields.ForEach(x => x.Static = true);
			} else {
				Constructor = new Constructor();
				foreach (var field in Fields.Where(x => !x.Static)) {
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

		public int CompareTo(ClassDeclaration? other) {
			if (other == null) {
				return 1;
			} else {
				if (this.IsEnum != other.IsEnum) {
					return this.IsEnum ? -1 : 1;
				}
				if (this.BaseClass.Any(x => x.Name == other.Name)) {
					return 1;
				} else if (other.BaseClass.Any(x => x.Name == this.Name)) {
					return -1;
				}
				return Name.CompareTo(other.Name);
			}
		}
	}
}