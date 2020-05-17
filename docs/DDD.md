# DDD
## Entities
Domain-driven design (DDD) is the concept that the structure and language of your code (class names, class methods, class variables) should match the business domain. For example, if your software processes loan applications, it might have classes such as LoanApplication and Customer, and methods such as AcceptOffer and Withdraw. The term was coined by Eric Evans in his book of the same title.

### IHasId
IHasId, IHasId<T>
Generic and non-generic versions of the IHasId attribute are used both for business objects and their projections (DTO’s). These interfaces are essential for some handy extension methods described in the “meta-programming” section of the documentation.

### HasIdBase: IHasId, HasIdBase<T>: IHasId<T>
The only reason for the existence of these classes is to avoid code duplication in your codebase because typically there are lots of IDs in every business app. You might also want to declare more advanced classes for your entities. For instance, making your Id property setter non-public maybe a good thing for your design in terms of reliability and maintainability. The good news is that you can write these classes by yourself. Force is not a framework, so we try to keep it as simple and lightweight as possible.

### HasNameBase: HasIdBase
This is just a more specific base class derived from the HasIdBase. Literally every single business app has at least one entity with a name. That’s why this base class is included.

## ValueObjects

## Spec, SpecBuilder

## Domain Events

## DomainEventStore