using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

[ExcludeFromCodeCoverage]
public class ValueException(string message) : Exception(message);