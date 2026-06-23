#!/usr/bin/env bash

target=$1

# Prettify
jq . "$target" > "$target".tmp && mv "$target".tmp "$target"

if grep -q "$(basename "$target")" LanguageAPIWhitelist.conf; then
  sed -i 's|"strings"|"uk"|' "$target"
fi
