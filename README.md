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
build.fsx deploys to ~/bin/Hamtor in Rasbperry pi, so you need to make directory first if it does not exists.

    $ cd Tool
    $ NuGet\FAKE.4.5.6\tools\FAKE.exe build.fsx Deploy

## How to install
After deployment, you need to setup auto startup script if you have not done yet.

Log in to Raspberry Pi, run following commands.

    $ cd ~/bin/Hamtor
    $ chmod 755 Install.sh
    $ sudo ./Install.sh
    
# License
Software is under the [MIT License](http://opensource.org/licenses/MIT)
