using System;
using System.Collections.Generic;
using System.Reflection;
using Blackguard.Utilities;

namespace Blackguard.Tiles;

public abstract class TileDefinition {
    public string Id = "Unknown";
    public char Glyph;
    public Highlight Highlight;

    private static Dictionary<Type, TileDefinition> defsByType = null!;
    private static Dictionary<string, TileDefinition> defsById = null!;

    // Registers every type inheriting from TileDefinition
    public static void InitializeTileDefs() {
        defsByType = new();
        defsById = new();

        foreach (Type t in (Assembly.GetAssembly(typeof(TileDefinition)) ?? throw new Exception("Unable to get assembly for TileDefinition")).GetTypes()) {
            // Don't need to check nested types, because I don't plan on defining any

            if (t.IsSubclassOf(typeof(TileDefinition))) {
                TileDefinition instance = (TileDefinition)(Activator.CreateInstance(t) ?? throw new Exception($"Unable to create instance of {t}"));
                defsByType.Add(t, instance);
                defsById.Add(instance.Id, instance);
            }
        }
    }

    public static TileDefinition GetTileDefinition<T>() {
        return defsByType[typeof(T)];
    }

    public static TileDefinition GetTileDefinition(string id) {
        return defsById[id];
    }
}
