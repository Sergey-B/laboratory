using System.Text.RegularExpressions;

namespace Task1_1;

#region Rules
// 1. Запрещены else и любые цепочки else if.  
// 2. Запрещены if, вложенные в if, и циклы, вложенные в if.  
// 3. if внутри цикла допускается только один, и только для прерывания его работы (break/continue), выхода из функции (return) или генерации исключения.
// 4. Не определять методы, которые ничего не вычисляют
#endregion

#region Example 1

internal class Order
{
    public bool IsPaid { get; internal set; }
    public int Amount { get; internal set; }
    public int RetryCount { get; internal set; }
    public IEnumerable<OrderItem>? Items { get; internal set; } = [];
}

internal class User
{
    public bool IsActive { get; internal set; }
}

internal class OrderItem
{
    public int Price { get; internal set; }
}

// CC 18
class OrderProcessor
{

    public string ProcessOrder(Order order, User user, bool isExpress, bool isHoliday)
    {
        string status = "Pending";

        if (order != null) // +1
        {
            if (order.IsPaid) // +1
            {
                if (user != null) // +1
                {
                    if (user.IsActive) // +1
                    {
                        if (isExpress) // +1
                        {
                            if (order.Amount > 1000) // +1
                            {
                                status = "Priority Express";
                            }
                            else // +1
                            {
                                status = "Standard Express";
                            }
                        }
                        else // +1
                        {
                            if (isHoliday) // +1
                            {
                                status = "Holiday Delayed";
                            }
                            else // +1
                            {
                                status = "Normal Processing";
                            }
                        }
                    }
                    else // +1
                    {
                        status = "User Inactive";
                    }
                }
                else // +1
                {
                    status = "User Not Found";
                }
            }
            else // +1
            {
                if (order.RetryCount > 3) // +1
                {
                    status = "Cancelled";
                }
                else // +1
                {
                    status = "Waiting For Payment";
                }
            }
        }
        else // +1
        {
            status = "Invalid Order";
        }

        foreach (var item in order?.Items ?? Enumerable.Empty<OrderItem>()) // +1
        {
            if (item.Price < 0) // +1
            {
                status = "Error: Negative Item Price";
            }
        }

        return status;
    }
}

// CC 4
class OrderProcessorReducedCC
{
    public string ProcessOrder(Order order, User user, bool isExpress, bool isHoliday)
    {
        var orderValidationResult = ValidateOrder(order);
        if (orderValidationResult is not OrderValidationResult.Valid)
        {
            return orderValidationResult;
        }

        var userValidationResult = ValidateUser(user);
        if (userValidationResult is not UserValidationResult.Valid)
        {
            return userValidationResult;
        }

        if (isExpress)
        {
            return GetExpressDeliveryStatus(order);
        }

        if (isHoliday)
        {
            return "Holiday Delayed";
        }

        return "Normal Processing";
    }

    private static string GetExpressDeliveryStatus(Order order)
    {
        if (order.Amount > 1000)
        {
            return "Priority Express";
        }

        return "Standard Express";
    }

    private static class OrderValidationResult
    {
        public const string Valid = "Valid";
        public const string InvalidOrder = "Invalid Order";
        public const string Cancelled = "Cancelled";
        public const string WaitingForPayment = "Waiting For Payment";
        public const string NegativeItemPrice = "Error: Negative Item Price";
    }

    private static class UserValidationResult
    {
        public const string Valid = "Valid";
        public const string UserNotFound = "User Not Found";
        public const string UserInactive = "User Inactive";
    }

    private static string ValidateUser(User user)
    {
        if (user is null)
        {
            return UserValidationResult.UserNotFound;
        }

        if (!user!.IsActive)
        {
            return UserValidationResult.UserInactive;
        }

        return UserValidationResult.Valid;
    }

