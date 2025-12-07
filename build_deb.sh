#!/bin/bash

# Exit on error
set -e

VERSION="${1:-1.0.0}"
ARCH="amd64"
PACKAGE_NAME="mtc"
DEB_NAME="${PACKAGE_NAME}_${VERSION}_${ARCH}.deb"
BUILD_DIR="deb_build"

echo "📦 Building Debian Package..."

# Clean previous build
rm -rf $BUILD_DIR
mkdir -p $BUILD_DIR/usr/local/bin
mkdir -p $BUILD_DIR/usr/share/mtc
mkdir -p $BUILD_DIR/DEBIAN

# Build binary (if not already built)
if [ ! -f "dist/linux-x64/MTC" ]; then
    echo "⚠️ Binary not found in dist/linux-x64. Running publish.sh..."
    ./publish.sh
fi

# Copy binary
echo "📄 Copying binary..."
cp dist/linux-x64/MTC $BUILD_DIR/usr/local/bin/mtc
chmod +x $BUILD_DIR/usr/local/bin/mtc

# Copy templates
echo "📄 Copying templates..."
cp -r Templates $BUILD_DIR/usr/share/mtc/Templates

# Create control file
echo "📝 Creating control file..."
cat <<EOF > $BUILD_DIR/DEBIAN/control
Package: $PACKAGE_NAME
Version: $VERSION
Section: devel
Priority: optional
Architecture: $ARCH
Maintainer: Juanse <juanse@example.com>
Description: Modular Template CLI for .NET
 A powerful CLI tool for scaffolding .NET projects using
 Clean Architecture, MVC, and Vertical Slice patterns.
EOF

# Build .deb
echo "🔨 Packaging..."
dpkg-deb --build $BUILD_DIR dist/$DEB_NAME

echo "✅ Package created: dist/$DEB_NAME"
# Cleanup
rm -rf $BUILD_DIR
