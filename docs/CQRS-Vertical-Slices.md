# CQRS, Vertical Slices
Vertical Slices architecture is built around distinct requests, encapsulating and grouping all concerns from front-end to back. You take a normal "n-tier" or hexagonal/whatever architecture and remove the gates and barriers across those layers, and couple along the axis of change. The style is used and popularized by:
- Jimmy Boggard: [Vertical slice architecture] (https://jimmybogard.com/vertical-slice-architecture/), [NDC talk] (https://www.youtube.com/watch?v=SUiWfhAhgQw), [NDC talk] (https://www.youtube.com/watch?v=T6nglsEDaqA)
- Steven van Deursen: [Meanwhile on the command side of my architecture] (https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/), [meanwhile on the query side of my architecture] (https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-query-side-of-my-architecture/)
- Maxim Arshinov: [Instant Design] (https://habr.com/ru/company/jugru/blog/447308/)

## IHandler<TIn, TOut>
Todo: add docs

## ICommand<T>, ICommandHandler<TIn, TOut>
Todo: add docs

## IQuery<T>, IQueryHandler<TIn, TOut>
Todo: add docs

## FilterQuery, PagedFilterQuery
Todo: add docs