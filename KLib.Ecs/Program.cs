//entity
//id
//components

using System.Collections;

public struct Entity
{
    public int Id { get; }
    public BitArray Components { get; }
    public Scene Scene { get; }

    public Entity(Scene scene)
    {
        Id = scene.GetNextEntityId();
        Components = new(Ecs.ComponentIds.Count);
        Scene = scene;
    }

    public void AddComponent<TComponent>() 
    {
        if(Ecs.ComponentIds.TryGetValue(typeof(TComponent), out var componentId))
        {
            Scene.MapEntityToComponent(Id, componentId);
            Components[componentId] = true;
        }
    }

    public TComponent? GetComponent<TComponent>()
    {
        if (Ecs.ComponentIds.TryGetValue(typeof(TComponent), out var componentId))
        {
            if(Components[componentId])
            {
                return (TComponent)Scene.GetComponentByEntity(componentId, Id);
            }
        }
        return default;
    }
}

//component
//id (power of 2)

public interface IComponent
{

}

//system
//processes components

public class Scene
{
    public int GetNextEntityId()
    {
        throw new NotImplementedException();
    }

    internal object GetComponentByEntity(int componentId, int id)
    {
        throw new NotImplementedException();
    }

    internal void MapEntityToComponent(int id, int componentId)
    {
        throw new NotImplementedException();
    }
}

public class ComponentAttribute : Attribute
{
}

public static class Ecs
{
    public static Dictionary<Type, int> ComponentIds { get; }

    static Ecs()
    {
        ComponentIds = new();

        int currentComponentId = 0;
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach(var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ComponentAttribute), false);
                if(attributes.Length > 0)
                {
                    ComponentIds.Add(type, currentComponentId);
                    currentComponentId++;
                    
                    if(currentComponentId == 32)
                    {
                        Console.WriteLine("Maximum number of components has been reached.");
                        return;
                    }
                }
            }
        }
    }
}