using Albatross.SpecFlowPlugin;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;

[assembly: RuntimePlugin(typeof(AlbatrossPlugin))]
namespace Albatross.SpecFlowPlugin {
	public class AlbatrossPlugin : IRuntimePlugin {
		private static readonly object _registrationLock = new object();

		public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration) {
			runtimePluginEvents.CustomizeGlobalDependencies += CustomizeGlobalDependencies;
			runtimePluginEvents.CustomizeFeatureDependencies += CustomizeFeatureDependencies;
		}
	
		private void CustomizeGlobalDependencies(object sender, CustomizeGlobalDependenciesEventArgs args) {
			// temporary fix for CustomizeGlobalDependencies called multiple times
			// see https://github.com/techtalk/SpecFlow/issues/948
			if (!args.ObjectContainer.IsRegistered<ITestHostFinder>()) {
				// an extra lock to ensure that there are not two super fast threads re-registering the same stuff
				lock (_registrationLock) {
					if (!args.ObjectContainer.IsRegistered<ITestHostFinder>()) {
						args.ObjectContainer.RegisterTypeAs<AlbatrossTestObjectResolver, ITestObjectResolver>();
						args.ObjectContainer.RegisterTypeAs<TestHostFinder, ITestHostFinder>();

						var testHostFinder = args.ObjectContainer.Resolve<ITestHostFinder>();
						var testHost = testHostFinder.GetHost();
						args.ObjectContainer.RegisterFactoryAs(() => testHost);

						var lcEvents = args.ObjectContainer.Resolve<RuntimePluginTestExecutionLifecycleEvents>();
						lcEvents.AfterFeature += AfterFeaturePluginLifecycleEventHandler;
					}
				}
			}
		}

		private void CustomizeFeatureDependencies(object sender, CustomizeFeatureDependenciesEventArgs args) {
			var host = args.ObjectContainer.Resolve<SpecFlowHost>();
			args.ObjectContainer.RegisterFactoryAs<IServiceProvider>(() => {
				var context = args.ObjectContainer.Resolve<FeatureContext>();
				return host.CreateScope(context).ServiceProvider;
			});
		}

		private void AfterFeaturePluginLifecycleEventHandler(object sender, RuntimePluginAfterFeatureEventArgs e) {
			var context = e.ObjectContainer.Resolve<FeatureContext>();
			var host = e.ObjectContainer.Resolve<SpecFlowHost>();
			host.DisposeScope(context);
		}
	}
}
