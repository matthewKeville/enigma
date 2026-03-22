set -e

PROJECT_DIR="$PWD"
RELEASE_DIR="$PROJECT_DIR"/release
ZIP_DIR="$RELEASE_DIR"/zips

dotnet publish -r linux-x64 -p:IsPortableRelease=true
dotnet publish -r win-x64 -p:IsPortableRelease=true

mkdir -p "$ZIP_DIR"

zip -j -r "$ZIP_DIR"/enigma-linux-x64-portable.zip "$RELEASE_DIR"/linux-x64-portable
zip -j -r "$ZIP_DIR"/enigma-win-x64-portable.zip "$RELEASE_DIR"/win-x64-portable




