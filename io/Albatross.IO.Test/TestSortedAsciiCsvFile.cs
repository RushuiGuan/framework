﻿using System;
using Albatross.Collections;
using Albatross.Hosting.Test;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.IO.Test {
	public class TestSortedAsciiCsvFile{
		[Theory]
		//[InlineData("", "", "")]
		// contain
		[InlineData("1-o5", "4", "1,3,4,5")]
		[InlineData("1-o5", "2-e4", "1,2,4,5")]
		[InlineData("0-5", "1,4", "0,1,4,5")]
		// rightie
		[InlineData("0", "1", "0,1")]
		[InlineData("0-e2", "3-o5", "0,2,3,5")]
		// rightie-with-overlap
		[InlineData("0-1", "1", "0,1")]
		[InlineData("0-1", "1-2", "0,1,2")]
		[InlineData("0-5", "2,5", "0,1,2,5")]
		[InlineData("1-o5", "4-e6", "1,3,4,6")]
		// leftie
		[InlineData("1", "0", "0,1")]
		[InlineData("2-3", "0-1", "0,1,2,3")]
		// leftie-with-overlap
		[InlineData("0-5", "0-1", "0,1,2,3,4,5")]
		[InlineData("1-2", "0-1", "0,1,2")]
		[InlineData("1-2", "1", "1,2")]
		// wrap
		[InlineData("0-o5", "0-5", "0,1,2,3,4,5")]
		[InlineData("2-3", "0-e6", "0,2,4,6")]
		[InlineData("2-3", "2-3", "2,3")]
		[InlineData("2-3", "1-3", "1,2,3")]
		[InlineData("2-3", "2-4", "2,3,4")]
		public async Task TestTheStiching(string current, string changes, string expected) {
			FileStream current_stream;
			using (current_stream = File.Open(My.SortedTestFile(current, @"c:\temp\current.csv"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read)) {
				using (var changes_stream = File.Open(My.SortedTestFile(changes, @"c:\temp\changes.csv"), FileMode.Open, FileAccess.ReadWrite, FileShare.Read)) {
					var current_data = new SortedAsciiCsvFile<int>(current_stream, x => int.Parse(x));
					var changes_data = new SortedAsciiCsvFile<int>(changes_stream, x => int.Parse(x));
					await current_data.Stitch(changes_data);
				}
			}
			current_stream.Name.StringContent().Should().BeEquivalentTo(My.SortedTestFile(expected).StringContent());
		}
	}
}
