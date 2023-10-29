using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Decl;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterDeclImplAttribute : Attribute
{
    public Type declType;
    public string tag = string.Empty;
    public RegisterDeclImplAttribute(Type declType, string tag)
    {
        this.declType = declType;
        this.tag = tag;
    }
}
