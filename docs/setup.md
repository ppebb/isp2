# Setup
This document contains instructions for setting up the repository on any windows computer so you can write code for and test Blackguard.

## Installing Git
1. Download git for windows from [here](https://git-scm.com/download/win)
2. Find the installer in your Downloads folder and run it
3. Make sure to select notepad as your default editor.

![gitscreenshot](https://github.com/ppebb/isp2/assets/36719558/45242652-99e1-488b-b989-7a5fa32b436c)

4. The remaining settings should all be default. Keep clicking next.

## Installing Visual Studio Code
1. Download Visual Studio Code from [here](https://code.visualstudio.com/)
2. Find the installer in your Downloads folder and run it.
3. Install the C# Dev Kit extension

![c#](https://github.com/ppebb/isp2/assets/36719558/e3954c63-aabc-45f6-8e16-f207604dbb68)

## Cloning the repository
1. In VS Code, navigate to the explorer tab and click 'Clone Repository' and then 'Clone From Github'
2. VS Code will ask you to sign in to Github and authorize VS Code.
3. Select isp2 from the list of available repositories and clone it.
4. Authorize git-ecosystem to clone it.
5. Select to open the project, and select that you trust the author.
6. VS Code will complain about not having the .NET SDK, select no if you are on a computer where you do not have administrator access (such as a school computer), otherwise select yes.

## Installing .NET SDK without administrator access
1. Go to the .NET download page [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Select Windows x64
3. Go to downloads and extract the files.
4. Move the resulting folder to the repository, and rename it to dotnet
5. Run scripts/env.sh to set your path, and now you can run dotnet
