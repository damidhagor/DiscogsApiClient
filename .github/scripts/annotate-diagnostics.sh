#!/usr/bin/env bash
# Parses a `dotnet build` or `dotnet format` log and emits non-failing GitHub Actions
# `::warning::` annotations for each reported warning/diagnostic line, without affecting
# the calling step's exit code. All findings are also written to the job summary so
# they're visible from the workflow run's Summary page without opening the Annotations
# panel. Usage: annotate-diagnostics.sh <log-file>
set -uo pipefail

escape_workflow_command() {
  local value="$1"
  value="${value//'%'/'%25'}"
  value="${value//$'\r'/'%0D'}"
  value="${value//$'\n'/'%0A'}"
  printf '%s' "$value"
}

escape_workflow_property() {
  local value
  value="$(escape_workflow_command "$1")"
  value="${value//':'/'%3A'}"
  value="${value//','/'%2C'}"
  printf '%s' "$value"
}

escape_markdown_table_cell() {
  printf '%s' "${1//'|'/'\|'}"
}

append_summary_line() {
  if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
    echo "$1" >> "$GITHUB_STEP_SUMMARY"
  fi
}

log_file="${1:-}"

if [[ -z "$log_file" ]]; then
  echo "Usage: annotate-diagnostics.sh <log-file>" >&2
  exit 0
fi

if [[ ! -f "$log_file" ]]; then
  echo "Log file not found: $log_file" >&2
  exit 0
fi

# MSBuild/dotnet-format diagnostic lines look like:
#   <file>(<line>,<col>): warning <CODE>: <message> [<project>]
matches="$(grep -E '(^|: )warning [A-Za-z]+[0-9]+:' "$log_file" || true)"

if [[ -z "$matches" ]]; then
  append_summary_line "No warnings found in $log_file."
  exit 0
fi

append_summary_line "### Warnings: $log_file"
append_summary_line ""
append_summary_line "| File | Line | Code | Message |"
append_summary_line "|---|---|---|---|"

while IFS= read -r line; do
  if [[ "$line" =~ ^(.*)\(([0-9]+),([0-9]+)\):[[:space:]]warning[[:space:]]([A-Za-z0-9]+):[[:space:]](.*)[[:space:]]\[(.*)\]$ ]]; then
    file="${BASH_REMATCH[1]}"
    line_number="${BASH_REMATCH[2]}"
    code="${BASH_REMATCH[4]}"
    message="${BASH_REMATCH[5]}"
    echo "::warning file=$(escape_workflow_property "$file"),line=${line_number},title=$(escape_workflow_property "$code")::$(escape_workflow_command "$message")"
    append_summary_line "| $(escape_markdown_table_cell "$file") | $line_number | $code | $(escape_markdown_table_cell "$message") |"
  else
    echo "::warning::$(escape_workflow_command "$line")"
    append_summary_line "| — | — | — | $(escape_markdown_table_cell "$line") |"
  fi
done <<< "$matches"

exit 0
