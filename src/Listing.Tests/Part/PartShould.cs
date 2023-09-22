using System.Text;
using Listing.Code;
using Listing.Contents;

namespace Listing.Tests.Part;

public sealed class PartShould
{
    private static TestCaseData Case(string name, string code, string path) =>
        new TestCaseData(code, path).SetName(name);

    private static IEnumerable<TestCaseData> Cases()
    {
        yield return Case("Class_In_Global_Namespace", "class A {}", "A");
        yield return Case("Record_In_Global_Namespace", "record A {}", "A");
        yield return Case("Record_Class_In_Global_Namespace", "record class A {}", "A");
        yield return Case("Struct_In_Global_Namespace", "struct A {}", "A");
        yield return Case("Record_Struct_In_Global_Namespace", "record struct A {}", "A");
        yield return Case("Record_With_Primary_Ctor", "record A(int B) {}", "A");
        yield return Case("Nested_Class_In_Global_Namespace", "class A { class B {} }", "A.B");
        yield return Case("Nested_Record_In_Global_Namespace", "class A { record B {} }", "A.B");
        yield return Case("Class_In_Regular_Namespace", "namespace Root { class A {} }", "Root.A");
        yield return Case("Nested_Class_In_Regular_Namespace", "namespace Root { class A { class B {} } }", "Root.A.B");
        yield return Case("Class_With_Generic_Parameter", "class A<T> {}", "A");
        yield return Case("Class_With_Generic_Parameters", "class A<T1, T2, T3, T4> {}", "A");
        yield return Case("Nested_Class_With_Generic_Parameter", "class A { class B<TArg> {} }", "A.B");
        yield return Case("Nested_Class_When_Both_Has_Generic_Parameter", "class A<TA> { class B<TB> {} }", "A.B");
        yield return Case("Class_With_Base", "class A {} class B : A {}", "B");
        yield return Case("Constrained_Generic", "class A<T> where T : class {}", "A");
        yield return Case("Complex_Nesting", "class A<TA> { record B<TB> { struct C<TC> { readonly record struct D<TD> {} } } }", "A.B.C.D");
    }

    [Test]
    [TestCaseSource(nameof(Cases))]
    public Task Print(string source, string path)
    {
        var compilation = SimpleCompilation.Create(source);
        return MakePart(compilation, path);
    }

    private static Task MakePart(SimpleCompilation compilation, string path)
    {
        var targetSymbol = compilation.Locate(path);
        var output = new Output(new StringBuilder());
        var part = new Code.Part(targetSymbol);
        using (part.Open(output))
        {
            output.WriteLine("// TYPE HERE".AsContent());
        }

        return Verify.Output(output);
    }
}