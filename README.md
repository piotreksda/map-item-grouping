# Map Item Grouping Solution

***IN PROGRESS!***

This project is a fullstack solution designed for grouping items in an external database specifically for maps, utilizing Uber H3 for grouping. The current implementation includes a CRUD system for managing shops (as an example).

## Tech Stack

- **Kafka**: Used for handling event streaming and real-time data updates.
- **.NET 8**: The primary framework for building the backend services, providing a robust and scalable solution.
- **React**: The front-end framework used for building a dynamic and responsive user interface.
- **MongoDB**: The database used for storing and managing shop data, providing flexibility with its NoSQL structure.

## Why?

I considered how to make data operations as efficient as possible. Given that the data will be sharded, choosing the appropriate shard key was crucial. The map will be essential for analysis.

I had two options to choose from:

1. **Geolocation**
   - `+` Easy to implement for the map and fast for map operations.
   - `-` Accessing the resource by ID would be difficult, and operations on the shop would mainly occur in the shop's context. Additionally, since the overall architecture will follow DDD and RESTful API principles, accessing resources will be based on their ID.

2. **Resource ID**
   - `+` Easy and fast access to the resource.
   - `-` Map-related endpoints would be extremely slow, which is problematic since the map is key to analysis.

## Conclusions

I have decided to choose the ID as the key for the shards and I will keep the map-related data in an external database. Through events with `MapCrud`, the data in the database responsible for the maps will be updated.

## How This Will Work

`Uber H3` splits the world into cells with different resolutions. You can [read more](https://h3geo.org) about `Uber H3`.

1. The React side of the solution contains logic to collect only H3 cells that are currently visible.
   ![image](https://github.com/piotreksda/map-item-grouping/assets/23263384/3b915a5e-55b6-47d0-92f4-1cf412b90b00)
2. This list of cells will be sent as a parameter.
3. **/map/...** endpoint will process simple queries on already aggregated data in the secondary database.
4. It's that easy.

## To Do

- Move logic from controller to CQRS (using MediatR).
- Implement Kafka integration.
- Implement backend for maps.
- Add tabs to the UI: one for the map and another for a table.
- Add pagination to the GET requests.
