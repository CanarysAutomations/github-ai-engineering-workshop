# Exercise 08 — Diagnose CI Build Failures and Fix with Coding Agent

**Duration**: 6 minutes
**GitHub/Copilot Feature**: GitHub Actions, Coding Agent, Copilot CLI
**Goal**: Configure code coverage, diagnose a CI build failure, and assign it to the coding agent to fix.

---

## Background

Build failures block merging. This exercise shows how to configure coverage thresholds, read CI failure logs, and delegate the fix to the coding agent via an issue.

---

## Step 1 — Configure Code Coverage Analysis
 
**Access Code Quality settings**
- Go to Repository → Settings → Code security and analysis
- Click **Code Quality**
 
**Select workflow files**
- Click **Configure** under Code coverage analysis
- Backend CI Coverage → Select `.github/workflows/backend-ci.yml`

**Enable coverage options**
- Show coverage in pull requests
- Show coverage badge
- Fail PR if coverage drops
 
**Save configuration**
- Click **Save**
- Verify "Code coverage configuration saved" confirmation

![Code Coverage Configuration](assets/configurecodecoverage.png)
---
## Step 2 — Verify Workflow Configuration
 
**Check coverage threshold**
- Go to Repository → `.github/workflows/backend-ci.yml`
- Find "Run tests with coverage" section
- Verify 80% minimum enforced:
  - `/p:Threshold=80` (80% minimum)
  - `/p:ThresholdType=line` (line coverage)
  - `/p:ThresholdStat=total` (whole project)
  
 
**Confirm quality gates**
- Verify "Coverage quality gate" step enforces 80% on PRs
- Verify "Upload backend test" uploads logs & coverage XML
- Verify "Generate HTML coverage report" creates artifact
 
 
---
 
## Step 3 — Diagnose Build Failure
 
- Go to your **Actions** tab
- Click the failed build run
- Expand logs to find error details

![Build Failure Logs](assets/buildissue.png)
 
**Identify error type**
- Test failures (test assertion errors)
- Compilation errors (syntax or missing dependencies)
- Missing implementations (incomplete methods/services)
- Note all relevant error messages
 
---
 
 
## Step 4 — Create Issue for Coding Agent
 
**Open Copilot in PR**
- Click **Copilot** icon
- Select **Immersive Mode**
- Type: `/create-issue`
 
**Fill issue details**
- Title: `Fix: Resolve build failure in [feature name]`
- Description: Include error logs and context
- Assignee: Coding Agent
 
**Set issue labels**
- Label: `type:bug`
- Label: `priority:high`
- Click **Create**
 
**Coding agent handles fix**
- Agent investigates the issue
- Agent creates linked PR with fixes
- Agent adds tests automatically
- Agent submits for review
 
**Verify and merge**
- Wait for linked PR creation
- Review agent's changes
- Merge when ready
 
---
 
## Step 5 — Build and View Coverage Report Artifact
 
- Go to **Actions** tab
- Click the successful build run
- Expand **Artifacts** section
- Click **backend-test-results** to download
- Extract the ZIP file — contains `coverage.cobertura.xml` and the HTML report

![Workflow Configuration](assets/codecoveragereport.png)
![Coverage Quality Gate](assets/codecoveragefile.png)

---

## Step 6 — Use Copilot CLI to Analyze & Fix Coverage Gaps (Optional)

When build fails with **coverage threshold errors**, use the Copilot CLI workflow to analyze and auto-fix.

### Workflow File
Location: `.github/workflows/copilot-cli-coverage-fixer.yml`

### How to Trigger (3 Modes)

**Via GitHub UI:**
- Go to **Actions** → **Copilot CLI - Fix Coverage Gaps**
- Set mode and click **"Run workflow"**

**GitHub CLI**:
```bash
# Analyze only
gh workflow run copilot-cli-coverage-fixer.yml -f fix_mode=analyze-only

# Preview test stubs
gh workflow run copilot-cli-coverage-fixer.yml -f fix_mode=auto-generate-tests

# Auto-commit fixes
gh workflow run copilot-cli-coverage-fixer.yml -f fix_mode=commit-fixes
```
---

## Verify

- [ ] Code coverage is configured
- [ ] Build issues are diagnosed and fixed
- [ ] Coverage report artifact is available

---

## Productivity Benefit

> An automated 80% coverage gate blocks regressions before they merge — no manual test audit needed. When the gate fails, delegating the fix to the Coding Agent means coverage gaps get closed without pulling a developer off their current task.

---

**Next**: [Exercise 09 — Enable GHAS + Fix Code Scanning](exercise-09-ghas-code-scanning.md)