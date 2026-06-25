#!/usr/bin/env bash

for i in "$@"; do
  case $i in
    --source=*)
      SOURCE="${i#*=}"
      shift # past argument=value
      ;;
    --dest=*)
      DEST="${i#*=}"
      shift # past argument=value
      ;;
    --id=*)
      MOD_ID="${i#*=}"
      shift # past argument=value
      ;;
    *)
      ;;
  esac
done

if [ ! -f "$SOURCE/manifest.json" ]; then
  echo "Source $SOURCE isn't a mod directory"
  exit 1
fi

MOD_ID="${MOD_ID:-$(jq -r .name "$SOURCE/manifest.json")}"

if [ ! "$(basename "${DEST}")" = "BepInEx" ]; then
  echo "Destination isn't a BepInEx directory"
  exit 1
fi

find "$SOURCE" -maxdepth 1 -type f | while read -r file; do
  mkdir -p "$DEST/plugins/$MOD_ID"
  rsync "$file" "$DEST/plugins/$MOD_ID"
done

subdirs=(
  BepInEx/config/
  BepInEx/core/
  BepInEx/patchers/
  BepInEx/plugins/
  config/
  core/
  patchers/
  plugins/
)

for subdir in "${subdirs[@]}"; do
  if [ -d "$SOURCE/$subdir" ]; then
    if [ "$(basename "$subdir")" = "patchers" ] || [ "$(basename "$subdir")" = "plugins" ]; then
      mkdir -p "$DEST/$(basename "$subdir")/$MOD_ID"
      rsync -r "$SOURCE/$subdir/"* "$DEST/$(basename "$subdir")/$MOD_ID"
    else
      mkdir -p "$DEST/$(basename "$subdir")"
      rsync -r "$SOURCE/$subdir/"* "$DEST/$(basename "$subdir")"
    fi
  fi
done
