ARCHIVE = RoR2UA.zip

BUILD_DIR = build
FILES =                                             \
	BepInEx/*                                         \
	doorstop_config.ini                               \
	winhttp.dll                                       \
	.doorstop_version
BEPINEX = $(addprefix ${BUILD_DIR}/, ${FILES})
MOD = \
	${BUILD_DIR}/BepInEx/plugins/RoR2_UA-Risk_of_Rain_2_Ukrainian \
	${BUILD_DIR}/BepInEx/plugins/RoR2_UA-Risk_of_Rain_2_Ukrainian/Language/uk

all: ${ARCHIVE}

${BEPINEX}:
	./install-deps.sh

${ARCHIVE}: ${MOD} ${BEPINEX}
	mkdir -p ${BUILD_DIR}

	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${BUILD_DIR} && zip -r ../"$@" ${FILES}
	[ -f "$@.bak" ] && rm "$@.bak" || true
