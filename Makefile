
QUICKINSTALL_ARCHIVE=dist/RoR2UA.zip
MOD_RELEASE_ARCHIVE=dist/RoR2UA-Thunderstore.zip

BUILD_DIR=.build

LANG_FILES =              \
	credits_roles.json      \
	DLC3.json icon.png      \
	language.json           \
	output-ukrainian.json
LANG_DEPS = $(addprefix lang/uk/, ${LANG_FILES})
LANG_MODS_DEPS = $(wildcard lang_mods/uk/*.language)
PLUGIN = plugin/bin/Release/Risk_of_Rain_2_Ukrainian.dll
METADATA =        \
	manifest.json   \
	README.md       \
	CHANGELOG.md    \
	assets/icon.png
MOD_DIR = quickinstall/build/BepInEx/plugins/RoR2_UA-Risk_of_Rain_2_Ukrainian/

all: prepare mod quickinstall

.PHONY: prepare quickinstall mod
quickinstall: ${QUICKINSTALL_ARCHIVE}
mod: ${MOD_RELEASE_ARCHIVE}

${QUICKINSTALL_ARCHIVE}: quickinstall/RoR2UA.zip
	# Copy first dependency to target
	rsync $< $@

prepare:
	$(MAKE) -C plugin

quickinstall/RoR2UA.zip: ${METADATA} ${MOD_RELEASE_ARCHIVE}
	mkdir -p "${MOD_DIR}"/Language/uk
	rsync -r --force --delete ${METADATA}             ${MOD_DIR}
	rsync -r --force --delete ${BUILD_DIR}/plugins/*  ${MOD_DIR}
	$(MAKE) -C quickinstall

${MOD_RELEASE_ARCHIVE}: ${PLUGIN} ${METADATA} ${LANG_DEPS} ${LANG_MODS_DEPS}
	rm -r ${BUILD_DIR} || true
	mkdir -p ${BUILD_DIR}/plugins/Language/uk

	rsync ${PLUGIN}         ${BUILD_DIR}/plugins/
	@if [ -n "$(LANG_MODS_DEPS)" ]; then \
		rsync ${LANG_MODS_DEPS} ${BUILD_DIR}/plugins/; \
	else \
		@echo No translations for mods found, skipping; \
	fi

	rsync ${METADATA}  ${BUILD_DIR}/
	rsync ${LANG_DEPS} ${BUILD_DIR}/plugins/Language/uk/

	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${BUILD_DIR} && zip -r ../"$@" .
	[ -f "$@.bak" ] && rm "$@.bak" || true
