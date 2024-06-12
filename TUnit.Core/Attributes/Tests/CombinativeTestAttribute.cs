using System.Runtime.CompilerServices;

namespace TUnit.Core;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CombinativeTestAttribute : BaseTestAttribute
{
    public CombinativeTestAttribute([CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) : base(file, line)
    {
    }
}