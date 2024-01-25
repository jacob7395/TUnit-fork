﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using TUnit.Core;
using TUnit.TestAdapter.Constants;
using TUnit.TestAdapter.Extensions;

namespace TUnit.TestAdapter;

[FileExtension(".dll")]
[FileExtension(".exe")]
[DefaultExecutorUri(TestAdapterConstants.ExecutorUriString)]
[ExtensionUri(TestAdapterConstants.ExecutorUriString)]
public class TestExecutor : ITestExecutor2
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly AsyncTestExecutor _asyncTestExecutor;
    
    public TestExecutor()
    {
        Console.CancelKeyPress += (_, _) => _cancellationTokenSource.Cancel();
        _asyncTestExecutor = new AsyncTestExecutor(_cancellationTokenSource);
    }

    public void RunTests(IEnumerable<TestCase>? testCases, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
        if (testCases is null)
        {
            return;
        }

        var testsWithTestCases = new TestCollector(frameworkHandle).TestsFromTestCases(testCases);
            
        _asyncTestExecutor.RunInAsyncContext(testsWithTestCases, runContext, frameworkHandle).GetAwaiter().GetResult();    }

    public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
        if (sources is null)
        {
            return;
        }

        var tests = new TestCollector(frameworkHandle).TestsFromSources(sources)
            .Select(x => new TestWithTestCase(x, x.ToTestCase()));
        
        _asyncTestExecutor.RunInAsyncContext(tests, runContext, frameworkHandle).GetAwaiter().GetResult();
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }

    public bool ShouldAttachToTestHost(IEnumerable<TestCase>? tests, IRunContext runContext)
    {
        return ShouldAttachToTestHost(tests?.Select(x => x.Source), runContext);
    }

    public bool ShouldAttachToTestHost(IEnumerable<string>? sources, IRunContext runContext)
    {
        return runContext.IsBeingDebugged;
    }
}