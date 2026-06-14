
EN_DIR := en
JSON_DIR := source/json
POT_DIR := source/pot
PO_DIR := uk-po
LANG_DIR := uk

DIRS = ${SRC_JSON_DIR} ${POT_DIR} ${PO_DIR} ${LANG_DIR}

# If there's a directory <mod> in en/, it adds targets:
# - source/json/<mod>.json
# - source/pot/<mod>.pot
# - uk-po/<mod>.po
# - uk/<mod>.language
TARGETS := $(patsubst ${EN_DIR}/%/, ${JSON_DIR}/%.json, $(wildcard ${EN_DIR}/*/)) \
				   $(patsubst ${EN_DIR}/%/, ${POT_DIR}/%.pot, $(wildcard ${EN_DIR}/*/)) \
					 $(patsubst ${EN_DIR}/%/, ${PO_DIR}/%.po, $(wildcard ${EN_DIR}/*/)) \
					 $(patsubst ${EN_DIR}/%/, ${LANG_DIR}/%.language, $(wildcard ${EN_DIR}/*/))

all: ${DIRS} ${TARGETS}

${DIRS}:
	mkdir -p $@

# This thing is quirky and doesn't detect changes in ${EN_DIR}/%/ files,
# so run make -B /.../.../...json manually when updating the en files,
# or create and delete a file in ${EN_DIR}/%/.
${JSON_DIR}/%.json: ${EN_DIR}/%/
	@echo "Building $@"
	./combine.sh $< $@

${POT_DIR}/%.pot: ${JSON_DIR}/%.json
	@echo "Building $@"
	json2po -P $< $@

${PO_DIR}/%.po: ${POT_DIR}/%.pot
	@echo "Building $@"
	./updatepo.sh $< $@

${LANG_DIR}/%.language: ${PO_DIR}/%.po
	@echo "Building $@"
	po2json -t ${JSON_DIR} $< $@ --removeuntranslated

	# Prettify
	jq . $@ > $@.tmp && mv $@.tmp $@
