﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using UnityEngine;

public class JIT_Compiler : MonoBehaviour
{
    public string JITContent = "";

    void Start()
    {
        var assembly = Compile(@JITContent);

        var runtimeType = assembly.GetType("RuntimeCompiled");
        var method = runtimeType.GetMethod("AddYourselfTo");
        var del = (Func<GameObject, MonoBehaviour>)
                      Delegate.CreateDelegate(
                          typeof(Func<GameObject, MonoBehaviour>),
                          method
                  );

        // We ask the compiled method to add its component to this.gameObject
        var addedComponent = del.Invoke(gameObject);

        // The delegate pre-bakes the reflection, so repeated calls don't
        // cost us every time, as long as we keep re-using the delegate.
    }

    public static Assembly Compile(string source)
    {
        // Replace this Compiler.CSharpCodeProvider wth aeroson's version
        // if you're targeting non-Windows platforms:
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();

        // Add ALL of the assembly references
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            param.ReferencedAssemblies.Add(assembly.Location);
        }

        // Or, uncomment just the assemblies you need...

        // System namespace for common types like collections.
        //param.ReferencedAssemblies.Add("System.dll");

        // This contains methods from the Unity namespaces:
        //param.ReferencedAssemblies.Add("UnityEngines.dll");

        // This assembly contains runtime C# code from your Assets folders:
        // (If you're using editor scripts, they may be in another assembly)
        //param.ReferencedAssemblies.Add("CSharp.dll");


        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0)
        {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors)
            {
                msg.AppendFormat("Error ({0}): {1}\n",
                    error.ErrorNumber, error.ErrorText);
            }
            throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }
}