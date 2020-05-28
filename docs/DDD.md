# DDD
Domain-driven design (DDD) is the concept that the structure and language of your code (class names, class methods, class variables) should match the business domain. For example, if your software processes loan applications, it might have classes such as LoanApplication and Customer, and methods such as AcceptOffer and Withdraw. The term was coined by Eric Evans in his book of the same title.

## Entities
There is a category of objects which seem to have an identity, which remains the same throughout the states of the software. Such objects are called Entities.

### Id<TKey, T>
TODO: Doc

### IHasId
IHasId, IHasId<T>
Generic and non-generic versions of the IHasId attribute are used both for business objects and their projections (DTO’s). These interfaces are essential for some handy extension methods described in the “meta-programming” section of the documentation.

### HasIdBase: IHasId, HasIdBase<T>: IHasId<T>
The only reason for the existence of these classes is to avoid code duplication in your codebase because typically there are lots of IDs in every business app. You might also want to declare more advanced classes for your entities. For instance, making your Id property setter non-public maybe a good thing for your design in terms of reliability and maintainability. The good news is that you can write these classes by yourself. Force is not a framework, so we try to keep it as simple and lightweight as possible.

### HasNameBase: HasIdBase
This is just a more specific base class derived from the HasIdBase. Literally every single business app has at least one entity with a name. That’s why this base class is included.

## ValueObjects
There are cases when we need to contain some attributes of a domain element. We are not interested in which object it is, but what attributes it has. An object that is used to describe certain aspects of a domain, and which does not have an identity, is named Value Object.
The ValueObject implementation is inspired by Vladimir Khorikov’s article “Value Object: a better implementation”

## Spec
Specification (Spec)is a pattern that allows us to encapsulate some piece of domain knowledge into a single unit - specification - and reuse it in different parts of the codebase. C# implementation of the pattern benefits from using the Expression Trees so that it can be applied to both IQueryable and IEnumerable. Compiling an expression tree is a relatively slow operation so it’s cached internally so that the Compile method will run only once per specification instance. That’s why it’s recommended to define specifications as static read-only variables.

Spec implementation supports &,&&, |,||, and ! operators and also provides an extension method From which helps to build new specifications composing the existing ones within a single aggregate.

## SpecBuilder
SpecBuilder is a powerful tool for creating specifications by conventions. See the FilterConventions documentation for more details.

## Domain Events
The need for domain events comes from a desire to inject services into domain models. What we really want is to create an encapsulated domain model, but decouple ourselves from potential side effects and isolate those explicitly. The domain events implementation is based on Jimmy Boggard’s article “A better domain events pattern” and slightly improved.

## DomainEventStore