namespace DevLogix.Services.Notifications;

/// <summary>
/// Demonstrates method hiding vs overriding (syllabus requirement).
/// 
/// When called through a base-type reference:
/// - Override: resolves to the derived class (polymorphic dispatch)
/// - New (hiding): resolves to the base class (compile-time binding)
/// </summary>
public class MethodHidingDemo
{
    public class BaseLogger
    {
        public virtual string GetLogPrefix() => "[BASE]";
        public string GetStaticPrefix() => "[BASE-STATIC]";
    }

    public class DerivedLoggerOverride : BaseLogger
    {
        // OVERRIDE — resolves dynamically at runtime
        public override string GetLogPrefix() => "[DERIVED-OVERRIDE]";
    }

    public class DerivedLoggerHiding : BaseLogger
    {
        // NEW (hiding) — resolves statically at compile time
        public new string GetStaticPrefix() => "[DERIVED-HIDING]";
    }

    /// <summary>
    /// Run this to see the difference:
    /// - baseRef pointing to DerivedLoggerOverride: GetLogPrefix() returns "[DERIVED-OVERRIDE]" (override wins)
    /// - baseRef pointing to DerivedLoggerHiding: GetStaticPrefix() returns "[BASE-STATIC]" (hiding: base resolves)
    /// - directRef to DerivedLoggerHiding: GetStaticPrefix() returns "[DERIVED-HIDING]" (direct call)
    /// </summary>
    public static (string overrideResult, string hidingViaBase, string hidingDirect) Demonstrate()
    {
        BaseLogger overrideRef = new DerivedLoggerOverride();
        BaseLogger hidingBaseRef = new DerivedLoggerHiding();
        var hidingDirectRef = new DerivedLoggerHiding();

        return (
            overrideResult: overrideRef.GetLogPrefix(),         // "[DERIVED-OVERRIDE]"
            hidingViaBase: hidingBaseRef.GetStaticPrefix(),     // "[BASE-STATIC]"
            hidingDirect: hidingDirectRef.GetStaticPrefix()     // "[DERIVED-HIDING]"
        );
    }
}
