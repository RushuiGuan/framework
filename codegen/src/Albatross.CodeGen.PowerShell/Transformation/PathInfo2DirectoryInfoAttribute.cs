using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.PowerShell.Transformation
{
	public class PathInfo2DirectoryInfoAttribute : ArgumentTransformationAttribute {
		public override object Transform(EngineIntrinsics engineIntrinsics, object inputData) {
			if ((inputData as PSObject)?.BaseObject is PathInfo pathInfo) {
				return new DirectoryInfo[] { new DirectoryInfo(pathInfo.Path) };
			} else {
				return inputData;
			}
		}
	}
}
