// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Naming",
        "CA1703:ResourceStrings" + "ShouldBeSpelledCorrectly",
        MessageId = "nwill",
        Scope = "resource",
        Target = "Tekapo.Properties.Resources.resources")]
[assembly:
    SuppressMessage("Microsoft.Usage",
        "CA1806:DoNotIgnoreMethodResults",
        MessageId = "System.Text.RegularExpressions.Regex",
        Scope = "member",
        Target = "Tekapo.Controls.SelectPathPage.#IsPageValid()")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Tekapo.Controls.SelectPathPage.#InitializeComponent()")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Tekapo.Controls.TimeShiftPage.#InitializeComponent()")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Tekapo.Controls.NameFormatPage.#InitializeComponent()")]

