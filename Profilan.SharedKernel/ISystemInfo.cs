using System;

namespace Profilan.SharedKernel
{
    public interface ISystemInfo
    {
        int State { get; set; }
        DateTime Created { get; set; }
        int CreatedBy { get; set; }
        DateTime Modified { get; set; }
        int ModifiedBy { get; set; }
    }
}
