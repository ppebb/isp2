cd "$(dirname "$0")" || exit
cd .. || exit

if [ -e "./scripts/run.bat" ]; then
    printf "run.bat already exists, and will not be created again. Exiting..."
    exit 1
fi

wdlin=$(pwd)
wdwin=$(cmd //c cd)

echo -e "setlocal

SET PATH=%PATH%;$wdwin\\dotnet

code

endlocal" >> scripts/run.bat

echo "alias dotnet='$wdlin/dotnet/dotnet.exe'" >> ~/.bashrc
