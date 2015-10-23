# autonek-hamtor
autonek-hamtor is a human motion detector in the hallway.

# For Development
## Requirements
### NuGet.CommandLine
* [NuGet.CommandLine](https://chocolatey.org/packages/NuGet.CommandLine)

NuGet.CommandLine can be installed by chocolatey.

    $ choco install nuget.commandline
    
### scp

This is required when deploy to Raspberry Pi.

[Windows]<br/>
[GoW](https://github.com/bmatzelle/gow/wiki) contains scp.

    $ choco install gow

## How to restore nuget packages in Tool directory

    $ cd Tool
    $ nuget restore packages.config -PackagesDirectory NuGet

## How to deploy to Raspberry Pi

    $ cd Tool
    $ NuGet\FAKE.4.5.6\tools\FAKE.exe build.fsx Deploy

# License
Software is under the [MIT License](http://opensource.org/licenses/MIT)
