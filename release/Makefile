DIST_DIR = ../dist

QUICKINSTALL_ARCHIVE := ${DIST_DIR}/RoR2UA.zip
THUNDERSTORE_ARCHIVE := ${DIST_DIR}/RoR2UA-Thunderstore.zip

all: ${THUNDERSTORE_ARCHIVE} ${QUICKINSTALL_ARCHIVE}


# ============ Shared Variables ===================
LANG_FILES =              \
	credits_roles.json      \
	DLC3.json icon.png      \
	language.json           \
	output-ukrainian.json
LANG_DIR = ../lang/uk/
LANG_DEPS = $(addprefix ${LANG_DIR}, ${LANG_FILES})
LANG_MODS_DIR = ../lang_mods/uk/
LANG_MODS_CONFIG = ${LANG_MODS_DIR}/../LanguageAPIWhitelist.conf
LANG_MODS_DEPS = $(wildcard ${LANG_MODS_DIR}/*.json) ${LANG_MODS_CONFIG}
PLUGIN = ../plugin/bin/Release/Risk_of_Rain_2_Ukrainian.dll
METADATA =           \
	../manifest.json   \
	../README.md       \
	../CHANGELOG.md    \
	../assets/icon.png
MOD_DEPS := ${PLUGIN} ${LANG_DEPS} ${LANG_MODS_DEPS} ${METADATA}


# ============ Thunderstore Release ===================
T_DIR = thunderstore
${THUNDERSTORE_ARCHIVE}: ${MOD_DEPS}
	rm -r ${T_DIR} || true
	mkdir -p ${T_DIR}/plugins/lang/uk/
	mkdir -p ${T_DIR}/plugins/lang_mods/uk/

	rsync ${METADATA}         ${T_DIR}/
	rsync ${PLUGIN}           ${T_DIR}/plugins/
	rsync ${LANG_DEPS}        ${T_DIR}/plugins/lang/uk/
	rsync ${LANG_MODS_CONFIG} ${T_DIR}/plugins/lang_mods/
	@if [ -n "$(LANG_MODS_DEPS)" ]; then \
	  rsync ${LANG_MODS_DEPS} ${T_DIR}/plugins/lang_mods/uk/ --exclude-from=${LANG_MODS_DIR}/../wip-mods.txt; \
	else \
		echo No translations for mods found, skipping; \
	fi

	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${T_DIR} && zip -r ../"$@" .
	[ -f "$@.bak" ] && rm "$@.bak" || true


# ============ Quickinstall Release ===================
Q_DIR = quickinstall
Q_MOD_DIR = ${Q_DIR}/BepInEx/plugins/RoR2_UA-Risk_of_Rain_2_Ukrainian
${Q_MOD_DIR}: ${THUNDERSTORE_ARCHIVE} ${MOD_DEPS}
	rm -r ${Q_MOD_DIR} || true
	./install-mod.sh --source="${T_DIR}" --dest="${Q_DIR}/BepInEx" --id="RoR2_UA-Risk_of_Rain_2_Ukrainian"

Q_BEPINEX :=                            \
	${Q_DIR}/BepInEx/config/              \
	${Q_DIR}/BepInEx/core/                \
	${Q_DIR}/BepInEx/patchers/            \
	${Q_DIR}/BepInEx/plugins/             \
	${Q_DIR}/doorstop_config.ini          \
	${Q_DIR}/winhttp.dll                  \
	${Q_DIR}/.doorstop_version

${Q_BEPINEX}:
	./install-deps.sh

${QUICKINSTALL_ARCHIVE}: ${Q_BEPINEX} ${Q_MOD_DIR} $(shell find ${Q_MOD_DIR})
	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${Q_DIR} && zip -r ../"$@" .
	[ -f "$@.bak" ] && rm "$@.bak" || true


clean:
	rm -r ${QUICKINSTALL_ARCHIVE} ${THUNDERSTORE_ARCHIVE} ${Q_DIR} ${T_DIR} || true
