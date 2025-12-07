#!/bin/bash
set -e

AUR_DIR="mtc-aur/mtc-bin"

echo "🏹 Publishing to AUR..."

# Check if AUR dir exists
if [ ! -d "$AUR_DIR" ]; then
    echo "Error: Directory $AUR_DIR does not exist. Please clone the AUR repo first."
    echo "Run: git clone ssh://aur@aur.archlinux.org/mtc-bin.git mtc-aur/mtc-bin"
    exit 1
fi

# Update PKGBUILD in AUR dir
echo "Updating PKGBUILD..."
cp mtc-aur/PKGBUILD "$AUR_DIR/"

# Go to AUR dir
cd "$AUR_DIR"

# Generate .SRCINFO
echo "Generating .SRCINFO..."
makepkg --printsrcinfo > .SRCINFO

# Commit and Push
echo "Committing and Pushing..."
git add PKGBUILD .SRCINFO
git commit -m "Update version" || echo "No changes to commit"
git push origin master

echo "✅ AUR Publish Complete!"
