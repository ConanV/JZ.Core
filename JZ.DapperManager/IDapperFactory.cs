using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.DapperManager
{
    public interface IDapperFactory
    {
        DapperClient CreateClient(string name);
    }
}
