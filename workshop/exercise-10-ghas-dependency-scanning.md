# Exercise 10 — Remediate Dependency Scanning Findings

> **Duration**: 8 minutes
> **Copilot Feature**: Dependabot, Dependency Scanning
> **Goal**: Remove the vulnerable client dependency while keeping the frontend build and runtime behavior unchanged.

---

## Background

Dependency scanning is useful when the vulnerable package is not part of the live runtime path. Here, the client has a vulnerable package in its manifest and lockfile, update the dependency metadata without changing the UI code.
---

## Step 1 — Enable Dependency Scanning

Open **Security and quality** → **Dependency scanning** and enable dependency scanning for the repository.



> **Note**: Enable the scanner first, then open the alert so the rest of the flow is visible end-to-end. Refer to [exercise-00-prerequisites.md](exercise-00-prerequisites.md) for setup instructions if you do not see the alert.

---

## Step 2 — Inspect the Vulnerability

Open the Dependabot alert and read the dependency path that leads to the vulnerable package.



> **Tip**: Focus on the exact package and version, not the whole client app.

---

## Step 3 — Run Dependabot fix

Use Dependabot fix for the dependency alert, then choose the option to commit the fix in a new branch.



> **Tip**: The fix flow should offer a commit in a new branch; that is the expected workshop outcome.

---

## Step 4 — Commit the Fix

Review the generated diff, commit the new branch, and confirm the alert is closed or moved to a fixed state.

---

## Verify

- [ ] Dependabot alert is closed or resolved
- [ ] Client build still succeeds
- [ ] Dependency manifest and lockfile stay in sync
- [ ] Fix is committed in a new branch

---

## Key Takeaway

> Dependency fixes should stay surgical: change the package metadata, not the working app.

---

**Next**: [Exercise 11 — Enable Secret Scanning + Remove Secret](exercise-11-ghas-secret-scanning.md)