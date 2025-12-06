#!/bin/bash

# Exit on error
set -e

VERSION="1.0.0"
PACKAGE_NAME="mtc"
SOURCE_DIR="${PACKAGE_NAME}-${VERSION}"
TARBALL="${PACKAGE_NAME}_${VERSION}.orig.tar.gz"

echo "📦 Preparing Source Package for PPA..."

# Clean previous build
rm -rf ppa_build
mkdir -p ppa_build

# Create source directory
echo "📂 Creating source directory..."
mkdir -p ppa_build/$SOURCE_DIR
cp -r MTC ppa_build/$SOURCE_DIR/
cp -r Templates ppa_build/$SOURCE_DIR/
cp MTC.sln ppa_build/$SOURCE_DIR/

# Copy debian directory
echo "📄 Copying debian directory..."
cp -r debian ppa_build/$SOURCE_DIR/

# Create orig tarball
echo "📦 Creating orig tarball..."
cd ppa_build
tar -czvf $TARBALL $SOURCE_DIR
cd ..

echo "✅ Source package prepared in ppa_build/"
echo "👉 To build source package (requires debhelper):"
echo "   cd ppa_build/$SOURCE_DIR"
echo "   debuild -S -sa"
