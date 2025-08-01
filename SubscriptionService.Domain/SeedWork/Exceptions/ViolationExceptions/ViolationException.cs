using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ViolationExceptions;

[ExcludeFromCodeCoverage]
public class ViolationException(string message) : Exception(message);