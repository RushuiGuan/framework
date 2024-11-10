using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Albatross.SemVer {
	/// <summary>
	/// Simplified sematic version 2.0
	/// Only allows a label and a revision number for pre-releases
	/// </summary>
	public sealed record class SematicVersion : IComparable<SematicVersion> {
		public SematicVersion() { }
		public SematicVersion(string input) {
			if (string.IsNullOrWhiteSpace(input)) { throw new EmptyIdentifierException(); }
			int hyphenIndex = input.IndexOf(Hyphen);
			int plusIndex = input.IndexOf(Plus);

			if (//plus has to be after hyphen
				plusIndex != -1 && hyphenIndex > plusIndex
				//not next to each other
				|| hyphenIndex + 1 == plusIndex
				//not at the beginning or at the end
				|| plusIndex == input.Length - 1
				|| hyphenIndex == input.Length - 1
				|| hyphenIndex == 0
				|| plusIndex == 0) {
				throw new FormatException();
			}

			string versionText;
			string? prereleaseText = null;
			string? metadataText = null;


			if (hyphenIndex == -1) {
				if (plusIndex == -1) {
					versionText = input;
				} else {
					versionText = input.Substring(0, plusIndex);
				}
			} else {
				versionText = input.Substring(0, hyphenIndex);
				if (plusIndex == -1) {
					prereleaseText = input.Substring(hyphenIndex + 1);
				} else {
					prereleaseText = input.Substring(hyphenIndex + 1, plusIndex - hyphenIndex - 1);
				}
			}
			if (plusIndex != -1) {
				metadataText = input.Substring(plusIndex + 1);
			}
			ParseVersion(versionText, out var major, out var minor, out var patch);
			if (prereleaseText != null) {
				PreRelease = prereleaseText.Split(Dot);
			}
			if (metadataText != null) {
				Metadata = metadataText.Split(Dot);
			}
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			Validate();
		}

		public const char Dot = '.';
		public const char Hyphen = '-';
		public const char Plus = '+';

		const string AlphanumericsPattern = "^[0-9A-Za-z-]+$";
		public static readonly Regex AlphaNumericRegex = new Regex(AlphanumericsPattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		const string LeadingZeroNumericPattern = "^0[0-9]+$";
		public static readonly Regex LeadingZeroNumericRegex = new Regex(LeadingZeroNumericPattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		const string NonLeadingZeroNumericPattern = "^(0|[1-9][0-9]*)$";
		public static readonly Regex NonLeadingZeroNumericRegex = new Regex(NonLeadingZeroNumericPattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);


		public int Major { get; init; }
		public int Minor { get; init; }
		public int Patch { get; init; }

		/// <summary>
		/// Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]. Identifiers MUST NOT be empty. Numeric identifiers MUST NOT include leading zeroes.
		/// </summary>
		public IEnumerable<string> PreRelease { get; init; } = new string[0];
		/// <summary>
		/// Identifiers MUST comprise only ASCII alphanumerics and hyphen [0-9A-Za-z-]. Identifiers MUST NOT be empty.
		/// </summary>
		public IEnumerable<string> Metadata { get; init; } = new string[0];

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append(Major).Append(Dot).Append(Minor).Append(Dot).Append(Patch);
			if (PreRelease?.Count() > 0) {
				bool first = true;
				foreach (var item in PreRelease) {
					if (first) {
						sb.Append(Hyphen);
						first = false;
					} else {
						sb.Append(Dot);
					}
					sb.Append(item);
				}
			}
			if (Metadata?.Count() > 0) {
				bool first = true;
				foreach (var item in Metadata) {
					if (first) {
						sb.Append(Plus);
						first = false;
					} else {
						sb.Append(Dot);
					}
					sb.Append(item);
				}
			}
			return sb.ToString();
		}
		public bool IsRelease => Major != 0 && !HasPreRelease;
		public bool HasPreRelease => PreRelease?.Count() > 0;
		/// <summary>
		/// Validate the prerelease and metadata format
		/// </summary>
		public void Validate() {
			if (PreRelease != null) {
				foreach (var item in PreRelease) {
					if (string.IsNullOrWhiteSpace(item)) {
						throw new EmptyIdentifierException();
					} else if (!AlphaNumericRegex.IsMatch(item)) {
						throw new FormatException();
					} else if (LeadingZeroNumericRegex.IsMatch(item)) {
						throw new LeadingZeroException();
					}
				}
			}
			if (Metadata != null) {
				foreach (var item in Metadata) {
					if (string.IsNullOrWhiteSpace(item)) {
						throw new EmptyIdentifierException();
					} else if (!AlphaNumericRegex.IsMatch(item)) {
						throw new FormatException();
					}
				}
			}
		}

		public int CompareTo(SematicVersion sematicVersion) {
			if (sematicVersion != null) {
				int result = Major.CompareTo(sematicVersion.Major);
				if (result == 0) {
					result = Minor.CompareTo(sematicVersion.Minor);
					if (result == 0) {
						result = Patch.CompareTo(sematicVersion.Patch);
						if (result == 0) {
							if (HasPreRelease == false && sematicVersion.HasPreRelease == false) {
								result = 0;
							} else if (HasPreRelease && !sematicVersion.HasPreRelease) {
								result = -1;
							} else if (!HasPreRelease && sematicVersion.HasPreRelease) {
								result = 1;
							} else {
								string[] a = PreRelease.ToArray();
								string[] b = sematicVersion.PreRelease.ToArray();
								for (int i = 0; i < a.Length && i < b.Length; i++) {
									result = a[i].CompareTo(b[i]);
									if (result != 0) { break; }
								}
								if (result == 0) {
									if (a.Length > b.Length) {
										result = 1;
									} else if (a.Length < b.Length) {
										result = -1;
									}
								}
							}
						}
					}
				}
				return result;
			} else {
				throw new ArgumentNullException();
			}
		}

		static void ParseVersion(string text, out int major, out int minor, out int patch) {
			string[] list = text.Split(Dot);
			if (list.Length != 3) {
				throw new FormatException();
			}
			foreach (string item in list) {
				if (!NonLeadingZeroNumericRegex.IsMatch(item)) {
					throw new LeadingZeroException();
				}
			}
			major = int.Parse(list[0]);
			minor = int.Parse(list[1]);
			patch = int.Parse(list[2]);
		}

		public SematicVersion NextRelease(ReleaseType type) {
			if (!IsRelease) {
				if (Major == 0) {
					return this with {
						Major = 1,
						Minor = 0,
						Patch = 0,
						PreRelease = new string[0]
					};
				} else {
					return this with {
						PreRelease = new string[0]
					};
				}
			} else if (type == ReleaseType.Major) {
				return this with {
					Major = Major + 1,
					Minor = 0,
					Patch = 0,
					PreRelease = new string[0]
				};
			} else if (type == ReleaseType.Minor) {
				return this with {
					Minor = Minor + 1,
					Patch = 0,
				};
			} else {
				return this with {
					Patch = Patch + 1
				};
			}
		}

		private static void NextPrerelease(List<string> list) {
			if (int.TryParse(list.LastOrDefault(), out int numeric)) {
				numeric++;
				list[list.Count - 1] = Convert.ToString(numeric);
			} else {
				list.Add(Convert.ToString(numeric));
			}
		}

		public SematicVersion NextPrerelease(string? label) {
			if (string.IsNullOrEmpty(label)) { label = "0"; }
			var newVersion = this with {
				Major = Major,
				Minor = Minor,
				Patch = HasPreRelease ? Patch : Patch + 1,
				PreRelease = [label],
			};
			if (newVersion.CompareTo(this) > 0) {
				return newVersion;
			} else {
				var list = new List<string>(PreRelease);
				NextPrerelease(list);
				return this with {
					PreRelease = list
				};
			}
		}
	}
}
