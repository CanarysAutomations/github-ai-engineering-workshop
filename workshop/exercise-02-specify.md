# Exercise 02 — Specify: Define Wishlist + Reviews Requirements

> **Duration**: 10 minutes<br>
> **GitHub/Copilot Feature**: Spec Kit Specify, GitHub MCP <br>
> **Goal**: Transform the parent issue into a detailed specification with user goals, acceptance criteria, and non-functional requirements.

---

## Background

The **Specify** phase converts GitHub issue requirements into structured documents: User Goals, Acceptance Criteria , Non-Functional Requirements, Architecture & Integration Points. Using `#issue_read` (GitHub MCP tool), Spec Kit reads your parent issue and enriches it into a full specification.

---

## Step 1 — Run Spec Kit Specify with GitHub MCP

In Copilot Chat (`Ctrl+Alt+I`), use the GitHub MCP `#issue_read` tool to reference your parent issue from Exercise 01. Paste this prompt:

```
/speckit.specify #issue_read <ISSUE-NUMBER>

```


Where `<ISSUE-NUMBER>` is your parent issue number from Exercise 01.

Spec Kit will read the parent issue and generate a detailed specification covering:
- User Goals & Acceptance Criteria (map from issue)
- Non-Functional Requirements (in-memory storage, JWT auth, concurrency)
- Architecture & Integration 


Output: .specs/001-add-wishlist-reviews/spec.md

> **Tip**: GitHub MCP tool `#issue_read` allows Copilot to read your GitHub issue directly. If GitHub MCP is not configured, you can manually paste the issue body into the prompt instead.
> **Note**: Make sure GitHub MCP is enabled; refer to [exercise-00-prerequisites.md](exercise-00-prerequisites.md) for setup instructions.


---

## Step 2 — Review & Commit Specification

Once Copilot completes, open `.specs/001-add-wishlist-reviews/spec.md` and verify:

- Should map to all issue ACs
- Includes: Summary, User Goals, Acceptance Criteria, Non-Functional Requirements
- Defines required services and DTOs for Wishlist and Reviews features

Commit:

```bash
git add .specs/001-add-wishlist-reviews/spec.md && git commit -m "spec: wishlist and reviews"
```

---

## Verify

- [ ] `.specs/001-add-wishlist-reviews/spec.md` exists
- [ ] Spec maps all parent issue ACs
- [ ] Includes Non-Functional Requirements for in-memory storage, auth, and concurrency
- [ ] Defines the required services and DTOs

---

## Key Takeaway

> GitHub issue → Spec with full traceability. Specification is the single source of truth for the feature.

---

**Next**: [Exercise 03 — Plan: Design the Technical Architecture](exercise-03-plan.md)
