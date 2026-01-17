using System;
using System.IO;
using dnlib.DotNet;
using Moq;
using Xunit;

namespace Confuser.Renamer.Test {
	internal static class Helpers {
		internal static ModuleDefMD LoadTestModuleDef() {

			var asmResolver = new AssemblyResolver { EnableTypeDefCache = true };

			var tpa = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;
			if (!string.IsNullOrEmpty(tpa)) {
				foreach (var p in tpa.Split(Path.PathSeparator))
					asmResolver.PreSearchPaths.Add(Path.GetDirectoryName(p));
			}

			asmResolver.DefaultModuleContext = new ModuleContext(asmResolver);			

			var options = new ModuleCreationOptions(asmResolver.DefaultModuleContext) {
				TryToLoadPdbFromDisk = false
			};

            asmResolver.AddToCache(ModuleDefMD.Load(typeof(Mock).Module, options));
            asmResolver.AddToCache(ModuleDefMD.Load(typeof(FactAttribute).Module, options));

            var thisModule = ModuleDefMD.Load(typeof(VTableTest).Module, options);
            asmResolver.AddToCache(thisModule);

			return thisModule;
		}
	}
}
