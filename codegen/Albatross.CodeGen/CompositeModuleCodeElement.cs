using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen {
	public abstract class CompositeModuleCodeElement : List<IModuleCodeElement>, IModuleCodeElement {
		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get; set; } = string.Empty;

		public CompositeModuleCodeElement(string name, string module) {
			this.Name = name;
			this.Module = module;
		}
		public virtual void Build() { }

		protected T? SingleOrDefault<T>(string tag) where T : class, IModuleCodeElement
			=> this.Where(x => x is T && x.Tag == tag).Cast<T>().SingleOrDefault();

		protected T Single<T>(string tag) where T : class, IModuleCodeElement
			=> this.Where(x => x is T && x.Tag == tag).Cast<T>().Single();

		protected IEnumerable<T> Collection<T>(string tag) where T : class, IModuleCodeElement
			=> this.Where(x => x is T && x.Tag == tag).Cast<T>();

		protected void SetNullable<T>(T? element, string tag) where T : class, IModuleCodeElement {
			var index = this.FindIndex(x => x.Tag == tag && x is T);
			if (index >= 0) {
				if (element == null) {
					this.RemoveAt(index);
				} else {
					element.Tag = tag;
					this[index] = element;
				}
			} else {
				if (element != null) {
					element.Tag = tag;
					this.Add(element);
				}
			}
		}

		protected void Set<T>(T element, string tag) where T : class, IModuleCodeElement
			=> SetNullable(element, tag);

		public CompositeModuleCodeElement AddCodeElement<T>(T element, string tag) where T : class, IModuleCodeElement {
			element.Tag = tag;
			this.Add(element);
			return this;
		}
		public CompositeModuleCodeElement RemoveCodeElement<T>(T element, string tag) where T : class, IModuleCodeElement {
			var index = this.FindIndex(x => x.Tag == tag && x is T t && t == element);
			if (index >= 0) { this.RemoveAt(index); }
			return this;
		}
		public CompositeModuleCodeElement RemoveCodeElement<T>(string tag) where T : class, IModuleCodeElement {
			for (int i = this.Count - 1; i >= 0; i--) {
				if (this[i].Tag == tag && this[i] is T) {
					this.RemoveAt(i);
				}
			}
			return this;
		}
		public CompositeModuleCodeElement AddLine(IModuleCodeElement elem) {
			Add(new CodeLine(elem));
			return this;
		}
		public abstract TextWriter Generate(TextWriter writer);
	}

	public abstract class CompositeModuleCodeElement<T> : List<T>, IModuleCodeElement where T : class, IModuleCodeElement {
		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get; set; } = string.Empty;

		public CompositeModuleCodeElement(string name, string module, IEnumerable<T> items) : base(items) {
			this.Name = name;
			this.Module = module;
		}
		public virtual void Build() { }
		public abstract TextWriter Generate(TextWriter writer);

		IEnumerator<IModuleCodeElement> IEnumerable<IModuleCodeElement>.GetEnumerator() {
			List<T> list = this;
			return list.Select(x => x as IModuleCodeElement).GetEnumerator();
		}
	}
}