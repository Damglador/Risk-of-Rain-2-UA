#!/usr/bin/env bash

# Зклеює купу json файлів з наданої директорії в один
# Люта милиця, треба перевіряти вихідний файл на помилки

input_folder="$1"
output_file="output-ukrainian.json"

echo "{
	\"strings\":
	{" > "$output_file"

for f in "$input_folder"/*.json; do
    [ "$(basename "$f")" = "$output_file" ] && continue
    [ "$(basename "$f")" = "language.json" ] && continue
    grep "		.*" "$f" >> "$output_file"
done

sed -i 's/"*"*"*"$/&,/g' "$output_file"

echo "	}
}" >> "$output_file"