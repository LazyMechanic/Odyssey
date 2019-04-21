using Leopotam.Ecs;

namespace Odyssey
{
    public class EntityBuilder
    {
        public EcsWorld world
        {
            get;
            private set;
        }

        public int entityId
        {
            get;
            private set;
        }

        protected EntityBuilder() { }

        public static EntityBuilder Instance(EcsWorld world)
        {
            EntityBuilder eb = new EntityBuilder();
            eb.world = world;
            return eb;
        }

        public EntityBuilder SetWorld(EcsWorld world)
        {
            world = world;
            return this;
        }
        public EntityBuilder CreateEntity()
        {
            return CreateEntity(out int _);
        }

        public EntityBuilder CreateEntity(out int outEntity)
        {
            entityId = world.CreateEntity();
            outEntity = entityId;
            return this;
        }

        public EntityBuilder AddComponent<T>() where T : class, new()
        {
            return AddComponent<T>(out T _);
        }

        public EntityBuilder AddComponent<T>(out T component) where T : class, new()
        {
            component = world.AddComponent<T>(entityId);
            return this;
        }

        public EntityBuilder ClearEntity()
        {
            entityId = 0;
            return this;
        }
    }
}