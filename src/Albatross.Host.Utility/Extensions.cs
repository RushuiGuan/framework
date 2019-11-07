using Albatross.Reflection;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Host.Utility {
	public static class Extensions {
		public static int Run(this Parser parser, string[] args, params Type[] jobTypes) {
			Dictionary<Type, Type> dict = new Dictionary<Type, Type>();
			foreach(var jobType in jobTypes){
				if (jobType.TryGetClosedGenericType(typeof(IUtility<>), out Type genericType)) {
					Type optionType = genericType.GetGenericArguments().First();
					dict.Add(optionType, jobType);
				}
			}
			ParserResult<Object> parserResult = parser.ParseArguments(args, dict.Keys.ToArray());
			return parserResult.MapResult<object, int>(opt => {
				Type optionType = opt.GetType();
				Type jobType = dict[optionType];
				IUtility utility = (IUtility)Activator.CreateInstance(jobType, opt);
				return utility.Run();
			}, err=>1);
		}
	}
}
