using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using CostEffectiveCode.Common;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Tests;
using JetBrains.Annotations;

namespace Test.Stubs
{
    public class LinqQuery : IQuery<string, IEnumerable<ProductDto>>
    {
        private readonly ILinqProvider _linqProvider;
        private readonly IProjector _projector;

        public LinqQuery([NotNull] ILinqProvider linqProvider, [NotNull] IProjector projector)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            _linqProvider = linqProvider;
            _projector = projector;
        }

        public IEnumerable<ProductDto> Ask(string spec) => _linqProvider
            .Query<Product>()
            .Where(x => x.Name.StartsWith(spec))
            .Project<ProductDto>(_projector)
            .ToArray();
    }
}