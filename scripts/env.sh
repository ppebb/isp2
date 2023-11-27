cd "$(dirname "$0")" || exit
cd .. || exit

wdlin=$(pwd)
wdwin=$(cmd //c cd)

echo -e "setlocal

SET PATH=%PATH%;$wdwin\\dotnet

code

endlocal" >> scripts/run.bat

echo "alias dotnet='$wdlin/dotnet/dotnet.exe'" >> ~/.bashrc
