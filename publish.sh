#!/bin/bash

# Exit on error
set -e

echo "🚀 Starting MTC Build Process..."

# Define version
VERSION="1.0.0"
OUTPUT_DIR="./dist"

# Clean previous builds
echo "🧹 Cleaning up..."
rm -rf $OUTPUT_DIR
mkdir -p $OUTPUT_DIR

# Build for Linux (x64)
echo "🐧 Building for Linux (x64)..."
dotnet publish MTC/MTC.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o $OUTPUT_DIR/linux-x64
# Copy templates to linux output
rm -rf $OUTPUT_DIR/linux-x64/templates
cp -r Templates $OUTPUT_DIR/linux-x64/templates
# Create tarball
cd $OUTPUT_DIR/linux-x64
tar -czvf ../mtc-linux-x64-$VERSION.tar.gz *
cd ../..

# Build for Windows (x64)
echo "🪟 Building for Windows (x64)..."
dotnet publish MTC/MTC.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o $OUTPUT_DIR/win-x64
# Copy templates to windows output
rm -rf $OUTPUT_DIR/win-x64/templates
cp -r Templates $OUTPUT_DIR/win-x64/templates
# Create zip (or tar if zip not found, but let's use tar for consistency in this env)
cd $OUTPUT_DIR/win-x64
tar -czvf ../mtc-win-x64-$VERSION.tar.gz *
cd ../..

# Build for macOS (x64)
echo "🍎 Building for macOS (x64)..."
dotnet publish MTC/MTC.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o $OUTPUT_DIR/osx-x64
# Copy templates to osx output
rm -rf $OUTPUT_DIR/osx-x64/templates
cp -r Templates $OUTPUT_DIR/osx-x64/templates
# Create tarball
cd $OUTPUT_DIR/osx-x64
tar -czvf ../mtc-osx-x64-$VERSION.tar.gz *
cd ../..

echo "✅ Build Complete! Artifacts are in $OUTPUT_DIR"
echo "✅ Build Complete! Artifacts are in $OUTPUT_DIR"
ls -lh $OUTPUT_DIR/*.tar.gz
