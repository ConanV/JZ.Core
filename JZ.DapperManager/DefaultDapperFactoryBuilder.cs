using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.DapperManager
{
    internal class DefaultDapperFactoryBuilder: IDapperFactoryBuilder
    {
        public DefaultDapperFactoryBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}
