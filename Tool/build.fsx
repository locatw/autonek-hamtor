#I @"NuGet/FAKE.4.5.6/tools/"
#r @"NuGet/FAKE.4.5.6/tools/FakeLib.dll"

open Fake
open Fake.EnvironmentHelper
open Fake.FileHelper
open Fake.FileSystem

let toolPath =
    if isLinux then
        "scp"
    // Windows
    else
        normalizePath (ProgramFilesX86 @@ @"Gow\bin\scp.bat")

let rootDir = ".."
let hamtorProjectDir = rootDir @@ "Hamtor/Hamtor/"
let binDir = hamtorProjectDir @@ "bin/"
let releaseBinDir = binDir @@ "Release/"

// Don't normalize. Raspberry Pi is linux, so directory separator is '/'.
// And cannot use '~' in Windows, so needs absolute path.
let deployPath = @"pi@192.168.0.7:/home/pi/bin/Hamtor/"

let packDir = hamtorProjectDir @@ "pack/"
let releasePackDir = normalizePath (packDir @@ "Release/")

let deploy source target =
    let args = sprintf "-r %s %s" source target
    let result = 
        ExecProcess (fun info -> 
            info.FileName <- toolPath
            info.WorkingDirectory <- "."
            info.Arguments <- args) System.TimeSpan.MaxValue
    if result <> 0 then
        failwithf "failed to execute scp. Source: %s Target: %s" source target

Target "Pack" (fun _ ->
    if not (TestDir releasePackDir) then
        CreateDir releasePackDir

    // delete unnecessary files and directories.
    CleanDir releasePackDir

    !! (releaseBinDir @@ "*.exe")
    ++ (releaseBinDir @@ "*.dll")
    ++ (rootDir @@ "Hamtor.sh")
    ++ (rootDir @@ "Install.sh")
    -- (releaseBinDir @@ "FSharp.Core.dll")
    |> Copy (normalizePath releasePackDir)
)

Target "Deploy" (fun _ ->
    deploy releasePackDir deployPath
)

"Pack" ==> "Deploy"

RunTargetOrDefault "Deploy"