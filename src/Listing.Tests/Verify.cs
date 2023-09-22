using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Listing.Code;

namespace Listing.Tests;

public static class Verify
{
    [MustUseReturnValue]
    public static Task Output(Output output, [CallerFilePath] string path = "")
    {
        var settings = new VerifySettings();
        settings.UseDirectory("Verified");
        settings.UseFileName(TestContext.CurrentContext.Test.Name);
        var text = output.ToString();
        // ReSharper disable ExplicitCallerInfoArgument
        return Verify(text, settings, sourceFile: path);
        // ReSharper restore ExplicitCallerInfoArgument
    }
}