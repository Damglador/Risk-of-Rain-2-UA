
all: source/output-ukrainian.json source/DLC3.json

source/output-ukrainian.json: extra/* en
	./build-output.sh en/ source/output-ukrainian.json

	# https://stackoverflow.com/questions/19529688/how-to-merge-2-json-objects-from-2-files-using-jq#24904276
	# and Claude
	jq -s 'reduce .[] as $$x ({}; . * $$x)' source/output-ukrainian.json extra/* > \
		/tmp/output-ukrainian.json && \
		mv /tmp/output-ukrainian.json source/output-ukrainian.json

source/DLC3.json: en/DLC3.json
	cp en/DLC3.json source/DLC3.json
