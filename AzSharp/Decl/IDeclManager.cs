using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Decl;

public interface IDeclManager
{
    public T GetDecl<T>(string tag) where T : Decl;
    public List<T> GetDecls<T>() where T : Decl;
    public void RegisterDecl(Type decl_type);
    public void RegisterDeclImpl(Type impl_type, Type decl_type, string tag);
    public void RegisterFromAttributes();
}
