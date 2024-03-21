using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen {
	public interface ICompositeModuleCodeElement : IModuleCodeElement{
		IEnumerable<IModuleCodeElement> Nodes{ get; }
	}

	public abstract class CompositeModuleCodeElement : ICompositeModuleCodeElement {
		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get; set; } = string.Empty;
		public IEnumerable<IModuleCodeElement> Nodes => nodes;
		List<IModuleCodeElement> nodes = new List<IModuleCodeElement>();

		public CompositeModuleCodeElement(string name, string module) {
			this.Name = name;
			this.Module = module;
		}
		public virtual void Build() { }

		protected T? SingleOrDefault<T>(string tag) where T : class, IModuleCodeElement
			=> this.Nodes.Where(x => x is T && x.Tag == tag).Cast<T>().SingleOrDefault();

		protected T Single<T>(string tag) where T : class, IModuleCodeElement
			=> this.Nodes.Where(x => x is T && x.Tag == tag).Cast<T>().Single();

		protected IEnumerable<T> Collection<T>(string tag) where T : class, IModuleCodeElement
			=> this.Nodes.Where(x => x is T && x.Tag == tag).Cast<T>();

		protected void SetNullable<T>(T? element, string tag) where T : class, IModuleCodeElement {
			var index = this.nodes.FindIndex(x => x.Tag == tag && x is T);
			if (index >= 0) {
				if (element == null) {
					this.nodes.RemoveAt(index);
				} else {
					element.Tag = tag;
					this.nodes[index] = element;
				}
			} else {
				if (element != null) {
					element.Tag = tag;
					this.nodes.Add(element);
				}
			}
		}

		protected void Set<T>(T element, string tag) where T : class, IModuleCodeElement
			=> SetNullable(element, tag);

		protected CompositeModuleCodeElement AddCodeElement<T>(T element, string tag) where T : class, IModuleCodeElement {
			element.Tag = tag;
			this.nodes.Add(element);
			return this;
		}

		protected CompositeModuleCodeElement RemoveCodeElement<T>(T element, string tag) where T : class, IModuleCodeElement {
			var index = this.nodes.FindIndex(x => x.Tag == tag && x is T t && t == element);
			if (index >= 0) { this.nodes.RemoveAt(index); }
			return this;
		}
	
		protected CompositeModuleCodeElement RemoveCodeElement<T>(string tag) where T : class, IModuleCodeElement {
			for (int i = this.nodes.Count - 1; i >= 0; i--) {
				if (this.nodes[i].Tag == tag && this.nodes[i] is T) {
					this.nodes.RemoveAt(i);
				}
			}
			return this;
		}
	
		public CompositeModuleCodeElement AddLine(IModuleCodeElement elem) {
			nodes.Add(new CodeLine(elem));
			return this;
		}
		
		public abstract TextWriter Generate(TextWriter writer);
	}

	public abstract class CompositeModuleCodeElement<T> : ICompositeModuleCodeElement where T : class, IModuleCodeElement {
		List<T> nodes = new List<T>();
		public string Name { get; set; }
		public string Module { get; set; }
		public string Tag { get; set; } = string.Empty;

		public IEnumerable<IModuleCodeElement> Nodes => nodes;

		public CompositeModuleCodeElement(string name, string module, IEnumerable<T> items) {
			this.Name = name;
			this.Module = module;
			this.nodes.AddRange(items);
		}
		public virtual void Build() { }
		public abstract TextWriter Generate(TextWriter writer);
	}
}