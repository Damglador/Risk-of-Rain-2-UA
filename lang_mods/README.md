# Instructions
Just run `make` and translate files in `uk-po/`, then run `make` again.

Dependencies for the Makefile:
- jq
- prettier
- json2po and po2json (translate-toolkit)
- msgmerge and msginit (gettext)

# Structure
```
.
├── combine.sh
├── en
│   └── <MOD_ID> - Directory with translation files for a mod
│       └── <file> - Translation files for the mod.
│                    Can have any file extension, but the content should be resembling json.
├── source
│   ├── json - [DO NOT TOUCH] Valid .json files generated from whatever is provided in en/
│   └── pot - [DO NOT TOUCH] Template files generated from json files
├── uk - [DO NOT TOUCH] Output .language files for the mod
└── uk-po - .po files to translate
```


PS: This is not AI generated, I just used `tree`
