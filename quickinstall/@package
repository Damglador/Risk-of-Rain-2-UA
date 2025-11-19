#!/usr/bin/env bash

set -e
cd "$(dirname "$0")"

archive_name="RoR2UA.zip"

[ -f "$archive_name" ] && mv "$archive_name" "$archive_name.bak"

zip -r "$archive_name" BepInEx doorstop_config.ini winhttp.dll .doorstop_version

[ -f "$archive_name.bak" ] && rm "$archive_name.bak"
