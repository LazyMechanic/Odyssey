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

        public EcsEntity entity
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
            this.world = world;
            return this;
        }
        public EntityBuilder CreateEntity()
        {
            return CreateEntity(out EcsEntity _);
        }

        public EntityBuilder CreateEntity(out EcsEntity outEntity)
        {
            entity = world.CreateEntity();
            outEntity = entity;
            return this;
        }

        public EntityBuilder AddComponent<T>() where T : class, new()
        {
            return AddComponent<T>(out T _);
        }

        public EntityBuilder AddComponent<T>(out T component) where T : class, new()
        {
            component = world.AddComponent<T>(entity);
            return this;
        }

        public EntityBuilder ClearEntity()
        {
            entity = 0;
            return this;
        }
    }
}