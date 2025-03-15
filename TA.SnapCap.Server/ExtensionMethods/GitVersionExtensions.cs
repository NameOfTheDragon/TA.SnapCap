// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Linq;
using System.Reflection;

namespace TA.SnapCap.Server.ExtensionMethods
    {
    public static class GitVersionExtensions
        {
        public static string GitInformationalVersion => GitVersion().GitVersionField("InformationalVersion");

        public static string GitCommitSha => GitVersion().GitVersionField("Sha");

        public static string GitCommitShortSha => GitVersion().GitVersionField("ShortSha");

        public static string GitCommitDate => GitVersion().GitVersionField("CommitDate");

        public static string GitSemVer => GitVersion().GitVersionField("SemVer");

        public static string GitFullSemVer => GitVersion().GitVersionField("FullSemVer");

        public static string GitBuildMetadata => GitVersion().GitVersionField("FullBuildMetaData");

        public static string GitMajorVersion => GitVersion().GitVersionField("Major");

        public static string GitMinorVersion => GitVersion().GitVersionField("Minor");

        public static string GitPatchVersion => GitVersion().GitVersionField("Patch");

        private static string GitVersionField(this Type gitVersionInformationType, string fieldName)
            {
            var versionField = gitVersionInformationType?.GetField(fieldName);
            return versionField?.GetValue(null).ToString() ?? "undefined";
            }

        private static Type GitVersion()
            {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().SingleOrDefault(t => t.Name == "GitVersionInformation");
            return type;
            }
        }
    }