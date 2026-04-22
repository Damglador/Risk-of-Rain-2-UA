#!/usr/bin/env bash

set -e
cd "$(dirname "$0")"

archive_name="Package.zip"

[ -f "$archive_name" ] && mv "$archive_name" "$archive_name.bak"

zip -r "$archive_name" plugins CHANGELOG.md icon.png manifest.json README.md

[ -f "$archive_name.bak" ] && rm "$archive_name.bak"
