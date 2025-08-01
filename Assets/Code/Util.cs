
using UnityEngine;

namespace Code
{
    public static class Util
    {
        /// <summary>
        /// Returns a normalized value between 0 - 1
        /// </summary>
        /// <param name="value">number being normalized</param>
        /// <param name="min">The min range of value</param>
        /// <param name="max">the max range of value</param>
        /// <returns></returns>
        public static float Normalize( float value, float min, float max )
        {
            var divisor = (max - min);
            if( divisor == 0 )
            {
                return 0;
            }
            return (value - min) / divisor;
        }
        
        /// <summary>
        /// Get the normalized direction between two positions.
        /// based on: https://docs.unity3d.com/2019.3/Documentation/Manual/DirectionDistanceFromOneObjectToAnother.html
        /// </summary>
        /// <param name="sourcePos">The source position</param>
        /// <param name="targetPos">The target position to calculate direction towards</param>
        public static Vector3 NormalizedDirection( Vector3 sourcePos, Vector3 targetPos )
        {
            var heading = targetPos - sourcePos;
            return heading / heading.magnitude;
        }
    }
}