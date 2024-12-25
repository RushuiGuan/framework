# 7.5.8
* Make `Extensions.TrimDecimal` obsolete
* Create `Extensions.Decimal2CompactText(decimal)` to replace `Extensions.TrimDecimal`.  `Extensions.Decimal2CompactText` uses G29 formatting code to get the same job done.
* Create `Extensions.Decimal2CompactText(string)` to accept a string instead of decimal type.  This is useful to compact the decimal text after formatting.
