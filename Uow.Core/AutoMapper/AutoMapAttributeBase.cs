using System;
using AutoMapper;

namespace Uow.Core.AutoMapper
{
    public abstract class AutoMapAttributeBase : Attribute
    {
        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public Type[] TargetTypes { get; }

        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}