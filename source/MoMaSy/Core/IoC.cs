using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace outcold.MoMaSy.Core
{
    public class IoC
    {
        private static readonly Lazy<IoC> _instance = new Lazy<IoC>(() => new IoC());

        public static IoC Instance
        {
            get { return _instance.Value; }
        }

        private BindingFlags _bindingFlagsAllInstanceCtors = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;        
        private readonly IDictionary<Type, RegisteredObject> _registeredObjects = new Dictionary<Type, RegisteredObject>();

        public void Register<TType>() where TType : class
        {
            Register<TType, TType>(false, null);
        }

        public void Register<TType, TConcrete>() where TConcrete : class, TType 
        {
            Register<TType, TConcrete>(false, null);
        }

        public void RegisterSingleton<TType>() where TType : class
        {
            RegisterSingleton<TType, TType>();
        }

        public void RegisterSingleton<TType, TConcrete>() where TConcrete : class, TType 
        {
            Register<TType, TConcrete>(true, null);
        }

        public void RegisterInstance<TType>(TType instance) where TType : class
        {
            RegisterInstance<TType, TType>(instance);
        }

        public void RegisterInstance<TType, TConcrete>(TConcrete instance) where TConcrete : class, TType 
        {
            Register<TType, TConcrete>(true, instance);
        }

        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            return (TTypeToResolve)ResolveObject(typeof(TTypeToResolve));
        }

        public object Resolve(Type type)
        {
            return ResolveObject(type);
        }

        private void Register<TType, TConcrete>(bool isSingleton, TConcrete instance)
        {
            Register(typeof(TType), typeof(TConcrete), isSingleton, instance);
        }

        private void Register(Type type, Type concreteType, bool isSingleton, object concreteInstance)
        {
            if (_registeredObjects.ContainsKey(type))
                _registeredObjects.Remove(type);
            var registeredObject = new RegisteredObject(concreteType, isSingleton, concreteInstance);
            _registeredObjects.Add(type, registeredObject);
        }

        private object ResolveObject(Type type)
        {
            if (type == GetType())
                return this;

            if (!_registeredObjects.ContainsKey(type))
            {
                if (!type.IsInterface && !type.IsAbstract)
                    Register(type, type, false, null);
                else
                    throw new ArgumentOutOfRangeException(string.Format("The type {0} has not been registered", type.Name));
            }
            return GetInstance(_registeredObjects[type]);
        }

        private object GetInstance(RegisteredObject registeredObject) 
        { 
            object instance = registeredObject.Instance; 
            if (instance == null) 
            { 
                var constructorInfo = registeredObject.ConcreteType.GetConstructors(_bindingFlagsAllInstanceCtors).First(); 
                var parameters = constructorInfo.GetParameters().Select(parameter => ResolveObject(parameter.ParameterType)); 
                instance = registeredObject.CreateInstance(constructorInfo, parameters.ToArray()); 
            } 
            return instance; 
        }

        private class RegisteredObject 
        { 
            private readonly bool _isSinglton; 
            
            public RegisteredObject(Type concreteType, bool isSingleton, object instance) 
            { 
                _isSinglton = isSingleton; ConcreteType = concreteType; Instance = instance; 
            } 
            
            public Type ConcreteType { get; private set; } 
            public object Instance { get; private set; } 
            
            public object CreateInstance(ConstructorInfo ctor, params object[] args) 
            { 
                NewExpression newExp = Expression.New(ctor, args.Select(x => Expression.Constant(x))); 
                LambdaExpression lambda = Expression.Lambda(typeof(Func<object>), newExp); 
                Func<object> compiled = (Func<object>)lambda.Compile(); var instance = compiled(); 
                if (_isSinglton)                    
                    Instance = instance; return instance; 
            } 
        }
    }
}