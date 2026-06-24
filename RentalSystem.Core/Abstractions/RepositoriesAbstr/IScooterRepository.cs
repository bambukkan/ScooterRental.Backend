using RentalSystem.Core.Models;

public interface IScooterRepository
{
    public Task<List<ScooterEntity>> Get();

    public Task<List<ScooterEntity>> GetWithDetails();
    public Task<ScooterEntity> GetBySerialNumber(string serNum);
    public Task Add(ScooterEntity Scooter);
    public Task Delete(Guid id);
    public Task Update(Guid id,UpdateScooterRequest request);

}