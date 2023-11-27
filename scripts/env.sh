#!/usr/bin/env bash

cd "$(dirname "$0")" || exit
cd .. || exit

alias dotnet "$(pwd)/dotnet/dotnet.exe"
export PATH=$PATH:"$(pwd)/dotnet"
