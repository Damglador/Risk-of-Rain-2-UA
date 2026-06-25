#!/usr/bin/env bash
#shellcheck disable=SC2001

set -eu

INSTALL_DIR="$(realpath "$(dirname "$0")")/quickinstall"
echo Installing to $INSTALL_DIR
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
  curl --retry 3 -sL "$URL" -o "$CACHE/$(baseid "$ID")".zip
  [ -d "$CACHE/$(baseid "$ID")" ] && rm -r "${CACHE:?}/$(baseid "$ID")"
  unzip -q "$CACHE/$(baseid "$ID")".zip -d "$CACHE/$(baseid "$ID")"
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
    rsync -r "$CACHE/$(baseid "$package")/BepInExPack/." "$INSTALL_DIR"
    continue
  fi
  ./install-mod.sh --source="$CACHE/$(baseid "$ID")" --dest="$INSTALL_DIR/BepInEx" --id="$package"
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
