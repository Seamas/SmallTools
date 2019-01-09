using System;
using System.Data;

namespace SQLCreator
{
    public interface ICreator
    {
        string Create(Table table);
    }
}