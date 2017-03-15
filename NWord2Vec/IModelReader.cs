using System;

namespace NWord2Vec
{
    public interface IModelReader 
    {
        Model Open();
    }
}
