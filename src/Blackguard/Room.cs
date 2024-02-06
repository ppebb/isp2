using Blackguard.Entities;
using Blackguard.Tiles;

namespace Blackguard;

public class Room {
    public Entity[] Entities;

    public Tile[][] Tiles; // Tiles will eventually be loaded from prefabs
}
