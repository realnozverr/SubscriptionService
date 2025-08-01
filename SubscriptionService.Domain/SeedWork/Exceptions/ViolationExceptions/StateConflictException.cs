using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ViolationExceptions;

[ExcludeFromCodeCoverage]
public class StateConflictException(string message = "State conflict") : ViolationException(message);