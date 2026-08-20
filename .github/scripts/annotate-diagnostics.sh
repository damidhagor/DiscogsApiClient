#!/usr/bin/env bash
# Parses a `dotnet build` or `dotnet format` log and emits non-failing GitHub Actions
# `::warning::` annotations for each reported warning/diagnostic line, without affecting
# the calling step's exit code. Usage: annotate-diagnostics.sh <log-file>
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
while IFS= read -r line; do
  if [[ "$line" =~ ^(.*)\(([0-9]+),([0-9]+)\):[[:space:]]warning[[:space:]]([A-Za-z0-9]+):[[:space:]](.*)[[:space:]]\[(.*)\]$ ]]; then
    file="$(escape_workflow_property "${BASH_REMATCH[1]}")"
    line_number="${BASH_REMATCH[2]}"
    code="$(escape_workflow_property "${BASH_REMATCH[4]}")"
    message="$(escape_workflow_command "${BASH_REMATCH[5]}")"
    echo "::warning file=${file},line=${line_number},title=${code}::${message}"
  else
    echo "::warning::$(escape_workflow_command "$line")"
  fi
done < <(grep -E '(^|: )warning [A-Za-z]+[0-9]+:' "$log_file" || true)

exit 0