    private static string ValidateOrder(Order order)
    {
        if (order is null)
        {
            return OrderValidationResult.InvalidOrder;
        }

        if (order.IsPaid)
        {
            return OrderValidationResult.Valid;
        }

        if (order.RetryCount > 3)
        {
            return OrderValidationResult.Cancelled;
        }

        if (order!.Items!.Any(item => item.Price < 0))
        {
            return OrderValidationResult.NegativeItemPrice;
        }

        return OrderValidationResult.WaitingForPayment;
    }
}

#endregion

#region Example 2

// CC 25
public class DeliveryManager
{
    public double CalculateDeliveryCost(string transportType, double distance, double weight, bool isUrgent, bool isHoliday)
    {
        double baseCost = 0;

        try // +1
        {
            baseCost = transportType switch
            {
                "Drone" when distance < 10 && weight <= 2 && !isHoliday => 50.0, // +3
                "Drone" when distance < 15 && isUrgent => 80.0,                   // +2

                "Courier" when weight <= 5 && (distance <= 5 || !isUrgent) => 30.0, // +3
                "Courier" when distance > 5 && distance <= 20 => 45.0,              // +2

                "Truck" when weight > 500 || distance > 500 => 500.0,              // +2
                "Truck" when weight > 100 && isUrgent => 300.0,                    // +2
                "Truck" => 150.0,                                                  // +1

                _ => throw new ArgumentException("Unknown transport type")         // +1
            };

            if (isUrgent) // +1
            {
                baseCost *= 1.5;

                if (isHoliday && transportType != "Drone") // +2
                {
                    baseCost += 50.0;
                }
            }
            else if (isHoliday) // +1
            {
                baseCost *= 1.2;
            }

            while (distance > 100) // +1
            {
                baseCost += 5.0;
                distance -= 50; // Искусственное уменьшение счетчика пути
            }
        }
        catch (ArgumentException ex) // +1
        {
            Console.WriteLine($"Log: Logistics error - {ex.Message}");
            return -1;
        }
        catch (Exception) // +1
        {
            Console.WriteLine("Log: Critical unknown error");
            return -2;
        }

        return baseCost;
    }
}

// CC 2
public class DeliveryManagerReducedCC
{
    public double CalculateDeliveryCost(string transportType, double distance, double weight, bool isUrgent, bool isHoliday)
    {
        var delivery = Delivery.Build(transportType, distance, weight, isUrgent, isHoliday);
        if (delivery.GetStatus() is DeliveryBuildStatus.BuildUnknownTransportTypeError)
        {
            Console.WriteLine("Log: Logistics error - Unknown transport type");

            return -1;
        }

        return delivery.GetBaseCost();
    }

    private class Delivery
    {
        protected string _transportType;
        protected double _distance;
        protected double _weight;
        protected double _baseCost;
        protected bool _isUrgent;
        protected bool _isHoliday;
        protected DeliveryBuildStatus _deliveryBuildStatus;
        protected const double DefaultDeliveryBaseCost = 100.0;

        protected Delivery(string transportType, double distance, double weight, bool isUrgent, bool isHoliday)
        {
            _transportType = transportType;
            _distance = distance;
            _weight = weight;
            _isUrgent = isUrgent;
            _isHoliday = isHoliday;

            _deliveryBuildStatus = DeliveryBuildStatus.BuildNil;

            _baseCost = DefaultDeliveryBaseCost;
        }

        public static Delivery Build(string transportType, double distance, double weight, bool isUrgent, bool isHoliday)
        {
            return transportType switch
            {
                nameof(TransportType.Drone) => new DroneDelivery(transportType, distance, weight, isUrgent, isHoliday),
                nameof(TransportType.Courier) => new CourierDelivery(transportType, distance, weight, isUrgent, isHoliday),
                nameof(TransportType.Truck) => new TruckDelivery(transportType, distance, weight, isUrgent, isHoliday),
                _ => new InvalidDelivery(transportType, distance, weight, isUrgent, isHoliday)
            };
        }

        public virtual double GetBaseCost()
        {
            return _baseCost;
        }

