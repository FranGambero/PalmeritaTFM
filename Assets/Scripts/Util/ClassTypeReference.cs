using System;
using UnityEngine;

namespace ElJardin.Util
{
    /// <summary>
    /// Reference to a class <see cref="System.Type"/> with support for Unity serialization. Based on Rotorz solution.
    /// </summary>
    [Serializable]
    public sealed class ClassTypeReference : ISerializationCallbackReceiver
    {
        [SerializeField] string classRef;
        
        Type type;

        #region Accesors
        public Type Type
        {
            get => type;
            set
            {
                if (value != null && !value.IsClass)
                    throw new ArgumentException($"'{value.FullName}' is not a class type.", nameof(value));

                type = value;
                classRef = GetClassRef(value);
            }
        }
        #endregion
        
        #region Constructors
        public ClassTypeReference() { }
        public ClassTypeReference(Type type)
        {
            Type = type;
        }
        public ClassTypeReference(string assemblyQualifiedClassName)
        {
            Type = !string.IsNullOrEmpty(assemblyQualifiedClassName)
                ? Type.GetType(assemblyQualifiedClassName)
                : null;
        }
        #endregion

        #region ISerializationCallbackReceiver Implementation
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            type = null;
            if(string.IsNullOrEmpty(classRef))
                return;
            
            type = Type.GetType(classRef);
            if (type == null)
                Debug.LogWarning($"'{classRef}' was referenced but class type was not found.");
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        #endregion
        
        public static string GetClassRef(Type type) => type != null ? type.FullName + ", " + type.Assembly.GetName().Name : "";

        #region Operator overloading
        public static implicit operator string(ClassTypeReference typeReference) => typeReference.classRef;
        public static implicit operator Type(ClassTypeReference typeReference) => typeReference.Type;
        public static implicit operator ClassTypeReference(Type type) => new ClassTypeReference(type);
        #endregion
        
        public override string ToString() => Type?.FullName ?? "(None)";
    }
}