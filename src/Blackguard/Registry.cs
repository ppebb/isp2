using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blackguard;

public static class Registry {
    public class Definition {
        public string Name = "Unknown";
        public int Id => SumString(Name);
    }

    private class RegistryForType() {
        internal Dictionary<Type, Definition> defsByType = new();
        internal Dictionary<int, Definition> defsById = new();
    }

    private static readonly Dictionary<Type, RegistryForType> registriesByDefinitionType = new();

    public static void InitializeDefinitionType<T>() where T : Definition {
        RegistryForType reg = new();
        registriesByDefinitionType.Add(typeof(T), reg);

        foreach (Type t in (Assembly.GetAssembly(typeof(Registry)) ?? throw new Exception("Unable to get assembly for " + typeof(T).Name)).GetTypes()) {
            // Don't need to check nested types, because I don't plan on defining any

            if (t.IsSubclassOf(typeof(T))) {
                T instance = (T)(Activator.CreateInstance(t) ?? throw new Exception($"Unable to create instance of {t}"));
                reg.defsByType.Add(t, instance);
                reg.defsById.Add(instance.Id, instance);
            }
        }
    }

    private static int SumString(string s) {
        int ret = 0;

        foreach (char c in s)
            ret += c;

        return ret;
    }

    // These are very safe functions
    // Given a derived definition (Dirt, Grass, ...) get the appropriate definition instance
    public static T GetDefinition<T>() where T : Definition {
        Type t = typeof(T);
        return (T)registriesByDefinitionType[t.BaseType].defsByType[t];
    }

    // Given a definition type (TileDefinition, ItemDefinition, ...), and an id, get a derived definition (Dirt, Grass, ...) instance
    public static T GetDefinition<T>(int id) where T : Definition {
        return (T)registriesByDefinitionType[typeof(T)].defsById[id];
    }

    // Given a derived definition (Dirt, Grass, ...) get the appropriate id
    public static int GetId<T>() {
        Type t = typeof(T);
        return registriesByDefinitionType[t].defsByType[t].Id;
    }
}
