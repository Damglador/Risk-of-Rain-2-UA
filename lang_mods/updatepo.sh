#!/usr/bin/env bash

pot="$1"
po="$2"

if [ -f "$po" ]; then
  msgmerge -U "$po" "$pot" --backup none
  touch "$po"
else
  msginit -i "$pot" -o "$po" -l uk --no-translator
fi
