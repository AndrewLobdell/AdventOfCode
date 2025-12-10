#!/usr/bin/env bash
set -euo pipefail

# Script to scaffold a new Advent of Code day.
# Usage: ./new-day.sh
# Run from the repository root.

AOC_YEAR=2025

read -rp "Enter day number (1-25): " day

if ! [[ "$day" =~ ^[0-9]+$ ]]; then
  echo "Day must be a number." >&2
  exit 1
fi

if (( day < 1 || day > 25 )); then
  echo "Day must be between 1 and 25." >&2
  exit 1
fi

printf -v day_padded "%02d" "$day"
echo "Creating Day${day_padded}..."

# Check whether day already exists
scaffold_code=true
scaffold_inputs=true

if [[ -f "Days/Day${day_padded}.cs" ]]; then
  echo "Days/Day${day_padded}.cs already exists. Will not overwrite code file."
  scaffold_code=false
fi

if [[ -d "Inputs/Day${day_padded}" ]]; then
  echo "Inputs/Day${day_padded} already exists. Will not overwrite existing inputs."
  scaffold_inputs=false
fi

# Use DayTemplate as template for code and tests
if [[ ! -f "Days/DayTemplate.cs" ]]; then
  echo "Template Days/DayTemplate.cs not found. Aborting." >&2
  exit 1
fi

if [[ ! -f "AdventOfCode2025.Tests/DayTemplateTests.cs" ]]; then
  echo "Template AdventOfCode2025.Tests/DayTemplateTests.cs not found. Aborting." >&2
  exit 1
fi

# Create day class from template (if not already present)
if [[ "$scaffold_code" == true ]]; then
  cp "Days/DayTemplate.cs" "Days/Day${day_padded}.cs"
  # Replace class name DayTemplate -> DayXX
  sed -i '' "s/DayTemplate/Day${day_padded}/g" "Days/Day${day_padded}.cs"
fi

# Create inputs directory and empty files (if not already present)
if [[ "$scaffold_inputs" == true ]]; then
  mkdir -p "Inputs/Day${day_padded}"
  : > "Inputs/Day${day_padded}/input.txt"
fi

# Try to automatically download puzzle input and description
AOC_BASE_URL="https://adventofcode.com/${AOC_YEAR}/day/${day}"
real_input_downloaded=false

if [[ -n "${AOC_SESSION:-}" ]]; then
  echo "Attempting to download REAL input for Day${day_padded} using AOC_SESSION..."
  if curl -sSf --cookie "session=${AOC_SESSION}" "${AOC_BASE_URL}/input" -o "Inputs/Day${day_padded}/input.txt"; then
    echo "Downloaded REAL input to Inputs/Day${day_padded}/input.txt."
    real_input_downloaded=true
  else
    echo "Failed to download REAL input; leaving Inputs/Day${day_padded}/input.txt as-is." >&2
  fi
else
  echo "Environment variable AOC_SESSION not set; skipping automatic REAL input download."
fi

if command -v w3m >/dev/null 2>&1; then
  NOTES_DIR="Notes/Day${day_padded}"
  mkdir -p "${NOTES_DIR}"
  # If we have AOC_SESSION, use authenticated fetch so we see the correct
  # per-user state (e.g., already-solved banner).
  if [[ -n "${AOC_SESSION:-}" ]]; then
    if curl -sSf --cookie "session=${AOC_SESSION}" "${AOC_BASE_URL}" \
      | w3m -dump -T text/html -cols 120 \
      | awk 'BEGIN{in_body=0} /--- Day/{in_body=1} /To play, please/{exit} in_body {print}' \
      > "${NOTES_DIR}/part1.txt"; then
      echo "Saved Part 1 puzzle description (trimmed, authenticated) to ${NOTES_DIR}/part1.txt."
    else
      echo "Failed to download puzzle description with curl+w3m." >&2
    fi
  else
    if w3m -dump -cols 120 "${AOC_BASE_URL}" \
      | awk 'BEGIN{in_body=0} /--- Day/{in_body=1} /To play, please/{exit} in_body {print}' \
      > "${NOTES_DIR}/part1.txt"; then
      echo "Saved Part 1 puzzle description (trimmed) to ${NOTES_DIR}/part1.txt."
    else
      echo "Failed to download puzzle description with w3m." >&2
    fi
  fi
else
  echo "w3m not found on PATH; skipping automatic puzzle description dump."
fi

# Create test file from DayTemplateTests template
cp "AdventOfCode2025.Tests/DayTemplateTests.cs" "AdventOfCode2025.Tests/Day${day_padded}Tests.cs"
sed -i '' "s/DayTemplate/Day${day_padded}/g" "AdventOfCode2025.Tests/Day${day_padded}Tests.cs"
sed -i '' "s/DayTemplate/Day${day_padded}/g" "AdventOfCode2025.Tests/Day${day_padded}Tests.cs"

# Create per-day test data directory and empty example input/expected files under the test project
TESTDATA_DIR="AdventOfCode2025.Tests/TestData/Day${day_padded}"
mkdir -p "$TESTDATA_DIR"
: > "$TESTDATA_DIR/part1.example.input.txt"
: > "$TESTDATA_DIR/part1.example.expected.txt"
: > "$TESTDATA_DIR/part2.example.input.txt"
: > "$TESTDATA_DIR/part2.example.expected.txt"

# Insert new mapping into Program.cs before NEW_DAY_MARKER
mapping_line="            ${day} => new Day${day_padded}(),"
perl -pi -e "s|            // NEW_DAY_MARKER|${mapping_line}\n            // NEW_DAY_MARKER|" Program.cs

echo
if [[ "$real_input_downloaded" == true ]]; then
  echo "REAL input has already been downloaded to Inputs/Day${day_padded}/input.txt."
  echo "If you want to override it, edit that file manually after this script finishes."
else
  echo "Optional: paste REAL input for Day${day_padded}. When done, press Ctrl-D."
  cat > "Inputs/Day${day_padded}/input.txt" || true
fi

echo

echo "Day${day_padded} scaffolded. You can now implement Days/Day${day_padded}.cs and adjust tests as needed."
