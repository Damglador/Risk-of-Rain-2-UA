#!/usr/bin/env bash

# Зклеює купу json файлів з наданої директорії в один
# Люта милиця, треба перевіряти вихідний файл на помилки
# Файли з blacklist не будуть враховані

input_folder="$1"
output_file="${2:-"output-ukrainian.json"}"
blacklist=(
  "$output_file"
  language.json
  cu8.json
  DLC3.json
)

echo "{
  \"strings\": {" > "$output_file"

for f in "$input_folder"/*.json; do
  for b in "${blacklist[@]}"; do
    [ "$(basename "$f")" == "$b" ] && continue 2
  done
  grep -oP '"(\\.|[^"\\])*"\s*:\s*"(\\.|[^"\\])*"' "$f" | sed 's/^/    /' >> "$output_file"
done

# Add commas after every key-value
sed -i 's/"*"*"*"$/&,/g' "$output_file"
# Remove the last comma
sed -i '$ s/,$//' "$output_file"

echo "  }
}" >> "$output_file"
