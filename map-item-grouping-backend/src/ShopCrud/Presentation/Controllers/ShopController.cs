using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MapSolution.Configurations;
using MapSolution.Domain.Entities;
using MapSolution.Domain.Models.Create;
using MapSolution.Domain.Models.Edit;
using MapSolution.Domain.Models.Get;

namespace MapSolution.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IMongoCollection<Shop> _shopCollection;
        private readonly MongoOptions _mongoOptions;

        public ShopController(IMongoClient mongoClient, MongoOptions mongoOptions)
        {
            _mongoOptions = mongoOptions;
            var database = mongoClient.GetDatabase(_mongoOptions.Name);
            _shopCollection = database.GetCollection<Shop>("Shops");
        }

        [HttpGet]
        public async Task<ActionResult<List<ShopListDto>>> GetShopList()
        {
            var projection = Builders<Shop>.Projection.Expression(s => new ShopListDto
            {
                Id = s.Id,
                Name = s.Name,
                Profit = s.Profit,
                Expenses = s.Expenses
            });

            var shopList = await _shopCollection.Find(_ => true).Project(projection).ToListAsync();
            return Ok(shopList);
        }

        [HttpGet("{shopId:guid}")]
        public async Task<ActionResult<ShopDetailDto>> GetShopDetail([FromRoute] Guid shopId)
        {
            var projection = Builders<Shop>.Projection.Expression(s => new ShopDetailDto
            {
                Id = s.Id,
                Name = s.Name,
                City = s.City,
                PostalCode = s.PostalCode,
                Street = s.Street,
                Number = s.Number,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Profit = s.Profit,
                Expenses = s.Expenses
            });

            var shop = await _shopCollection.Find(s => s.Id == shopId).Project(projection).FirstOrDefaultAsync();
            if (shop == null)
            {
                return NotFound();
            }

            return Ok(shop);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewShop([FromBody] CreateShopInputModel model)
        {
            var shop = new Shop(model.Name, model.City, model.PostalCode, model.Street, model.Number, model.Latitude,
                model.Longitude);

            await _shopCollection.InsertOneAsync(shop);

            return CreatedAtAction(nameof(GetShopDetail), new { shopId = shop.Id }, shop);
        }

        [HttpPut("{shopId:guid}")]
        public async Task<ActionResult> EditShopDetail([FromRoute] Guid shopId, [FromBody] EditShopInputModel model)
        {
            var update = Builders<Shop>.Update.Set(s => s.Name, model.Name);

            var result = await _shopCollection.UpdateOneAsync(s => s.Id == shopId, update);
            
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPatch("{shopId:guid}/earnMoney")]
        public async Task<ActionResult> EarnMoney([FromRoute] Guid shopId, [FromBody] decimal amount)
        {
            var update = Builders<Shop>.Update.Inc(s => s.Profit, amount);

            var result = await _shopCollection.UpdateOneAsync(s => s.Id == shopId, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{shopId:guid}/spendMoney")]
        public async Task<ActionResult> SpendMoney([FromRoute] Guid shopId, [FromBody] decimal amount)
        {
            var update = Builders<Shop>.Update.Inc(s => s.Expenses, amount);

            var result = await _shopCollection.UpdateOneAsync(s => s.Id == shopId, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
