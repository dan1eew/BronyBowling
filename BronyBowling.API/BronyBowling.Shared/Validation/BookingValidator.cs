//namespace BronyBowling.Shared.Validation;
//public class BookingValidator 
//{
//    public static List<string> Validate(DateTime start, DateTime end, int laneId)
//    {
//        var errors = new List<string>();

//        if (laneId <= 0 || laneId > 20)
//            errors.Add("Некорректная дорожка");

//        if (end <= start)
//            errors.Add("Время окончания должно быть позже начала");

//        if (start < DateTime.UtcNow.AddMinutes(-5))
//            errors.Add("Нельзя бронировать в прошлом");

//        return errors;
//    }
//}
