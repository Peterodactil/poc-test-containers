db.createCollection("climas");
db.climas.insertMany([
    {
        "_id": 1,
        "Date": "2024-04-10",
        "TemperatureC": 25,
        "Summary": "normal"
    },
    {
        "_id": 2,
        "Date": "2024-04-20",
        "TemperatureC": 16,
        "Summary": "frio"
    },
    {
        "_id": 3,
        "Date": "2024-04-28",
        "TemperatureC": 37,
        "Summary": "quente"
    }
]);