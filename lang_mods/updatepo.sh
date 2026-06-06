#!/usr/bin/env bash

pot="$1"
po="$2"

if [ -f "$po" ]; then
  msgmerge -U "$po" "$pot"
else
  msginit -i "$pot" -o "$po" -l uk --no-translator
fi
  
