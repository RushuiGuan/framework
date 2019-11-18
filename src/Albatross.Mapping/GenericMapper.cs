using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Albatross.Reflection;
using Albatross.Mapping.Core;

namespace Albatross.Mapping {
	public class PropertyMapper<From, To> {
		private readonly IMapperFactory factory;
		private Action<From, To> runner;


		public PropertyMapper(IMapperFactory factory) {
			this.factory = factory;
		}
		internal PropertyMapper(IMapperFactory factory, PropertyInfo from, PropertyInfo to):this(factory) {
			this.FromProperty = from;
			this.ToProperty = to;
		}

		public PropertyInfo FromProperty { get; private set; }
		public PropertyInfo ToProperty { get; private set; }

		public PropertyMapper<From, To> For(Expression<Func<To, object>> expression) {
			ToProperty = expression.GetPropertyInfo();
			if (!ToProperty.CanWrite) { throw new MappingException($"Property {ToProperty.Name} is readonly"); }
			if (ToProperty.GetAccessors().Any(args => args.IsStatic)) { throw new MappingException($"Property {ToProperty.Name} is static"); }
			return this;
		}

		public void Use(Expression<Func<From, object>> expression) {
			FromProperty = expression.GetPropertyInfo();
			if (!ToProperty.CanRead) { throw new MappingException($"Property {ToProperty.Name} cannot be read"); }
			if (ToProperty.GetAccessors().Any(args => args.IsStatic)) { throw new MappingException($"Property {ToProperty.Name} is static"); }
			Verify(true);
		}

		public bool Verify(bool throwIfNotVerified){
			if (FromProperty.PropertyType == ToProperty.PropertyType) {
				if (FromProperty.PropertyType.IsPrimitive || FromProperty.PropertyType == typeof(string) || FromProperty.PropertyType.IsValueType) {
					runner = DefaultRunner;
					return true;
				}
			}
			if (factory.TryGet(FromProperty.PropertyType, ToProperty.PropertyType, out IMapper mapper)) {
				if (ToProperty.PropertyType.GetConstructor(new Type[0]) != null) {
					this.runner = (from, to) => FancyRunner(from, to, mapper);
					return true;
				}
			}
			if(throwIfNotVerified){
				throw new MappingException($"Property mapping from {FromProperty.Name} to {ToProperty.Name} cannot be verified");
			}
			return false;
		}

		public void Run(From src, To dst){
			this.runner(src, dst);
		}

		void DefaultRunner(From src, To dst){
			var value = FromProperty.GetValue(src);
			ToProperty.SetValue(dst, value);
		}
		void FancyRunner(From srcObject, To dstObject, IMapper mapper){
			var srcValue = FromProperty.GetValue(srcObject);
			var dstValue = Activator.CreateInstance(ToProperty.PropertyType);
			mapper.Map(srcValue, dstValue);
			ToProperty.SetValue(dstObject, dstValue);
		}
	}

	public class GenericMapper<From, To> : MapperBase<From, To> {
		private readonly Core.IMapperFactory factory;
		Dictionary<PropertyInfo, PropertyMapper<From, To>> propertyMappers = new Dictionary<PropertyInfo, PropertyMapper<From, To>>();

		public PropertyMapper<From, To> ForProperty(Expression<Func<To, object>> expression) {
			var propertyMapper = new PropertyMapper<From, To>(factory).For(expression);
			this.propertyMappers[propertyMapper.ToProperty] = propertyMapper;
			return propertyMapper;
		}

		public void SetPropertyMapper(PropertyMapper<From, To> mapper) {
			propertyMappers[mapper.ToProperty] = mapper;
		}

		public GenericMapper(Core.IMapperFactory factory) {
			this.factory = factory;
			var mappings = from srcProperty in typeof(From).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
						   join dstProperty in typeof(To).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
						   on srcProperty.Name equals dstProperty.Name
						   select new PropertyMapper<From, To>(factory, srcProperty, dstProperty);
			foreach(var item in mappings){
				if (item.Verify(false)) {
					propertyMappers.Add(item.ToProperty, item);
				}
			}
		}

		public override void Map(From src, To dst) {
			foreach(var item in propertyMappers.Values){
				item.Run(src, dst);
			}
		}
	}
}
