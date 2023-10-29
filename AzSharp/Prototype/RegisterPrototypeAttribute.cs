using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Prototype;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RegisterPrototypeAttribute : Attribute
{
    public string Tag { get;}
    public RegisterPrototypeAttribute(string tag)
    {
        Tag = tag;
    }
}
