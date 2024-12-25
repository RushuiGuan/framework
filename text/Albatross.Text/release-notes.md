# 7.5.8
* Make `Extensions.TrimDecimal` obsolete
* Create `Extensions.Decimal2CompactString` to replace `Extensions.TrimDecimal`.  `Extensions.Decimal2CompactString` uses G29 formatting code to get the same job done.
* Create `Extensions.TrimDecimalTrailingZeros` to accept a string instead of decimal type.  This is useful to format compact the decimal text after formatting.
