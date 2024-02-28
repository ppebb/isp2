cd "$(dirname "$0")" || exit
cd .. || exit

if [ -e "./scripts/run.bat" ] || [ -e "./scripts.build.bat" ]; then
    printf "run.bat or build.bat already exists, and will not be created again. Exiting..."
    exit 1
fi

wdlin=$(pwd)
wdwin=$(cmd //c cd)

echo -e "setlocal

SET PATH=%PATH%;$wdwin\\dotnet

code

endlocal" >> scripts/run.bat

echo -e "setlocal

SET PATH=%PATH%;$wdwin\\dotnet

dotnet.exe build

endlocal" >> scripts/build.bat

echo "alias dotnet='$wdlin/dotnet/dotnet.exe'" >> ~/.bashrc
