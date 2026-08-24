# Exercise 07 — Enable Code Quality, Create PR and Fix Code Quality Issues

> **Duration**: 8 minutes<br>
> **GitHub/Copilot Feature**: Code Quality, PR Copilot<br>
> **Goal**: Enable repository Code Quality scanning and create your first quality-checked PR from VS Code.

---

## Background

Code Quality scans your codebase for bugs, security issues, and violations. This exercise sets up automated scanning and validates a PR against quality standards.

---
 
## Step 1 — Enable Code Quality
 
Go to **Repository → Settings → Security and Quality → Code Quality**, Click the **Enable Code Quality**.


>**Note**: If you do not see the Code Quality tab, refer to [exercise-00-prerequisites.md](exercise-00-prerequisites.md) for setup instructions.
 
---
 
 
## Step 2 — Ensure PR is Open
 
If you haven't already, complete **Ex 06 Step 6** to create the PR for the Reviews feature. Code Quality findings will appear against that open PR.

---
 
## Step 3 — Review Findings & Generate Fix for Code Quality Issues
 
**Access Security and Quality tab on GitHub**
- Go to **github.com**
- Click on **Security and quality** tab
- Click on **Code Quality**
- Review standard findings (bugs, issues, violations)

![Code Quality Findings](assets/codequality.png)
---
## Step 4 — Generate fix for an issue
- Click on one of the findings listed
- Review the issue description and code snippet
- Click **Generate fix** button
- Copilot auto-fix modal appears
- Review the suggested fix
- Click **Accept** to apply Copilot-generated fix
- Commit is added to the PR automatically
![Copilot Fix Modal](assets/codequalitygeneratefix.png)

---
 
## Step 5 — Verify fix in Actions
- Go to **Actions** tab
- Wait for build to re-run with the fix
- Verify build passes (green ✓)

---

> **Tip**: The backend pipeline is the one that should produce the coverage artifact.

---


## Verify

- [ ] Code Quality scanning is enabled
- [ ] Quality issues are identified and fixed
- [ ] Build pipeline passes successfully 

---

## Productivity Benefit

> Finding and fixing a code quality issue post-release costs 10–100× more than catching it on a PR. Copilot Autofix closes the loop in seconds — the same fix that would require a developer to context-switch, investigate, patch, and re-review.

---

**Next**: [Exercise 08 — View the Coverage Report Artifact](exercise-08-coverage-report-artifact.md)


 

 

 
