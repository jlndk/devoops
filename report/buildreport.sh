if ! [ -x "$(command -v pandoc)" ]; then
	echo 'Error: pandoc is not installed.' >&2
	echo '	apt-get install pandoc to solve this' >&2 
	exit 1
fi
for f in [0-9][0-9]*.md; do
    sed -i -e '$a\' $f
done
FILES=$(ls | grep -E '\<[0-9]{2}.*.md\>' | tr '\n' ' ')
echo 'Ensure that chapters are named in order with 01_NAME, 02_NAME' >&1
echo 'This results in the chapters being in that order in the finished report' >&1
pandoc -f markdown_github -t markdown_github $FILES > finished-report.md