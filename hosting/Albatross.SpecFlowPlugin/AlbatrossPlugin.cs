using System;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;

namespace Albatross.SpecFlowPlugin {
	public class AlbatrossPlugin : IRuntimePlugin {
		private static readonly object _registrationLock = new object();

		public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration) {
			runtimePluginEvents.CustomizeGlobalDependencies += CustomizeGlobalDependencies;
			runtimePluginEvents.CustomizeFeatureDependencies += CustomizeFeatureDependencies;
			runtimePluginEvents.CustomizeScenarioDependencies += CustomizeScenarioDependencies;

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
						args.ObjectContainer.RegisterFactoryAs(() => testHost.RootServiceProvider);

						var lcEvents = args.ObjectContainer.Resolve<RuntimePluginTestExecutionLifecycleEvents>();
						lcEvents.AfterScenario += AfterScenarioPluginLifecycleEventHandler;
						lcEvents.AfterFeature += AfterFeaturePluginLifecycleEventHandler;
					}
				}
			}
		}

		private void CustomizeFeatureDependencies(object sender, CustomizeFeatureDependenciesEventArgs e) {
			// At this point we have the bindings, we can resolve the service provider, which will build it if it's the first time.
			var spContainer = args.ObjectContainer.Resolve<RootServiceProviderContainer>();

			if (spContainer.Scoping == ScopeLevelType.Feature) {
				var serviceProvider = spContainer.ServiceProvider;

				// Now we can register a new scoped service provider
				args.ObjectContainer.RegisterFactoryAs<IServiceProvider>(() => {
					var scope = serviceProvider.CreateScope();
					BindMappings.TryAdd(scope.ServiceProvider, args.ObjectContainer.Resolve<IContextManager>());
					ActiveServiceScopes.TryAdd(args.ObjectContainer.Resolve<FeatureContext>(), scope);
					return scope.ServiceProvider;
				});
			}
		}

		private void CustomizeScenarioDependencies(object sender, CustomizeScenarioDependenciesEventArgs e) {
			// At this point we have the bindings, we can resolve the service provider, which will build it if it's the first time.
			var spContainer = args.ObjectContainer.Resolve<RootServiceProviderContainer>();

			if (spContainer.Scoping == ScopeLevelType.Scenario) {
				var serviceProvider = spContainer.ServiceProvider;
				// Now we can register a new scoped service provider
				args.ObjectContainer.RegisterFactoryAs<IServiceProvider>(() => {
					var scope = serviceProvider.CreateScope();
					BindMappings.TryAdd(scope.ServiceProvider, args.ObjectContainer.Resolve<IContextManager>());
					ActiveServiceScopes.TryAdd(args.ObjectContainer.Resolve<ScenarioContext>(), scope);
					return scope.ServiceProvider;
				});
			}
		}

		private void AfterFeaturePluginLifecycleEventHandler(object sender, RuntimePluginAfterFeatureEventArgs e) {
			throw new NotImplementedException();
		}

		private void AfterScenarioPluginLifecycleEventHandler(object sender, RuntimePluginAfterScenarioEventArgs e) {
			throw new NotImplementedException();
		}
	}
}
