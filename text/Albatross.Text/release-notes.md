# 8.0.0
* Breaking changes: `StringInterpolationService` has been removed and replaced with static methods in `StringInterpolationExtensions`.
* Deprecated the following classes `PrintTextExtensions`, `PrintOption`, `PrintTableOption`, `PrintOptionBuilder` and `PrintPropertiesOption`
	- Use `YamlDotNet` to print single object
	- Use `Albatross.Text.Table` to print collections of object in tabular format
# 7.5.8
* Make `Extensions.TrimDecimal` obsolete
* Create `Extensions.Decimal2CompactText(decimal)` to replace `Extensions.TrimDecimal`.  `Extensions.Decimal2CompactText` uses G29 formatting code to get the same job done.
* Create `Extensions.Decimal2CompactText(string)` to accept a string instead of decimal type.  This is useful to compact the decimal text after formatting.