        public virtual DeliveryBuildStatus GetStatus()
        {
            return _deliveryBuildStatus;
        }

        internal double ApplyBaseCostCalculations(double cost)
        {
            switch (_isUrgent, _isHoliday)
            {
                case (true, true):
                    cost *= 1.5;
                    break;
                case (false, true):
                    cost *= 1.2;
                    break;
                default:
                    break;
            }

            var distance = _distance;
            while (distance > 100)
            {
                cost += 5.0;
                distance -= 50;
            }

            return cost;
        }
    }

    private class DroneDelivery : Delivery
    {
        public DroneDelivery(string transportType, double distance, double weight, bool isUrgent, bool isHoliday) : base(transportType, distance, weight, isUrgent, isHoliday)
        {
        }

        public override double GetBaseCost()
        {
            var cost = DefaultDeliveryBaseCost;

            cost = (_distance, _weight, _isHoliday, _isUrgent) switch
            {
                ( < 10, <= 2, false, _) => 50.0,
                ( < 15, _, _, true) => 80.0,
                _ => cost
            };

            cost = ApplyBaseCostCalculations(cost);

            return cost;
        }
    }

    private class CourierDelivery : Delivery
    {
        public CourierDelivery(string transportType, double distance, double weight, bool isUrgent, bool isHoliday) : base(transportType, distance, weight, isUrgent, isHoliday)
        {
        }

        public override double GetBaseCost()
        {
            var cost = DefaultDeliveryBaseCost;

            cost = (_distance, _weight, _isHoliday, _isUrgent) switch
            {
                (_, <= 5, _, false) => 30.0,
                ( <= 5, <= 5, _, _) => 30.0,
                ( > 5 and <= 20, _, _, _) => 45.0,
                _ => cost
            };

            cost = ApplyBaseCostCalculations(cost);

            if (_isUrgent && _isHoliday)
            {
                cost = cost + 50.0;
            }

            return cost;
        }
    }

    private class TruckDelivery : Delivery
    {
        public TruckDelivery(string transportType, double distance, double weight, bool isUrgent, bool isHoliday) : base(transportType, distance, weight, isUrgent, isHoliday)
        {
        }

        public override double GetBaseCost()
        {
            var cost = DefaultDeliveryBaseCost;

            cost = (_distance, _weight, _isHoliday, _isUrgent) switch
            {
                ( > 500, _, _, _) => 500.0,
                (_, > 500, _, _) => 500.0,
                (_, > 100, _, true) => 300.0,
                _ => 150.0
            };

            cost = ApplyBaseCostCalculations(cost);

            if (_isUrgent && _isHoliday)
            {
                cost = cost + 50.0;
            }

            return cost;
        }
    }

    private class InvalidDelivery : Delivery
    {
        public InvalidDelivery(string transportType, double distance, double weight, bool isUrgent, bool isHoliday) : base(transportType, distance, weight, isUrgent, isHoliday)
        {
        }

        public override DeliveryBuildStatus GetStatus()
        {
            return DeliveryBuildStatus.BuildUnknownTransportTypeError;
        }
    }

    internal enum DeliveryBuildStatus
    {
        BuildNil,
        BuildOk,
        BuildUnknownTransportTypeError
    }

    private enum TransportType
    {
        Drone,
        Courier,
        Truck
    }
}
#endregion

