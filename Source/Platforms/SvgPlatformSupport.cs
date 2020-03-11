namespace Svg.Platforms
{
    /// <summary>
    /// Centralized Class for Implementing Platform Specific Code
    /// </summary>
    public static class PlatformSupport
    {
        private static int? pointsPerInch;

        private static ISvgPlatformSupport instance;

        /// <summary>
        /// Gets or sets Svg Platform Support, the Default Implementation works for Desktop .Net Frameworks
        /// </summary>
        public static ISvgPlatformSupport Instance
        {
            get => instance ?? (instance = GetDefault());

            set
            {
                // Reset point
                pointsPerInch = null;
                instance = value;
            }
        }

        // Points Per Inch
        public static int PointsPerInch => pointsPerInch ?? (int)(pointsPerInch = Instance.GetSystemDpi());

        private static ISvgPlatformSupport GetDefault()
        {
            return new SvgDefaultPlatformSupport();
        }
    }
}
