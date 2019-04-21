namespace Odyssey
{
    sealed class PidComponent
    {
        // Proportional constant (counters current delta)
        public float kp = 10.0f;

        // Integral constant (counters cumulated delta)
        public float ki = 0.0f;

        // Derivative constant (fights oscillation)
        public float kd = 1.0f;

        // rrent control value
        public float value;

        public float lastError;

        public float integral;
    }
}