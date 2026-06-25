SUBMAKE := lang_mods plugin release

.PHONY: all ${SUBMAKE}
all: plugin ${SUBMAKE}

${SUBMAKE}:
	$(MAKE) -C $@
