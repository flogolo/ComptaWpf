using System;

namespace CommonLibrary.IOC
{
    public class RegisteredObject
    {
        public RegisteredObject(Type typeToResolve, Type concreteType, LifeCycle lifeCycle)
        {
            TypeToResolve = typeToResolve;
            ConcreteType = concreteType;
            LifeCycle = lifeCycle;
        }

        public Type TypeToResolve { get; private set; }

        public Type ConcreteType { get; private set; }

        public object Instance { get; set; }

        public LifeCycle LifeCycle { get; private set; }

        public void CreateInstance(params object[] args)
        {
            Instance = Activator.CreateInstance(this.ConcreteType, args);
        }
    }
}