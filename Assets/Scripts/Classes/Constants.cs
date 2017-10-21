using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes
{
    public class Constans
    {
        public static float MaxCamaraOrthographicSize = 6f;
        public static float MinCamaraOrthographicSize = 5f;

        public static float MinGameTimeSpeed = 0f;
        public static float MaxMaxTimeSpeed = 2f;

        public static float MaxNumberSpawnRadious;

        public static float SpawnNumberTime = 5;
        public static float SpawnNumberDeltaTime = 4;

        public static float MinNumberRange = 9;
        public static float MaxNumberRange = 16;

        public const float HoursToDegrees = 360f / 12f;
        public const float MinutesToDegrees = 360f / 60f;
        public static float SecondsToDegrees = 360f / 60f;
    }
}
