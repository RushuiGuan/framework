using Albatross.ReqnrollPlugin;
using Reqnroll;
using Reqnroll.Infrastructure;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;
using System;

[assembly: RuntimePlugin(typeof(AlbatrossPlugin))]
namespace Albatross.ReqnrollPlugin {
	public class AlbatrossPlugin : IRuntimePlugin {
		private static readonly object _registrationLock = new object();

		public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration) {
			runtimePluginEvents.CustomizeGlobalDependencies += CustomizeGlobalDependencies;
			// runtimePluginEvents.CustomizeFeatureDependencies += CustomizeFeatureDependencies;
			runtimePluginEvents.CustomizeScenarioDependencies += CustomizeScenarioDependencies;
		}

		private void CustomizeGlobalDependencies(object sender, CustomizeGlobalDependenciesEventArgs args) {
			// temporary fix for CustomizeGlobalDependencies called multiple times
			// see https://github.com/techtalk/Reqnroll/issues/948
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
						// lcEvents.AfterFeature += AfterFeaturePluginLifecycleEventHandler;
						lcEvents.AfterScenario += AfterScenarioPluginLifecycleEventHandler; ;
					}
				}
			}
		}

		private void CustomizeFeatureDependencies(object sender, CustomizeFeatureDependenciesEventArgs args) {

		}

		private void CustomizeScenarioDependencies(object sender, CustomizeScenarioDependenciesEventArgs args) {
			var host = args.ObjectContainer.Resolve<ReqnrollHost>();
			args.ObjectContainer.RegisterFactoryAs(() => {
				var contextManager = args.ObjectContainer.Resolve<IContextManager>();
				var feature = args.ObjectContainer.Resolve<FeatureContext>();
				var scenario = args.ObjectContainer.Resolve<ScenarioContext>();
				return host.CreateScope(contextManager, feature, scenario).ServiceProvider;
			});
		}

		private void AfterScenarioPluginLifecycleEventHandler(object sender, RuntimePluginAfterScenarioEventArgs e) {
			var context = e.ObjectContainer.Resolve<ScenarioContext>();
			var host = e.ObjectContainer.Resolve<ReqnrollHost>();
			host.DisposeScope(context);
		}
	}
}