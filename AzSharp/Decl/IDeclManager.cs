using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Decl;

public interface IDeclManager
{
    public T GetDecl<T>(string tag);
    public List<T> GetDecls<T>();
    public void RegisterDecl(Type decl_type);
    public void RegisterDeclImpl(Type impl_type, Type decl_type, string tag);
    public void RegisterFromAttributes();
}
