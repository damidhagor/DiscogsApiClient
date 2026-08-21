#!/usr/bin/env bash
# Runs `dotnet format --verify-no-changes` and converts its plain-text diagnostic
# output into GitHub Actions annotations. Unlike `dotnet build`/`dotnet test`,
# `dotnet format` does not emit annotations on its own -- it only prints plain text.
# Any diagnostic at the configured --severity threshold fails the job (dotnet
# format's own exit code is preserved).
# Usage: format-check.sh <solution-path>
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

emit_annotation() {
  local level="$1" file="$2" line="$3" col="$4" title="$5" message="$6"
  echo "::${level} file=$(escape_workflow_property "$file"),line=${line},col=${col},title=$(escape_workflow_property "$title")::$(escape_workflow_command "$message")"
}

append_summary_line() {
  if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
    echo "$1" >> "$GITHUB_STEP_SUMMARY"
  fi
}

relative_path() {
  local raw_path="$1"
  local normalized_path="${raw_path//\\//}"
  local normalized_workspace="${workspace//\\//}"
  printf '%s' "${normalized_path#"$normalized_workspace"/}"
}

solution="${1:-}"
dotnet_bin="${DOTNET_BIN:-dotnet}"

if [[ -z "$solution" ]]; then
  echo "Usage: format-check.sh <solution-path>" >&2
  exit 2
fi

workspace="${GITHUB_WORKSPACE:-$(pwd)}"
output_file="$(mktemp)"
trap 'rm -f "$output_file"' EXIT

set +e
"$dotnet_bin" format "$solution" --verify-no-changes --severity info --no-restore > "$output_file" 2>&1
format_exit=$?
set -e

cat "$output_file"

diagnostic_pattern='^(.+)\(([0-9]+),([0-9]+)\): (error|warning|info|hidden) ([A-Za-z0-9]+): (.*) \[(.+)\]$'
reformat_pattern="^Formatted code file '(.+)' has changes\\.$"

findings=()
while IFS= read -r line; do
  line="${line%$'\r'}"
  if [[ "$line" =~ $diagnostic_pattern ]] || [[ "$line" =~ $reformat_pattern ]]; then
    findings+=("$line")
  fi
done < "$output_file"

if [[ "${#findings[@]}" -eq 0 ]]; then
  append_summary_line "No formatting or style diagnostics found for $solution."
  exit "$format_exit"
fi

append_summary_line "### Format check: $solution"
append_summary_line ""
append_summary_line "| Project | Location | Severity | Rule | Message |"
append_summary_line "|---|---|---|---|---|"

for line in "${findings[@]}"; do
  if [[ "$line" =~ $diagnostic_pattern ]]; then
    raw_path="${BASH_REMATCH[1]}"
    line_no="${BASH_REMATCH[2]}"
    col_no="${BASH_REMATCH[3]}"
    severity="${BASH_REMATCH[4]}"
    rule_id="${BASH_REMATCH[5]}"
    message="${BASH_REMATCH[6]}"
    project="${BASH_REMATCH[7]}"
    rel_path="$(relative_path "$raw_path")"

    case "$severity" in
      error) level=error ;;
      warning) level=warning ;;
      *) level=notice ;;
    esac

    emit_annotation "$level" "$rel_path" "$line_no" "$col_no" "$rule_id" "$message"
    append_summary_line "| $(basename "$project") | ${rel_path}:${line_no} | $severity | $rule_id | $message |"
  elif [[ "$line" =~ $reformat_pattern ]]; then
    rel_path="$(relative_path "${BASH_REMATCH[1]}")"

    emit_annotation warning "$rel_path" 1 1 "Formatting" "File is not formatted. Run 'dotnet format' locally to fix."
    append_summary_line "| - | ${rel_path}:1 | warning | Formatting | File is not formatted. Run 'dotnet format' locally to fix. |"
  fi
done

exit "$format_exit"
