
QUICKINSTALL_ARCHIVE=dist/RoR2UA.zip
MOD_RELEASE_ARCHIVE=dist/RoR2UA-Thunderstore.zip

BUILD_DIR=.build

all: mod quickinstall

.PHONY: quickinstall mod
quickinstall: ${QUICKINSTALL_ARCHIVE}
mod: ${MOD_RELEASE_ARCHIVE}

${QUICKINSTALL_ARCHIVE}: quickinstall/RoR2UA.zip
	# Copy first dependency to target
	cp $< $@

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

${MOD_RELEASE_ARCHIVE}: ${PLUGIN} ${METADATA} ${LANG_DEPS} ${LANG_MODS_DEPS}
	rm -r ${BUILD_DIR}
	mkdir -p ${BUILD_DIR}/plugins/Language/uk

	cp ${PLUGIN}         ${BUILD_DIR}/plugins/
	@if [ -n "$(LANG_MODS_DEPS)" ]; then \
		cp ${LANG_MODS_DEPS} ${BUILD_DIR}/plugins/; \
	else \
		@echo No translations for mods found, skipping; \
	fi

	cp ${METADATA}  ${BUILD_DIR}/
	cp ${LANG_DEPS} ${BUILD_DIR}/plugins/Language/uk/

	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${BUILD_DIR} && zip -r ../"$@" .
	[ -f "$@.bak" ] && rm "$@.bak" || true

${PLUGIN}:
	$(MAKE) -C plugin build
