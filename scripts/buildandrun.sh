#!/usr/bin/env bash

cd "$(dirname "$0")" || exit
cd .. || exit

dotnet build
bin/Debug/net7.0/Blackguard
