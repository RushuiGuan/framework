using Albatross.Hosting.CommandLine;
using System.CommandLine;

namespace Albatross.Hosting.CommandLine {
	public class CommandOptionBuilder<T> {
		public Option<T> Option;

		public CommandOptionBuilder(string name) {
			this.Option = new Option<T>(name);
		}
		public CommandOptionBuilder<T> WithAlias(string alias) {
			this.Option.WithAlias(alias);
			return this;
		}

		public CommandOptionBuilder<T> WithDescription(string description) {
			this.Option.Description = description;
			return this;
		}

		public CommandOptionBuilder<T> IsRequired(bool isRequired = true) {
			this.Option.IsRequired = isRequired;
			return this;
		}

		public CommandOptionBuilder<T> IsHidden(bool isHidden = true) {
			this.Option.IsHidden = isHidden;
			return this;
		}
		public CommandOptionBuilder<T> AddCompletions(params string[] completions) {
			this.Option.AddCompletions(completions);
			return this;
		}
		public CommandOptionBuilder<T> FromAmong(params string[] values) {
			this.Option.FromAmong(values);
			return this;
		}
		public Option<T> AddTo(Command command, bool global = false) {
			if (global) {
				command.AddGlobalOption(this.Option);
			} else {
				command.Add(this.Option);
			}
			return this.Option;
		}
	}
}
