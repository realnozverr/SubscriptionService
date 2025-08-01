using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

[ExcludeFromCodeCoverage]
public class ValueIsRequiredException(string message = "Value is required") : ValueException(message);