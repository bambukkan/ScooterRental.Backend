namespace RentalSystem.Core.Models;

public class BookingEntity
{
    public Guid Id {get;set;}
    public Guid UserId {get;set;}
    public UserEntity User { get; set; } = null!;

    public Guid ScooterId {get;set;}
    public ScooterEntity Scooter { get; set; } = null!;
    public decimal Price {get;set;} /*
    По идее вот цена за аренду будет высчитывать endtime - startTime и округленное
    и умноженное на какое нибудь число. По типу там в приложение 5.69 рублей в минуту. Ну вот так
    */
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}