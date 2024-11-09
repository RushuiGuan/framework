# Albatross.DevTools
A command line program that contains utilities that help with dotnet development process

## Installation
`dotnet tool install -g Albatross.DevTools`

## Commands
|Verb|Description|
|---|---|
|`fix-markdown-relative-urls`|Replace the relative urls in a markdown file with absolute urls using the provided RootUrl and RootFolder.  The new url will be the constructed with the format of: {RootUrl}/{PathRelativeToRootFolder}.  The utility is useful to fix the relative urls in the README.md file since the file is packed as part of the nuget package and its relative urls will not work in the nuget.org website|