using System;
using System.Collections.Generic;

namespace AzSharp.Reflection;

public interface IReflectionManager
{
    public List<Type> GetAllTypes();
    public List<Type> GetAllChildren<T>(bool inclusive = false);
    public List<Type> GetAllChildren(Type base_type, bool inclusive = false);
    public List<Type> FindTypesWithAttribute<T>() where T : Attribute;
    public List<Type> FindTypesWithAttribute(Type attributeType);
    public void Preload();
}
