using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ViolationExceptions;

[ExcludeFromCodeCoverage]
public class InvariantViolationException(string message = "Invariant violation") : ViolationException(message);