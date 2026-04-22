BASE_DIR := en
OUT_DIR  := source

TARGETS := $(patsubst $(BASE_DIR)/%/, $(OUT_DIR)/%.language, $(wildcard $(BASE_DIR)/*/))

all: $(TARGETS)

$(OUT_DIR)/%.language: $(BASE_DIR)/%/
	@mkdir -p $(OUT_DIR)
	@echo "Building $@"
	# твої команди тут
	./combine.sh $< $@
