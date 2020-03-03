namespace Svg.Platforms
{
    /// <summary>
    /// Centralized Class for Implementing Platform Specific Code
    /// </summary>
    public static class PlatformSupport
    {
        /// <summary>
        /// Gets or sets Svg Platform Support, the Default Implementation works for Desktop .Net Frameworks
        /// </summary>
        public static ISvgPlatformSupport Instance { get; set; } = new SvgDefaultPlatformSupport();
    }
}