#region Example 3
public class PasswordValidator
{
    // Цикломатическая сложность метода > 20
    public List<string> FilterAndValidatePasswords(List<string> passwords, bool strictMode, bool AllowSpecialChars, int minLength)
    {
        List<string> validPasswords = new List<string>();

        if (passwords == null || passwords.Count == 0)
        {
            return validPasswords; // +1
        }

        foreach (var pwd in passwords) // +1
        {
            if (string.IsNullOrWhiteSpace(pwd) || pwd.Length < minLength) // +2
            {
                continue; // +1
            }

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            for (int i = 0; i < pwd.Length; i++) // +1
            {
                char ch = pwd[i];

                if (char.IsUpper(ch)) hasUpper = true; // +1
                else if (char.IsLower(ch)) hasLower = true; // +1
                else if (char.IsDigit(ch)) hasDigit = true; // +1
                else if (!char.IsLetterOrDigit(ch)) hasSpecial = true; // +1
            }

            if (strictMode) // +1
            {
                if (hasUpper && hasLower && hasDigit) // +2
                {
                    if (AllowSpecialChars && !hasSpecial) // +2
                    {
                        // В strict-режиме спецсимволы разрешены, но если их нет — пароль слабый
                        continue;
                    }

                    if (!AllowSpecialChars && hasSpecial) // +2
                    {
                        continue;
                    }

                    validPasswords.Add(pwd);
                }
            }
            else // Режим не строгий
            {
                if ((hasUpper || hasLower) && hasDigit) // +2
                {
                    if (hasSpecial && !AllowSpecialChars) // +2
                    {
                        break;
                    }

                    validPasswords.Add(pwd);
                }
            }
        }

        return validPasswords;
    }
}

public class PasswordValidatorReducedCC
{
    public List<string> FilterAndValidatePasswords(List<string> passwords, bool strictMode, bool AllowSpecialChars, int minLength)
    {
        if (passwords == null || passwords.Count == 0)
        {
            return [];
        }

        var validator = new CustomPasswordValidator(strictMode, AllowSpecialChars, minLength);

        return [.. passwords.Where(validator.IsValid)];
    }

    private class CustomPasswordValidator
    {
        private bool strictMode;
        private bool allowSpecialChars;
        private int minLength;

        public CustomPasswordValidator(bool strictMode, bool allowSpecialChars, int minLength)
        {
            this.strictMode = strictMode;
            this.allowSpecialChars = allowSpecialChars;
            this.minLength = minLength;
        }

        internal bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < minLength)
            {
                return false;
            }

            var hasUpper = Regex.IsMatch(password, "[A-Z]");
            var hasLower = Regex.IsMatch(password, "[a-z]");
            var hasDigit = Regex.IsMatch(password, @"\d");
            var hasSpecial = Regex.IsMatch(password, "[^A-Za-z0-9]");

            var result = (strictMode, hasUpper, hasLower, hasDigit, hasSpecial, allowSpecialChars) switch
            {
                // strictMode on
                (true, true, true, true, true, true) => true,
                (true, true, true, true, false, false) => true,
                (true, true, true, true, false, true) => false,
                (true, true, true, true, true, false) => false,

                // strictMode off
                (false, true, _, true, true, false) => false,
                (false, _, true, true, true, false) => false,
                (false, true, _, true, _, _) => true,
                (false, _, true, true, _, _) => true,
                _ => false
            };

            return result;
        }
    }
}
#endregion

#region Reflection
/* 
    Вложенные условия в оригиналах ухудшали понимание и читаемость кода. ЦС была уменьшена в несколько раз для каждого из примеров.

    Пример1
        ЦС 18 -> 4.

        Использовал декомпозицию оригинального кода и вынос кода в приватные методы. 
        Также для уменьшения вложенности использовался early return (Guard - паттерн).
        Большинство "магических строк" было помещенов в enums. Цикл foreach заменени декларативным Any.

    Пример2
        ЦС 25 -> 5.

        Избавление от else обьединением условий и использованием early return.
        Также использовался полиморфизм подтипов и табличная логика c pattern matching в switch для подсчета цены.

    Пример3:
        ЦС > 20 -> 2.

        Использование декомпозиции и выноса кода в методы, "табличная логика" в switch и pattern matching.
        Замена цикла использованием LINQ. Анализ строк через Regex.

    Также насколько я знаю в высоконагруженных приложениях .NET Core вместо LINQ рекомендуется используется for loop потому что LINQ выделяет большое количество памяти.
    Это компромисс между производительностью и сложностью кода.   
*/
#endregion