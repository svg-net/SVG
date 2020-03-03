namespace Svg.Platforms
{
    /// <summary>
    /// Interface for Svg Platform specific Code
    /// </summary>
    public interface ISvgPlatformSupport
    {
        /// <summary> Returns the System Dpi </summary>
        /// <returns>dpi</returns>
        int GetSystemDpi();
    }
}
