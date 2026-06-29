#!/usr/bin/env bash
#
# Deploy a Unity WebGL build to the gh-pages branch.
#
# This is a PROJECT site served at  alikassab.dev/Mystic-Sands  - the custom
# domain is inherited from the AliKassab.github.io user site, so this repo must
# NOT carry its own CNAME file (that would collide with the apex domain).
#
# The gh-pages branch is rebuilt from scratch on every run so it contains ONLY
# the website data (the Unity build output + .nojekyll) - never the Unity
# project itself.
#
# Usage:
#   ./deploy-web.sh <path-to-unity-webgl-build-folder>
#
# The build folder is the one Unity writes when you do
# File > Build Settings > WebGL > Build. It must contain index.html and Build/.
#
set -euo pipefail

BUILD_DIR="${1:?Usage: ./deploy-web.sh <unity-webgl-build-folder>}"
BRANCH="gh-pages"

# Full public URL of the .data file on Cloudflare R2. The deploy rewrites
# index.html to load the data from here, so it survives Unity regenerating
# index.html on every rebuild. SET THIS before your first real deploy.
R2_DATA_URL="https://REPLACE-WITH-YOUR-R2-URL/Mystic-Sands.data.unityweb"

if [ ! -f "$BUILD_DIR/index.html" ]; then
  echo "ERROR: no index.html in '$BUILD_DIR'." >&2
  echo "Point this at the Unity WebGL build output folder (has index.html + Build/)." >&2
  exit 1
fi

git rev-parse --is-inside-work-tree >/dev/null 2>&1 || { echo "Run inside the git repo." >&2; exit 1; }

WORKTREE="$(mktemp -d)"
cleanup() { git worktree remove --force "$WORKTREE" >/dev/null 2>&1 || true; }
trap cleanup EXIT

# Fresh orphan worktree so gh-pages history/content is just the site.
git worktree add --force --detach "$WORKTREE" >/dev/null
(
  cd "$WORKTREE"
  git checkout --orphan "$BRANCH"
  git rm -rf . >/dev/null 2>&1 || true

  # Copy the build output to the branch root.
  # Exclude the big *.data.unityweb - it is hosted on Cloudflare R2, not Pages
  # (GitHub rejects files over 100MB, and the data file is larger than that).
  rsync -a --delete --exclude '.git' --exclude '*.data.unityweb' "${BUILD_DIR%/}/" ./

  if ls *.data.unityweb Build/*.data.unityweb >/dev/null 2>&1; then
    echo "WARNING: a .data.unityweb slipped through - it must be on R2, not Pages." >&2
  fi

  # Point index.html at the R2-hosted data file (Unity regenerates this file on
  # every build with a relative Build/ path, so we rewrite it here each deploy).
  if [ "$R2_DATA_URL" = "https://REPLACE-WITH-YOUR-R2-URL/Mystic-Sands.data.unityweb" ]; then
    echo "WARNING: R2_DATA_URL is still the placeholder - the game will not load until you set it." >&2
  fi
  perl -0pi -e "s{dataUrl:\s*[^,]+/[^/\"]+\.data\.unityweb\"}{dataUrl: \"$R2_DATA_URL\"}g" index.html

  # No CNAME: domain is inherited from the user site as a subpath.
  touch .nojekyll           # serve Build/ and underscore files verbatim

  git add -A
  git commit -m "Deploy WebGL build" >/dev/null
  git push -f origin "$BRANCH"
)

echo
echo "Deployed to branch '$BRANCH'."
echo "GitHub > Settings > Pages: source = '$BRANCH' / root (no custom domain here)."
echo "Live at: https://alikassab.dev/Mystic-Sands/"
