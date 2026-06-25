
namespace RentalSystem.Core.Models;

public class UserEntity
{
    public Guid Id {get;set;}

    public string Name{get; set;} = string.Empty; 
    public string Surname{get; set;} = string.Empty; 
    public string PasswordHash {get;  set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public UserRole Role {get;set;} = UserRole.User;
    //По умолчанию все регистрируются как обычные юзеры
    public List<BookingEntity> Bookings { get; set; } = new ();
    /*
    Короче, сделаю ограничение, пока не знаю где, но наверное в сервисе надо будет,
    что пользователь может только 2 самоката арендовать, тобишь иметь 2 самоката и 2 аренды
    Тип вот в whoosh сдлеано что человек может арендовать максимум 2 самоката, но на втором самокате может поехать его друг допустим
    но друг не будет пользователем самоката, а будет тот кто покупал
    */
}
