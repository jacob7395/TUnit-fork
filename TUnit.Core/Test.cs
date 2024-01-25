﻿using System.Reflection;
using TUnit.Core.Attributes;

namespace TUnit.Core;

public record Test
{
    public Test(MethodInfo MethodInfo,
        SourceLocation SourceLocation,
        ParameterArgument[]? arguments)
    {
        var classType = MethodInfo.DeclaringType!;
        
        this.MethodInfo = MethodInfo;
        this.SourceLocation = SourceLocation;
        
        ParameterTypes = arguments?.Select(x => x.Type).ToArray();
        ArgumentValues = arguments?.Select(x => x.Value).ToArray();
        
        TestName = MethodInfo.Name;
        ClassName = classType.Name;
        FullyQualifiedClassName = classType.FullName!;
        Assembly = classType.Assembly;
        Source = classType.Assembly.Location;
        FullyQualifiedName = $"{classType.FullName}.{MethodInfo.Name}{GetParameterTypes(ParameterTypes)}";
        IsSkipped = MethodInfo.CustomAttributes
            .Concat(classType.CustomAttributes)
            .Any(x => x.AttributeType == typeof(SkipAttribute));

        FileName = SourceLocation.FileName;
        MinLineNumber = SourceLocation.MinLineNumber;
        MaxLineNumber = SourceLocation.MaxLineNumber;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string TestName { get; }

    public string ClassName { get; }
    
    public string FullyQualifiedClassName { get; set; }

    public Assembly Assembly { get; }
    
    public string Source { get; }
    public string FullyQualifiedName { get; }
    public MethodInfo MethodInfo { get; init; }
    public string? FileName { get; set; }
    public int MinLineNumber { get; set; }
    public int MaxLineNumber { get; set; }
    public Type[]? ParameterTypes { get; init; }
    public object?[]? ArgumentValues { get; init; }
    public SourceLocation SourceLocation { get; }
    
    public bool IsSkipped { get; }

    public static string GetParameterTypes(Type[]? types)
    {
        if (types is null)
        {
            return string.Empty;
        }

        var argsAsString = types.Select(arg => arg.FullName!);
        
        return $"({string.Join(',', argsAsString)})";
    }

    private static string StringifyArgument(object? obj)
    {
        return obj switch
        {
            null => "null",
            string stringValue => $"\"{stringValue}\"",
            _ => obj.ToString() ?? string.Empty
        };
    }
}