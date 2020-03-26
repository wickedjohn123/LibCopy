using LibCopy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Reflection;

namespace LibraryTests
{
    [TestClass]
    public class MethodRefTests
    {
        [TestMethod]
        public void EnvironmentExitTests()
        {
            EnsureMethodIsNotBeingCalled(typeof(Environment).GetMethods().Where(x => x.Name == "Exit").ToArray());
        }


        private void EnsureMethodIsNotBeingCalled(MethodInfo[] methods)
        {
            var cnt = GetMethodsInfoCallCountInAssembly(typeof(Utils).Assembly, methods);
            Assert.IsTrue(cnt == 0, $"Detected {cnt} call(s) to {methods[0].DeclaringType}.{methods[0].Name} which is not allowed!");
        }

        private int GetMethodsInfoCallCountInAssembly(Assembly assemblyToSearch, MethodInfo[] methodsCallsToFind)
        {
            int cnt = 0;

            for (int i = 0; i < methodsCallsToFind.Length; i++)
            {
                var mi = methodsCallsToFind[i];

                // TODO decrease the assembly read calls
                var types = AssemblyDefinition.ReadAssembly(mi.DeclaringType.Assembly.Location).Modules.SelectMany(m => m.Types).Where(t => t.FullName.Equals(mi.DeclaringType.FullName)).ToList();
                if (types.Count() > 1)
                {
                    throw new AmbiguousMatchException("More than one type matched!");
                }
                else if (types.Count() == 0)
                {
                    throw new MissingMethodException("Unable to find matching type and thus no matching member can be found!");
                }

                var type = types.First();

                var methods = type.Methods.Where(m => m.Name.Equals(mi.Name) && m.Parameters.Count == mi.GetParameters().Count()).ToList();
                if (methods.Count() > 1)
                {
                    throw new AmbiguousMatchException("More than one method matched!");
                }
                else if (methods.Count() == 0)
                {
                    throw new MissingMethodException("Unable to find a matching method!");
                }

                var md = methods.First();

                cnt += GetMethodReferenceCountInAssembly(assemblyToSearch, md);
            }

            return cnt;
        }

        private int GetMethodReferenceCountInAssembly(Assembly assemblyToSearch, MethodReference methodCallsToFind)
        {
            int cnt = 0;

            var types = AssemblyDefinition.ReadAssembly(assemblyToSearch.Location).Modules.SelectMany(m => m.GetTypes()).ToList();
            bool foundAny = true;
            while (foundAny)
            {
                foundAny = false;
                for (int i = 0; i < types.Count(); i++)
                {
                    var t = types[i];
                    var nested = t.NestedTypes;
                    if (nested.Any())
                    {
                        foreach (var n in nested)
                        {
                            if (!types.Contains(n))
                            {
                                foundAny = true;
                                types.AddRange(nested);
                            }
                        }
                    }
                }
            }


            var methodDefinitions = types.SelectMany(t => t.Methods).ToList();

            foreach (var md in methodDefinitions)
            {
                var ins = md.Resolve().Body.Instructions.Cast<Instruction>();
                var c = ins.Count(i => (i.OpCode == OpCodes.Callvirt || i.OpCode == OpCodes.Calli || i.OpCode == OpCodes.Call)
                                                    && (i.Operand as MethodReference).FullName == methodCallsToFind.FullName);
                cnt += c;
            }

            return cnt;
        }
    }
}
