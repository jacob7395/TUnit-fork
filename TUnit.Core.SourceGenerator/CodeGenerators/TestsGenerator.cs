﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using TUnit.Core.SourceGenerator.CodeGenerators.Helpers;
using TUnit.Core.SourceGenerator.CodeGenerators.Writers;
using TUnit.Core.SourceGenerator.Extensions;
using TUnit.Core.SourceGenerator.Models;

namespace TUnit.Core.SourceGenerator.CodeGenerators;

[Generator]
internal class TestsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var standardTests = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "TUnit.Core.TestAttribute",
                predicate: static (_, _) => true,
                transform: static (ctx, _) =>
                    new TestCollectionDataModel(GetSemanticTargetForTestMethodGeneration(ctx)))
            .Where(static m => m is not null);
        
        var inheritedTests = context.SyntaxProvider
            .ForAttributeWithMetadataName("TUnit.Core.InheritsTestsAttribute",
                predicate: static (_, _) => true,
                transform: static (ctx, _) => GetSemanticTargetForInheritedTestsGeneration(ctx))
            .Where(static m => m is not null);
        
        context.RegisterSourceOutput(standardTests, (sourceContext, data) => GenerateTests(sourceContext, data));
        context.RegisterSourceOutput(inheritedTests, (sourceContext, data) => GenerateTests(sourceContext, data!, "Inherited_"));
    }

    static IEnumerable<TestSourceDataModel> GetSemanticTargetForTestMethodGeneration(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not IMethodSymbol methodSymbol)
        {
            yield break;
        }

        if (methodSymbol.ContainingType.IsAbstract)
        {
            yield break;
        }

        if (methodSymbol.IsStatic)
        {
            yield break;
        }

        if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
        {
            yield break;
        }

        foreach (var testSourceDataModel in methodSymbol.ParseTestDatas(context, methodSymbol.ContainingType))
        {
            yield return testSourceDataModel;
        }
    }
    
    static TestCollectionDataModel? GetSemanticTargetForInheritedTestsGeneration(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return null;
        }
        
        if (namedTypeSymbol.IsAbstract)
        {
            return null;
        }

        if (namedTypeSymbol.IsStatic)
        {
            return null;
        }

        if (namedTypeSymbol.DeclaredAccessibility != Accessibility.Public)
        {
            return null;
        }

        return new TestCollectionDataModel(
            namedTypeSymbol.GetBaseTypes()
                .SelectMany(x => x.GetMembers())
                .OfType<IMethodSymbol>()
                .Where(x => !x.IsAbstract)
                .Where(x => x.MethodKind != MethodKind.Constructor)
                .Where(x => x.IsTest())
                .SelectMany(x => x.ParseTestDatas(context, namedTypeSymbol))
        );
    }

    private void GenerateTests(SourceProductionContext context, TestCollectionDataModel testCollection, string? prefix = null)
    {
        foreach (var classGrouping in testCollection
                     .TestSourceDataModels
                     .GroupBy(x => $"{prefix}{x.ClassNameToGenerate}_{Guid.NewGuid():N}"))
        {
            var className = classGrouping.Key;

            using var sourceBuilder = new SourceCodeWriter();

            sourceBuilder.WriteLine("// <auto-generated/>");
            sourceBuilder.WriteLine("#pragma warning disable");
            sourceBuilder.WriteLine("using global::TUnit.Core;");
            sourceBuilder.WriteLine("using global::System.Reflection;");
            sourceBuilder.WriteLine("using global::System.Linq;");
            sourceBuilder.WriteLine();
            sourceBuilder.WriteLine("namespace TUnit.SourceGenerated;");
            sourceBuilder.WriteLine();
            sourceBuilder.WriteLine("[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
            sourceBuilder.WriteLine($"file partial class {className} : TUnit.Core.Interfaces.SourceGenerator.ITestSource");
            sourceBuilder.WriteLine("{");

            sourceBuilder.WriteLine("[global::System.Runtime.CompilerServices.ModuleInitializer]");
            sourceBuilder.WriteLine("public static void Initialise()");
            sourceBuilder.WriteLine("{");
            sourceBuilder.WriteLine($"SourceRegistrar.Register(new {className}());");
            sourceBuilder.WriteLine("}");

            sourceBuilder.WriteLine("public global::System.Collections.Generic.IReadOnlyList<SourceGeneratedTestNode> CollectTests()");
            sourceBuilder.WriteLine("{");
            sourceBuilder.WriteLine("return");
            sourceBuilder.WriteLine("[");
            for (var i = 0; i < classGrouping.Count(); i++)
            {
                sourceBuilder.WriteLine($"..Tests{i}(),");
            }
            sourceBuilder.WriteLine("];");
            sourceBuilder.WriteLine("}");

            var index = 0;
            foreach (var model in classGrouping)
            {
                sourceBuilder.WriteLine($"private global::System.Collections.Generic.List<SourceGeneratedTestNode> Tests{index++}()");
                sourceBuilder.WriteLine("{");
                sourceBuilder.WriteLine("global::System.Collections.Generic.List<SourceGeneratedTestNode> nodes = [];");
                sourceBuilder.WriteLine($"var {VariableNames.ClassDataIndex} = 0;");
                sourceBuilder.WriteLine($"var {VariableNames.TestMethodDataIndex} = 0;");

                sourceBuilder.WriteLine("try");
                sourceBuilder.WriteLine("{");
                GenericTestInvocationWriter.GenerateTestInvocationCode(sourceBuilder, model);
                sourceBuilder.WriteLine("}");
                sourceBuilder.WriteLine("catch (global::System.Exception exception)");
                sourceBuilder.WriteLine("{");
                FailedTestInitializationWriter.GenerateFailedTestCode(sourceBuilder, model);
                sourceBuilder.WriteLine("}");
                
                sourceBuilder.WriteLine("return nodes;");
                sourceBuilder.WriteLine("}");
            }

            sourceBuilder.WriteLine("}");

            context.AddSource($"{className}.Generated.cs", sourceBuilder.ToString());
        }
    }
}