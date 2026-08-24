# Jira Alternative Workflow 

> **Duration**: 12 minutes<br>
> **Tool**: Atlassian MCP Server <br>
> **Goal**: Use Jira instead of GitHub Issues for constitution setup and task decomposition in a single unified workflow.

---

## Background

This guide provides a **Jira-first alternative** to Exercises 01 and 04. Instead of GitHub Issues, use Atlassian MCP Server's `createjiraissue` tool to:
1. Create a parent **Epic** or **Task** for the eShop feature enhancement
2. Specify the feature requirements and acceptance criteria
3. Create **Subtasks** for each implementation phase (Wishlist, Reviews)
4. Link subtasks to the parent, maintaining full traceability

---

## Step 1 — Create Parent Jira Task

Use Copilot Chat (`Ctrl+Alt+I`) with this prompt to create the parent task:

```
#createJiraissue in Project: ESHOP 

Enhance the eShop application by adding two features: Wishlist and Product Reviews. Wishlist should allow logged-in users to save/remove products, view a personal wishlist, and move all saved items to the cart, adding cart items to the wishlist, while prompting guests to log in. Product Reviews should allow only users who have purchased a product to submit one review per order (1–5 star rating with an optional comment) in orders section. Display average ratings and review history on the product page, validate review inputs, and prevent duplicate reviews. 

```

**Note the parent Task ID** (e.g., `ESHOP-1`) — you will reference it in subtasks.

---

## Step 2 — Read Jira Task 

Use Copilot Chat to read the parent task and generate a specification:

```
/speckit.specify #getJiraissue <ISSUE-ID>
```

Where `<ISSUE-ID>` is the parent task ID (e.g., `ESHOP-1`).

---

## Step 3 — Create Subtasks for Each Phase

Use `createjiraissue` to create **two subtasks** linked to the parent task:

```
Create two Jira Subtasks in project TEC (issuetype Subtask) with parent <ISSUE-ID>:

"Wishlist — Implement end-to-end Wishlist service + frontend". Description: paste verbatim the entire "Phase 1: Complete Wishlist Feature" section from tasks.md
"Product Reviews — Implement reviews backend + catalog/frontend integration". Description: paste verbatim the entire "Phase 2: Complete Reviews Feature" section from tasks.md .

```

---

## Step 4 — Link Subtasks to Implementation

Once subtasks are created in Jira:
- Reference `ESHOP-1` (parent task) in commit messages
- Update each subtask's description with links to relevant `.specs/` files
- Track completion through Jira workflow states (To Do → In Progress → Done)

---

## Verify

- [ ] Parent Jira task created with clear feature summary
- [ ] Specification generated from parent task
- [ ] Two subtasks created (Wishlist, Reviews) linked to parent



---

## Key Takeaway

> Jira subtasks provide a lightweight, integrated alternative to GitHub issues. Use them when your team is already on Jira and want seamless task tracking from feature definition through implementation.

---



**Related**: [Exercise 01 — Constitution](exercise-01-constitution.md) | [Exercise 03 — Plan: Design the Technical Architecture](exercise-03-plan.md) | [Exercise 04 — Tasks and Sub-Issues](exercise-04-tasks.md)
