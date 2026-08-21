#!/usr/bin/env bash
# Scans a solution's NuGet dependencies for known vulnerabilities and reports them.
# High/Critical severity findings fail the job (::error:: + non-zero exit).
# Moderate/Low severity findings are reported as ::warning:: annotations only and do not fail the job.
# All findings (any severity) are written to the job summary.
# Usage: check-vulnerabilities.sh <solution-path>
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
  local level="$1"
  local title="$2"
  local message="$3"
  echo "::${level} title=$(escape_workflow_property "$title")::$(escape_workflow_command "$message")"
}

append_summary_line() {
  if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
    echo "$1" >> "$GITHUB_STEP_SUMMARY"
  fi
}

solution="${1:-}"
jq_bin="${JQ_BIN:-jq}"
dotnet_bin="${DOTNET_BIN:-dotnet}"

if [[ -z "$solution" ]]; then
  echo "Usage: check-vulnerabilities.sh <solution-path>" >&2
  exit 2
fi

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
report_json="${script_dir}/check-vulnerabilities.$$.json"
dotnet_stderr="${script_dir}/check-vulnerabilities.$$.dotnet.stderr"
jq_stderr="${script_dir}/check-vulnerabilities.$$.jq.stderr"

cleanup() {
  rm -f "$report_json" "$dotnet_stderr" "$jq_stderr"
}

trap cleanup EXIT

if ! command -v "$jq_bin" >/dev/null 2>&1; then
  emit_annotation error "Vulnerability scan failed" "jq is required to parse dotnet vulnerability JSON output."
  append_summary_line "### Vulnerability scan: $solution"
  append_summary_line ""
  append_summary_line "**The vulnerability scan could not run because jq was not available.**"
  exit 1
fi

if ! "$dotnet_bin" list "$solution" package --vulnerable --include-transitive --format json > "$report_json" 2> "$dotnet_stderr"; then
  emit_annotation error "Vulnerability scan failed" "dotnet list package failed for $solution. See step logs for details."
  append_summary_line "### Vulnerability scan: $solution"
  append_summary_line ""
  append_summary_line "**The vulnerability scan failed because dotnet list package did not complete successfully.**"

  if [[ -s "$dotnet_stderr" ]]; then
    cat "$dotnet_stderr" >&2
  fi

  exit 1
fi

if [[ ! -s "$report_json" ]]; then
  emit_annotation error "Vulnerability scan failed" "dotnet list package returned empty output for $solution."
  append_summary_line "### Vulnerability scan: $solution"
  append_summary_line ""
  append_summary_line "**The vulnerability scan failed because dotnet list package returned empty output.**"
  exit 1
fi

if ! findings=$("$jq_bin" -r '
  [
    .projects[]? | .path as $project | (.frameworks // [])[] |
    ((.topLevelPackages // []) + (.transitivePackages // []))[] |
    .id as $id | .resolvedVersion as $version | .vulnerabilities[]? |
    {
      project: $project,
      id: $id,
      version: $version,
      severity: .severity,
      advisoryUrl: (.advisoryurl // .advisoryUrl // "n/a")
    }
  ] | .[] | [.project, .id, .version, .severity, .advisoryUrl] | @tsv
' "$report_json" 2> "$jq_stderr"); then
  emit_annotation error "Vulnerability scan failed" "Failed to parse vulnerability JSON for $solution. Ensure dotnet list package returned valid JSON."
  append_summary_line "### Vulnerability scan: $solution"
  append_summary_line ""
  append_summary_line "**The vulnerability scan failed because the vulnerability report JSON was invalid or incomplete.**"

  if [[ -s "$jq_stderr" ]]; then
    cat "$jq_stderr" >&2
  fi

  exit 1
fi

findings="${findings//$'\r'/}"

if [[ -z "$findings" ]]; then
  append_summary_line "No known vulnerabilities found for $solution."
  exit 0
fi

has_blocking=0
while IFS=$'\t' read -r project id version severity advisory_url; do
  project_name=$(basename "$project")

  case "$severity" in
    High|Critical)
      emit_annotation error "Vulnerable package (${severity})" "${id} ${version} in ${project_name} — ${advisory_url}"
      has_blocking=1
      ;;
    *)
      emit_annotation warning "Vulnerable package (${severity})" "${id} ${version} in ${project_name} — ${advisory_url}"
      ;;
  esac
done <<< "$findings"

if ! grouped=$("$jq_bin" -r '
  def sevrank: if . == "Critical" then 0 elif . == "High" then 1 elif . == "Moderate" then 2 else 3 end;
  [
    .projects[]? | (.path | sub(".*[\\\\/]"; "")) as $project | (.frameworks // [])[] |
    ((.topLevelPackages // []) + (.transitivePackages // []))[] |
    .id as $id | .resolvedVersion as $version | .vulnerabilities[]? |
    {
      project: $project,
      id: $id,
      version: $version,
      severity: .severity,
      advisory: (.advisoryurl // .advisoryUrl // "n/a")
    }
  ]
  | group_by([.severity, .id, .version])
  | map({
      severity: .[0].severity,
      id: .[0].id,
      version: .[0].version,
      projects: ([.[].project] | unique),
      advisories: ([.[].advisory] | unique)
    })
  | sort_by([(.severity | sevrank), .id, .version])
  | .[]
  | [
      .severity,
      .id,
      .version,
      (.projects | join("<br>")),
      (.advisories | map("<a href=\"" + . + "\">" + (. | split("/") | last) + "</a>") | join("<br>"))
    ]
  | @tsv
' "$report_json" 2> "$jq_stderr"); then
  emit_annotation error "Vulnerability scan failed" "Failed to group vulnerability findings for $solution. See step logs for details."
  append_summary_line "### Vulnerability scan: $solution"
  append_summary_line ""
  append_summary_line "**The vulnerability scan failed while grouping findings for the summary table.**"

  if [[ -s "$jq_stderr" ]]; then
    cat "$jq_stderr" >&2
  fi

  exit 1
fi

grouped="${grouped//$'\r'/}"

append_summary_line "### Vulnerability scan: $solution"
append_summary_line ""

if [[ "$has_blocking" -eq 1 ]]; then
  append_summary_line "**High/Critical severity vulnerabilities were found — failing the job.**"
  append_summary_line ""
fi

current_severity=""
while IFS=$'\t' read -r severity id version projects advisories; do
  if [[ "$severity" != "$current_severity" ]]; then
    if [[ -n "$current_severity" ]]; then
      append_summary_line "</table>"
      append_summary_line ""
    fi

    current_severity="$severity"
    append_summary_line "#### $severity"
    append_summary_line ""
    append_summary_line "<table width=\"100%\">"
    append_summary_line "<colgroup><col width=\"20%\"><col width=\"12%\"><col width=\"28%\"><col width=\"40%\"></colgroup>"
    append_summary_line "<tr><th>Package</th><th>Version</th><th>Projects</th><th>Advisories</th></tr>"
  fi

  append_summary_line "<tr><td valign=\"top\">$id</td><td valign=\"top\">$version</td><td valign=\"top\">$projects</td><td valign=\"top\">$advisories</td></tr>"
done <<< "$grouped"

if [[ -n "$current_severity" ]]; then
  append_summary_line "</table>"
fi

if [[ "$has_blocking" -eq 1 ]]; then
  exit 1
fi

exit 0
