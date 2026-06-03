
EN_DIR := en
SRC_JSON_DIR := source/json
POT_DIR := source/pot
PO_DIR := uk-po
LANG_DIR := uk

DIRS = ${SRC_JSON_DIR} ${POT_DIR} ${PO_DIR} ${LANG_DIR}

JSON_FILES := $(patsubst ${EN_DIR}/%/, ${SRC_JSON_DIR}/%.json, $(wildcard ${EN_DIR}/*/))
POT_FILES := $(patsubst ${SRC_JSON_DIR}/%.json, ${POT_DIR}/%.pot, $(wildcard ${SRC_JSON_DIR}/*.json))
PO_FILES := $(patsubst ${POT_DIR}/%.pot, ${PO_DIR}/%.po, $(wildcard ${POT_DIR}/*.pot))
LANG_FILES := $(patsubst ${PO_DIR}/%.po, ${LANG_DIR}/%.language, $(wildcard ${PO_DIR}/*.po))

all: ${DIRS} ${JSON_FILES} ${POT_FILES} ${PO_FILES} ${LANG_FILES}

${DIRS}:
	mkdir -p $@

${SRC_JSON_DIR}/%.json: ${EN_DIR}/%/
	@echo "Building $@"
	./combine.sh $< $@

${POT_DIR}/%.pot: ${SRC_JSON_DIR}/%.json
	@echo "Building $@"
	json2po -P $< $@

${PO_DIR}/%.po: ${POT_DIR}/%.pot
	@echo "Building $@"
ifneq ("$(wildcard $("$@"))","")
	msgmerge -U "$@" "$<"
else
	msginit -i "$<" -o "$@" -l uk --no-translator
endif

${LANG_DIR}/%.language: ${PO_DIR}/%.po
	@echo "Building $@"
	po2json -t ${SRC_JSON_DIR} $< $@
	prettier --parser json --write $@
