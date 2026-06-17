#!/usr/bin/env bash
#shellcheck disable=SC2001

set -eu

INSTALL_DIR="$(realpath "$(dirname "$0")")/build"
CACHE=${TMPDIR:-/tmp}/bepinex-downloads
PACKAGE_QUE=()
mkdir -p "$CACHE" "$INSTALL_DIR"

# Get package path for api (like https://thunderstore.io/api/experimental/package/RoR2_UA/Risk_of_Rain_2_Ukrainian/)
# from ID specified in dependencies (like RoR2_UA-Risk_of_Rain_2_Ukrainian-1.4.11)
package_path(){
  ID="$1"
  while IFS='-' read -r author name _; do
    echo "package/$author/$name"
  done <<< "$ID"
}

baseid(){
  ID="$1"
  sed 's/-[0-9].*//' <<< "$ID"
}

request(){
  URL="$1" # like "https://thunderstore.io/api/experimental/package/RoR2_UA/Risk_of_Rain_2_Ukrainian/"
  curl -s -X GET \
    -H "accept: application/json" \
    "$URL"
}

package_deps(){
  ID="$1"
  request "https://thunderstore.io/api/experimental/$(package_path "$ID")/" | jq -r .latest.dependencies[]
}

# https://stackoverflow.com/a/8574392
is_qued () {
  for i in "${PACKAGE_QUE[@]}"
  do
    if [ "$i" == "$1" ] ; then
      return 0
    fi
  done
  return 1
}

download(){
  ID=$1
  echo "Downloading $ID"
  URL=$(request "https://thunderstore.io/api/experimental/$(package_path "$ID")/" | jq -r .latest.download_url)
  curl -sL "$URL" -o "$CACHE/$(baseid "$ID")".zip
  [ -d "$CACHE/$(baseid "$ID")" ] && rm -r "${CACHE:?}/$(baseid "$ID")"
  unzip -q "$CACHE/$(baseid "$ID")".zip -d "$CACHE/$(baseid "$ID")"
}

structure(){
  ID=$1

  if [ -d "$CACHE/$(baseid "$ID")/plugins" ]; then
    mkdir -p "$CACHE/$(baseid "$ID")/BepInEx/"
    mv "$CACHE/$(baseid "$ID")/plugins/" "$CACHE/$(baseid "$ID")/BepInEx/plugins/"
  else
    mkdir -p "$CACHE/$(baseid "$ID")/BepInEx/plugins/"
  fi
  find "$CACHE/$(baseid "$ID")" -maxdepth 1 -type f -exec mv {} "$CACHE/$(baseid "$ID")/BepInEx/plugins/" \;
}

install_mod(){
  source="$1" # Has to contain BepInEx directory. Directory name will be taken as mod ID
  dest="$2" # Has to contain BepInEx directory.
  ID="$(basename "$source")"

  cd "$source" # to make find relative
  find . -type f | while read -r file; do
    install -D "$source/$file" \
      "$dest/$(sed "s|/patchers/|/patchers/$ID/|" <<< "$(sed "s|/plugins/|/plugins/$ID/|" <<< "$file")")"
  done
}

check_deps(){
  PACKAGE=$1
  echo "Checking deps for $PACKAGE"
  for package in $(package_deps "$PACKAGE"); do
    if ! is_qued "$(baseid "$package")"; then
      PACKAGE_QUE+=("$(baseid "$package")")
      check_deps "$package"
    fi
  done
}

check_deps RoR2_UA-Risk_of_Rain_2_Ukrainian
echo "Going to install: [${PACKAGE_QUE[*]}]"
for package in "${PACKAGE_QUE[@]}"; do
  download "$package"
  if [ "$package" == bbepis-BepInExPack ]; then
    echo "Installing BepInEx"
    cp -r "$CACHE/$(baseid "$package")/BepInExPack/." "$INSTALL_DIR"
    continue
  fi
  structure "$package"
  install_mod "$CACHE/$(baseid "$ID")" "$INSTALL_DIR"
done

# Add config to disable console
mkdir -p "$INSTALL_DIR/BepInEx/config/"
cat > "$INSTALL_DIR/BepInEx/config/BepInEx.GUI.cfg" << EOF
[Settings]

## Enable the custom BepInEx GUI
# Setting type: Boolean
# Default value: true
Enable BepInEx GUI = false

## Close the graphic user interface window when the game is loaded
# Setting type: Boolean
# Default value: false
Close Window When Game Loaded = false

## Close the graphic user interface window when the game closes
# Setting type: Boolean
# Default value: true
Close Window When Game Closes = true
EOF
