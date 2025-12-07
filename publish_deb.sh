#!/bin/bash
set -e

echo "� Building Debian package..."

# Run the existing build_deb.sh script
if [ -f "./build_deb.sh" ]; then
    chmod +x build_deb.sh
    ./build_deb.sh
else
    echo "Error: build_deb.sh not found!"
    exit 1
fi

echo "✅ Debian package built successfully!"
echo "📍 Package location: dist/mtc_1.0.0_amd64.deb"
echo ""
echo "To install locally:"
echo "  sudo dpkg -i dist/mtc_1.0.0_amd64.deb"
echo ""
echo "To upload to GitHub Release:"
echo "  Upload the file: dist/mtc_1.0.0_amd64.deb"