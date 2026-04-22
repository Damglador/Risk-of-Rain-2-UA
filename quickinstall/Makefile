ARCHIVE = RoR2UA.zip

BUILD_DIR = build
FILES = BepInEx/ doorstop_config.ini winhttp.dll .doorstop_version
DEPS = $(addprefix ${BUILD_DIR}/, ${FILES})

all: ${ARCHIVE}

${ARCHIVE}: ${DEPS}
	mkdir -p ${BUILD_DIR}

	[ -f "${ARCHIVE}" ] && mv "${ARCHIVE}" "${ARCHIVE}.bak" || true

	cd ${BUILD_DIR} && zip -r ../"${ARCHIVE}" ${FILES}

	[ -f "${ARCHIVE}.bak" ] && rm "${ARCHIVE}.bak" || true
