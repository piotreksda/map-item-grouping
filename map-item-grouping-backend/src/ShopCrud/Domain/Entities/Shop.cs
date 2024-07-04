using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MapSolution.Domain.Entities;

public class Shop
{
    public Shop(string name, string city, string postalCode, string street, string number, decimal latitude, decimal longitude)
    {
        Id = Guid.NewGuid();
        Name = name;

        City = city;
        PostalCode = postalCode;
        Street = street;
        Number = number;
        Latitude = latitude;
        Longitude = longitude;
        
        Profit = 0;
        Expenses = 0;
    }
    
    [BsonId]
    public Guid Id { get; private set; }
    [BsonElement("name")]
    public string Name { get; private set; }
    [BsonElement("city")]
    public string City { get; private set; }
    [BsonElement("postalCode")]
    public string PostalCode { get; private set; }
    [BsonElement("street")]
    public string Street { get; private set; }
    [BsonElement("number")]
    public string Number { get; private set; }
    [BsonElement("latitude")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Latitude { get; private set; }
    [BsonElement("longitude")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Longitude { get; private set; }
    
    [BsonElement("profit")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Profit { get; private set; }
    [BsonElement("expenses")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Expenses { get; private set; }

    // public void EarnMoney(decimal money)
    // {
    //     Profit += money;
    // }
    //
    // public void SpendMoney(decimal money)
    // {
    //     Expenses += money;
    // }
}