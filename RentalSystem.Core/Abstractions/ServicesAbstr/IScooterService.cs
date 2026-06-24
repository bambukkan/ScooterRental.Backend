using RentalSystem.Core.Models;
public interface IScooterService
{
    public Task<List<ScooterEntity>> Get();
    public Task<List<ScooterEntity>> GetWithDetails();
    public Task<ScooterEntity> GetBySerialNumber(string serNum);
    public Task<Guid> Add(CreatingScooterRequest request);
    public Task Delete(Guid id);
    public Task Update(Guid id,UpdateScooterRequest request);
}