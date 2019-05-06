namespace Odyssey
{
    sealed class AxisComponent
    {
        public float horizontal;
        public float vertical;
        public float thrust;

        public float lastHorizontal;
        public float lastVertical;
        public float lastThrust;

        public float DeltaHorizontal()
        {
            return horizontal - lastHorizontal;
        }

        public float DeltaVertical()
        {
            return vertical - lastVertical;
        }

        public float DeltaThrust()
        {
            return thrust - lastThrust;
        }
    }
}
