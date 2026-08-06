# Exercise 04 — Tasks and Sub-Issues: Break into Implementation Work Items

> **Duration**: 10 minutes
> **Copilot Feature**: Spec Kit Checklist, Tasks & GitHub MCP Sub-Issues
> **Goal**: Decompose the plan into ordered tasks, create GitHub sub-issues, and preserve traceability for implementation.

---

## Background

The **Tasks** phase breaks the plan into 2 focused phases: (1) Complete Wishlist feature, (2) Complete Reviews feature. Once the task list is stable, each item is promoted to a GitHub sub-issue so implementation work stays traceable back to the parent issue.

---


## Step 1 — Run Spec Kit Tasks (2 Phases Only)

Paste this focused tasks prompt:

```
/speckit.tasks 2 phases only

Complete Wishlist feature
Complete Reviews feature


```

---

## Step 3 — Convert Tasks into GitHub Sub-Issues

Use the task list to create sub-issues linked to the parent issue from Exercise 01.

```text
Refer #file:tasks.md and Use GitHub MCP #sub_issue_write to create detailed sub-issue per phase to the parent issue number 11
```

> **Tip**: If the MCP tool is unavailable, create the sub-issues manually.

![Spec Kit Tasks](assets/subissue.png)

---

## Verify

- [ ] `.specs/001-add-wishlist-reviews/checklist.md` exists
- [ ] `.specs/001-add-wishlist-reviews/tasks.md` exists
- [ ] Tasks organized in 2 phases (Wishlist, Reviews)
- [ ] Every task is linked to a GitHub sub-issue


---

## Key Takeaway

> Two focused phases plus sub-issue traceability keep implementation work visible, reviewable, and easy to close out.

---

**Next**: [Exercise 05 — Wishlist Implementation with Copilot Coding Agent](exercise-05-wishlist-coding-agent.md)
