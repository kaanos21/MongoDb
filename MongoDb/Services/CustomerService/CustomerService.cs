using AutoMapper;
using MongoDB.Driver;
using MongoDb.Dtos.CustomerDtos;
using MongoDb.Entities;
using MongoDb.Settings;

namespace MongoDb.Services.CustomerService
{
    public class CustomerService:ICustomerService
    {
        private readonly IMongoCollection<Customer> _CustomerCollection;
        private readonly IMapper _mapper;
        public CustomerService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _CustomerCollection = database.GetCollection<Customer>(_databaseSettings.CustomerCollectionName);
            _mapper = mapper;
        }
        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var value = _mapper.Map<Customer>(createCustomerDto);
            await _CustomerCollection.InsertOneAsync(value);
        }
        public async Task DeleteCustomerAsync(string id)
        {
            await _CustomerCollection.DeleteOneAsync(x => x.CustomerId == id);
        }
        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            var values = await _CustomerCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCustomerDto>>(values);
        }
        public async Task<GetByIdCustomerDto> GetByIdCustomerAsync(string id)
        {
            var values = await _CustomerCollection.Find<Customer>(x => x.CustomerId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdCustomerDto>(values);
        }
        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var value = _mapper.Map<Customer>(updateCustomerDto);
            await _CustomerCollection.FindOneAndReplaceAsync(x => x.CustomerId == updateCustomerDto.CustomerId, value);
        }
    }
}
