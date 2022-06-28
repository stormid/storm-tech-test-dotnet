namespace Storm.TechTask.SharedKernel.Authorization
{
    [Flags]
    public enum AppRole : uint
    {
        Anonymous = 0x00000001,
        SysAdmin = 0x00000002,
        ProjectAdmin = 0x00000004,
        ProjectReader = 0x00000008,
        // Etc
        MAX = 0x80000000    // If we need to go beyond this, then declare enum as a ulong.
    }
}
