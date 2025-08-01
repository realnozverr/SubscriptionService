using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

[ExcludeFromCodeCoverage]
public class ValueIsNullException(string message = "Value is null") : ValueException(message);