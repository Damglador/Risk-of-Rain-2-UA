#!/usr/bin/env bash

# Зклеює всі текстові файлів з наданої директорії в один
# Файли з blacklist не будуть враховані

input_folder="$1"
output_file="${2:-"output-ukrainian.json"}"
blacklist=("$output_file")

echo "{
	\"uk\": {" > "$output_file"

for f in "$input_folder"/*; do
  if ! file "$f" | grep -q "text"; then
    continue
  fi
  for b in "${blacklist[@]}"; do
      [ "$(basename "$f")" == "$b" ] && continue 2
  done
  grep -oP '"(\\.|[^"\\])*"\s*:\s*"(\\.|[^"\\])*"' "$f" | sed 's/^/		/' >> "$output_file"
done

sed -i 's/"*"*"*"$/&,/g' "$output_file"

echo "  }
}" >> "$output_file"
