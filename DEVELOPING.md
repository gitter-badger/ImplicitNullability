# Design decisions

## Implicit nullability of delegates and methods in Razor/ASP.NET files (FS, UB on 02.07.2014)

The methods arguments *are* implicitly nullable. -> Principle: "Wherever a CanBeNull/NotNull annotation can be added by the developer, implicit nullability is applicable".

## Disadvantages

* Doesn't match Fody NullGuard plugin. But: Static analysis needn't completely match runtime null checks added by weaver; the main purpose of the rewriter is a good API exception contract paired with fail fast. Developer must know details of NullGuard anyway, and it's clear that NullGuard can go only so far (with realistic effort).
* Delegates: doesn't match anonymous methods/lambdas. But: Delegates are callable, and it makes sense to make use of static analysis at least at the call sites. Additionally, with lambdas/anonymous methods, C# doesn't allow placing CanBeNull/NotNull attributes on the parameters, but it does allow it on delegate declarations.

## Advantages

* Symmetry with "regular" methods parameters
* Simple rule, easy to understand
* For input parameters it makes the calls *safer*, not more unsafe (i.e., safer default than "Unknown" nullability)

Note that ReSharper does not check nullability constraints when binding methods to delegates. For example, the following code does not produce a warning.

    public delegate void SomeDelegate ([CanBeNull] string s);

    private static void M (/*[NotNull]*/ string s) // Explicit or implicit NotNull doesn't make a difference
    {
       Console.WriteLine (s.Length); // NullReferenceException when called with s == null
    }

    SomeDelegate d = M;
    d(null); // Problem: Static analysis allows calling d with null, although the method M does not

Therefore, the user is responsible for checking nullability constraints when creating delegates bound to methods.


# ImplicitNullability.Plugin.Tests integration tests

The ReSharper integration tests don't use the "gold file approach" of the ReSharper SDK, but use annotations directly in the source files for the expectations (e.g. `str == null /*Expect:ConditionIsAlwaysTrueOrFalse*/`), which are matched with the actual inspection warnings when running the analysis during test execution.

## CSS directory

The CSS directory is a copy of R# 8.2's bin directory, because it is necessary for aspx/cshtml tests and 
it's not included in the SDK package (see http://youtrack.jetbrains.com/issue/RSRP-416928).

Starting with the R# 9.0 SDK this could be removed again.

## ExternalAnnotations directory

Includes specific external annotations, e.g. for TemplateControl.Eval(), because the external annotation files 
aren't included in the SDK package.

# ReSharper 9.0 local development

Install ReSharper into an `Exp` VisualStudio instance and copy the following property into the `.csproj.user`.

    <PropertyGroup>
      <HostFullIdentifier>ReSharperPlatformVs12Exp</HostFullIdentifier>
    </PropertyGroup>