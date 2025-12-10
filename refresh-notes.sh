#!/usr/bin/env bash
set -euo pipefail

# Refresh Advent of Code notes after solving.
# - Optionally submits your Part 1 answer using AOC_SESSION.
# - Optionally submits your Part 2 answer using AOC_SESSION.
# - Then dumps the full (Part 1 + Part 2) description and a Part 2-only view
#   into Notes/DayXX.

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
AOC_BASE_URL="https://adventofcode.com/${AOC_YEAR}/day/${day}"
ANSWER_URL="${AOC_BASE_URL}/answer"

if [[ -z "${AOC_SESSION:-}" ]]; then
  echo "Environment variable AOC_SESSION is not set; cannot submit answers or fetch gated content." >&2
  echo "You can still fetch if you've already solved Part 1 in the browser by just using w3m manually."
  exit 1
fi

read -rp "Submit Part 1 answer via this script before refreshing notes? [y/N]: " submit_choice
submit_choice=${submit_choice:-N}

if [[ "$submit_choice" == [yY] ]]; then
  read -rp "Enter your Part 1 answer: " part1_answer
  echo "Submitting Part 1 answer for Day${day_padded}..."

  # Advent of Code expects POST with form fields level=1&answer=...
  # We send the session cookie via AOC_SESSION.
  response=$(curl -sS -X POST \
    -H "Content-Type: application/x-www-form-urlencoded" \
    --cookie "session=${AOC_SESSION}" \
    --data-urlencode "level=1" \
    --data-urlencode "answer=${part1_answer}" \
    "${ANSWER_URL}")

  if grep -qi "That's the right answer" <<<"$response"; then
    echo "Server says: That's the right answer! Part 2 should now be unlocked."
  elif grep -qi "That's not the right answer" <<<"$response"; then
    echo "Server says: That's not the right answer." >&2
    echo "Not refreshing notes, since Part 2 is still locked." >&2
    exit 1
  elif grep -qi "You gave an answer too recently" <<<"$response"; then
    echo "Server says: You gave an answer too recently; please wait before trying again." >&2
    exit 1
  elif grep -qi "Did you already complete it" <<<"$response"; then
    echo "Server indicates this level is already complete; treating as success."
  else
    # Fallback: if we didn't get an explicit wrong/ratelimit and the page mentions
    # the puzzle is already complete, also treat it as success.
    if grep -qi "The first half of this puzzle is complete" <<<"$response"; then
      echo "Server shows first half already complete; treating as success."
    else
      echo "Got an unexpected response from the server. Saving to Notes/Day${day_padded}_answer_response.html for inspection." >&2
      mkdir -p Notes
      printf '%s
' "$response" > "Notes/Day${day_padded}_answer_response.html"
      exit 1
    fi
  fi
else
  echo "Skipping Part 1 submission. Make sure you've already submitted the correct Part 1 answer in the browser."
fi

# Optional Part 2 submission
read -rp "Submit Part 2 answer via this script before refreshing notes? [y/N]: " submit_choice2
submit_choice2=${submit_choice2:-N}

if [[ "$submit_choice2" == [yY] ]]; then
  read -rp "Enter your Part 2 answer: " part2_answer
  echo "Submitting Part 2 answer for Day${day_padded}..."

  # Advent of Code expects POST with form fields level=2&answer=...
  response2=$(curl -sS -X POST \
    -H "Content-Type: application/x-www-form-urlencoded" \
    --cookie "session=${AOC_SESSION}" \
    --data-urlencode "level=2" \
    --data-urlencode "answer=${part2_answer}" \
    "${ANSWER_URL}")

  if grep -qi "That's the right answer" <<<"$response2"; then
    echo "Server says: That's the right answer for Part 2!"
  elif grep -qi "That's not the right answer" <<<"$response2"; then
    echo "Server says: That's not the right answer for Part 2." >&2
  elif grep -qi "You gave an answer too recently" <<<"$response2"; then
    echo "Server says: You gave an answer too recently; please wait before trying again." >&2
  elif grep -qi "Did you already complete it" <<<"$response2"; then
    echo "Server indicates Part 2 is already complete; treating as success."
  else
    # Fallback: if we didn't get explicit wrong/ratelimit and the page mentions
    # the puzzle is already complete, also treat it as success.
    if grep -qi "Both parts of this puzzle are complete" <<<"$response2"; then
      echo "Server shows both parts already complete; treating Part 2 submission as success."
    else
      echo "Got an unexpected response from the server for Part 2. Saving to Notes/Day${day_padded}_answer2_response.html for inspection." >&2
      mkdir -p Notes
      printf '%s
' "$response2" > "Notes/Day${day_padded}_answer2_response.html"
    fi
  fi
else
  echo "Skipping Part 2 submission. Make sure you've already submitted the correct Part 2 answer in the browser."
fi

if ! command -v w3m >/dev/null 2>&1; then
  echo "w3m not found on PATH; cannot dump puzzle description." >&2
  exit 1
fi

NOTES_DIR="Notes/Day${day_padded}"
mkdir -p "${NOTES_DIR}"

PART1_NOTES="${NOTES_DIR}/part1.txt"
FULL_NOTES="${NOTES_DIR}/full.txt"
PART2_NOTES="${NOTES_DIR}/part2.txt"

echo "Fetching full puzzle description for Day${day_padded}..."

# Dump the full page, then:
# - Strip everything up to and including the first "--- Day" header line.
# - Stop before the login/"To play, please" footer.
# - If Part 1 notes exist, derive Part 2 by removing duplicated lines.

TMP_FULL=$(mktemp)

# Fetch authenticated HTML with curl (using AOC_SESSION), then render via w3m
# and trim starting from the first "--- Day" header (inclusive).
if curl -sSf --cookie "session=${AOC_SESSION}" "${AOC_BASE_URL}" \
  | w3m -dump -T text/html -cols 120 \
  | awk 'BEGIN{in_body=0} /--- Day/{in_body=1} in_body {print}' \
  > "${TMP_FULL}"; then
  echo "Trimmed authenticated puzzle description into temporary file ${TMP_FULL}."
else
  echo "Failed to download or trim puzzle description with curl+w3m." >&2
  rm -f "${TMP_FULL}"
  exit 1
fi

# Always keep a full trimmed copy for reference
cp "${TMP_FULL}" "${FULL_NOTES}"
echo "Saved full trimmed description to ${FULL_NOTES}."

if [[ -f "${PART1_NOTES}" ]]; then
  echo "Found Part 1 notes at ${PART1_NOTES}; deriving Part 2 by removing duplicated lines."
  # Remove any lines from the full description that are exactly present in the
  # Part 1 notes file. This keeps ordering from the full description but
  # filters out verbatim duplicates.
  #
  # Note: grep exits with status 1 if no lines are selected, which is not an
  # error for us (it just means everything matched Part 1). We therefore do
  # not treat a non-zero exit as fatal unless it is a true grep error (2),
  # but the simplest is to run it and not gate on the exit code.
  grep -Fvx -f "${PART1_NOTES}" "${TMP_FULL}" > "${PART2_NOTES}" || true
  echo "Saved Part 2-ish notes (full minus Part 1 lines) to ${PART2_NOTES}."
else
  echo "Part 1 notes not found at ${PART1_NOTES}; no separate Part 2 file will be generated."
fi

if [[ -n "${TMP_FULL}" && -f "${TMP_FULL}" ]]; then
  rm -f "${TMP_FULL}"
fi
