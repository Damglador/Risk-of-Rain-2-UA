ARCHIVE = RoR2UA.zip

BUILD_DIR = build
FILES =                                             \
	BepInEx/*                                         \
	doorstop_config.ini                               \
	winhttp.dll                                       \
	.doorstop_version
DEPS = $(addprefix ${BUILD_DIR}/, ${FILES})

all: ${ARCHIVE}

${ARCHIVE}: ${DEPS}
	mkdir -p ${BUILD_DIR}

	[ -f "$@" ] && mv "$@" "$@.bak" || true
	cd ${BUILD_DIR} && zip -r ../"$@" ${FILES}
	[ -f "$@.bak" ] && rm "$@.bak" || true
