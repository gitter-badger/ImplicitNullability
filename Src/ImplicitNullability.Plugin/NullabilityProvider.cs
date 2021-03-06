using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CodeAnnotations;

namespace ImplicitNullability.Plugin
{
    /// <summary>
    /// An accessor for ReSharper's nullability information of declared elements (using <see cref="CodeAnnotationsCache"/>)
    /// </summary>
    [PsiComponent]
    public class NullabilityProvider
    {
#if RESHARPER92 || RESHARPER100
        private readonly CodeAnnotationsCache _codeAnnotationsCache;

        public NullabilityProvider(CodeAnnotationsCache codeAnnotationsCache)
        {
            _codeAnnotationsCache = codeAnnotationsCache;
        }

        public bool ContainsAnyExplicitNullabilityAttributes(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.NotNullAttributeShortName) ||
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.CanBeNullAttributeShortName));
        }

        public bool ContainsExplicitNotNullNullabilityAttribute(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.NotNullAttributeShortName));
        }

        public bool ContainsAnyExplicitItemNullabilityAttributes(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.ItemNotNullAttributeShortName) ||
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.ItemCanBeNullAttributeShortName));
        }

        public bool ContainsExplicitItemNotNullNullabilityAttribute(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsCache.IsAnnotationAttribute(x, CodeAnnotationsCache.ItemNotNullAttributeShortName));
        }

        public CodeAnnotationNullableValue? GetElementNullability(IAttributesOwner attributesOwner)
        {
            return _codeAnnotationsCache.GetNullableAttribute(attributesOwner);
        }

        public CodeAnnotationNullableValue? GetContainerElementNullability(IAttributesOwner attributesOwner)
        {
            return _codeAnnotationsCache.GetContainerElementNullableAttribute(attributesOwner);
        }

#else
        private readonly CodeAnnotationsConfiguration _codeAnnotationsConfiguration;
        private readonly NullnessProvider _nullnessProvider;
        private readonly ContainerElementNullnessProvider _containerElementNullnessProvider;

        public NullabilityProvider(
            CodeAnnotationsConfiguration codeAnnotationsConfiguration,
            CodeAnnotationsCache codeAnnotationsCache)
        {
            _codeAnnotationsConfiguration = codeAnnotationsConfiguration;
            _nullnessProvider = codeAnnotationsCache.GetProvider<NullnessProvider>();
            _containerElementNullnessProvider = codeAnnotationsCache.GetProvider<ContainerElementNullnessProvider>();
        }


        public bool ContainsAnyExplicitNullabilityAttributes(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, NullnessProvider.NotNullAttributeShortName) ||
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, NullnessProvider.CanBeNullAttributeShortName));
        }

        public bool ContainsExplicitNotNullNullabilityAttribute(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, NullnessProvider.NotNullAttributeShortName));
        }

        public bool ContainsAnyExplicitItemNullabilityAttributes(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, ContainerElementNullnessProvider.ItemNotNullAttributeShortName) ||
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, ContainerElementNullnessProvider.ItemCanBeNullAttributeShortName));
        }

        public bool ContainsExplicitItemNotNullNullabilityAttribute(IEnumerable<IAttributeInstance> attributeInstances)
        {
            return attributeInstances.Any(x =>
                _codeAnnotationsConfiguration.IsAnnotationAttribute(x, ContainerElementNullnessProvider.ItemNotNullAttributeShortName));
        }

        public CodeAnnotationNullableValue? GetElementNullability(IAttributesOwner attributesOwner)
        {
            return _nullnessProvider.GetInfo(attributesOwner);
        }

        public CodeAnnotationNullableValue? GetContainerElementNullability(IAttributesOwner attributesOwner)
        {
            return _containerElementNullnessProvider.GetInfo(attributesOwner);
        }
#endif
    }
}