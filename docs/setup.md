﻿# Setup
This document contains instructions for setting up the repository on any windows computer so you can write code for and test Blackguard.

## Installing Git
1. Download git for windows from [here](https://git-scm.com/download/win).
2. Find the installer in your Downloads folder and run it.
3. Make sure to select notepad as your default editor.

![gitscreenshot](https://github.com/ppebb/isp2/assets/36719558/45242652-99e1-488b-b989-7a5fa32b436c)

4. The remaining settings should all be default. Keep clicking next.

## Installing Visual Studio Code
1. Download Visual Studio Code from [here](https://code.visualstudio.com/).
2. Find the installer in your Downloads folder and run it.
3. Install the C# Dev Kit extension.

![c#](https://github.com/ppebb/isp2/assets/36719558/e3954c63-aabc-45f6-8e16-f207604dbb68)

## Cloning the repository
1. Make sure Firefox is your default browser, it breaks otherwise.
2. In VS Code, navigate to the explorer tab and click 'Clone Repository' and then 'Clone From Github'.
3. VS Code will ask you to sign in to Github and authorize VS Code.
4. Select isp2 from the list of available repositories and clone it.
5. Authorize git-ecosystem to clone it.
6. Select to open the project, and select that you trust the author.
7. VS Code will complain about not having the .NET SDK, close the popup if you are on a computer where you do not have administrator access (such as a school computer), otherwise select yes.

## Installing .NET SDK without administrator access
1. Go to the .NET download page [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Select Windows x64 under binaries.
3. Go to downloads and extract the files by right clicking, and selecting extract all.
4. Move the resulting folder to the repository, and rename it to dotnet.
5. Run scripts/env.sh to set your path, restart git bash, and now you can run dotnet.

## Launching Visual Studio Code without administrator access
1. Navigate to your repository in git bash.
2. Run scripts/run.bat to launch Visual Studio Code.
