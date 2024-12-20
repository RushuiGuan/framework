﻿using Albatross.Reflection;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Albatross.Hosting.Utility {
	public static class Extensions {
		public static async Task<int> Run(this Parser parser, string[] args, params Type[] jobTypes) {
			Dictionary<Type, Type> dict = new Dictionary<Type, Type>();
			foreach (var jobType in jobTypes) {
				if (jobType.TryGetClosedGenericType(typeof(IUtility<>), out var genericType)) {
					Type optionType = genericType.GetGenericArguments().First();
					dict.Add(optionType, jobType);
				}
			}
			ParserResult<Object> parserResult = parser.ParseArguments(args, dict.Keys.ToArray());
			return await parserResult.MapResult<object, Task<int>>(async opt => await RunAsync(opt, dict), err => Task.FromResult(1));
		}
		public static async Task<int> Run(this Parser parser, string[] args, params Assembly[] assemblies) {
			Dictionary<Type, Type> dict = new Dictionary<Type, Type>();
			foreach (var asm in assemblies) {
				foreach (var type in asm.GetTypes()) {
					if (type.TryGetClosedGenericType(typeof(IUtility<>), out var genericType)) {
						Type optionType = genericType.GetGenericArguments().First();
						dict.Add(optionType, type);
					}
				}
			}
			dict.Add(typeof(ShowEnvironmentOption), typeof(ShowEnvironment));
			ParserResult<Object> parserResult = parser.ParseArguments(args, dict.Keys.OrderBy(x => x.GetCustomAttribute<VerbAttribute>()?.Name).ToArray());
			return await parserResult.MapResult<object, Task<int>>(async opt => await RunAsync(opt, dict), err => Task.FromResult(1));
		}
		static async Task<int> RunAsync(object opt, Dictionary<Type, Type> dict) {
			Type optionType = opt.GetType();
			Type jobType = dict[optionType];
			using (IUtility utility = (IUtility)Activator.CreateInstance(jobType, opt)!) {
				return await utility.Run();
			}
		}
	}
}