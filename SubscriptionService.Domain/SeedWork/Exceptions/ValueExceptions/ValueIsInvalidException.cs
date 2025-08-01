using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

[ExcludeFromCodeCoverage]
public class ValueIsInvalidException(string message = "Value is invalid") :  ValueException(message);