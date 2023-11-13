#!/usr/bin/env sh

cd "$(dirname "$0")" || exit

if [ -z "$1" ]; then
    echo "No argument supplied for commit message"
    exit
fi

# stages all files, shows changes, and then commits
git add -A
git diff --cached
git commit -m "$1"
