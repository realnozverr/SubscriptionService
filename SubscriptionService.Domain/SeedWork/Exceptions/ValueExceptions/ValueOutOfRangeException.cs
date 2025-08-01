using System.Diagnostics.CodeAnalysis;

namespace SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

[ExcludeFromCodeCoverage]
public class ValueOutOfRangeException(string message = "Value out of range") : ValueException(message);