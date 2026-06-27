using RentalSystem.Core.Models;
using RentalSystem.Core.Exceptions;
public class ScooterService : IScooterService
{
    private readonly IScooterRepository _ScooterRepository;

    public ScooterService(IScooterRepository ScooterRepository)
    {
        _ScooterRepository = ScooterRepository;
    }
    public async Task<List<ScooterEntity>> Get()
    {
        return await _ScooterRepository.Get();
    }
    public async Task<List<ScooterEntity>> GetWithDetails()
    {
        return await _ScooterRepository.GetWithDetails();
    }
    public async Task<ScooterEntity> GetBySerialNumber(string serNum)
    {
        return await _ScooterRepository.GetBySerialNumber(serNum);
    }
    public async Task<Guid> Add(CreatingScooterRequest request)
    {
        var existingScooter = await _ScooterRepository.GetBySerialNumber(request.SerialNumber);
        if(existingScooter != null)
        {
            throw new ScooterAlreadyExistsException(request.SerialNumber);
        }
        ScooterEntity scooter = new ScooterEntity()
        {
            Id = Guid.NewGuid(),
            SerialNumber = request.SerialNumber  
        };
        await _ScooterRepository.Add(scooter);
        return scooter.Id;
    }
    public async Task Delete(Guid id)
    {
        await _ScooterRepository.Delete(id);
    }
    public async Task Update(Guid id,UpdateScooterRequest request)
    {
        await _ScooterRepository.Update(id,request);
    }
}