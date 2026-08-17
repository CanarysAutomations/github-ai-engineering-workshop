# Exercise 06 — Product Reviews Implementation with Local Agent

> **Duration**: 10 minutes<br>
> **GitHub/Copilot Feature**: Local Agent Assignment, GitHub Extension Issue, Code Review<br>
> **Goal**: Assign Reviews (with purchase verification and rating aggregation) to the GitHub Copilot local agent and observe domain-focused code.

---

## Background

Reviews require complex business logic: verify user purchased product, aggregate ratings safely, prevent duplicates. By assigning to the **GitHub Copilot local agent**, Copilot prioritizes verification, validation, and concurrency — not just boilerplate.

---
## Step 1 — Create Product Review feature branch

1. Open **Source Control** (Ctrl+Shift+G or left sidebar)
2. Click **Create Branch** and name it `feature/ProductReviews`

## Step 2 — Read GitHub Issue for Product Reviews from GitHub Extension

Go to **Issues** → **Created Issues** → select the sub-issue for Product Reviews as context to the local agent




---

## Step 3 — Assign to GitHub Copilot Local Planning Agent and Assign to GitHub Copilot Local Agent

From Copilot chat, use the GitHub Copilot local planning agent and implement the feature with this main prompt:

```
Create plan to implement this product review feature
```
![GitHub Copilot Local Planning Agent](assets/issuelocalagent.png)
Add this follow-up prompt to steer the agent with the message feature:

```
Implement the feature and update .vscode/launch.json and .vscode/tasks.json so the new service/app can be built and launched from VS Code, while preserving existing configs.
```
![Steer with Message](assets/steerwithmsg.png)
Wait 2–3 min for plan , review and make changes if any

Verify plan emphasizes:
   - Order Service verification call
   - Concurrency handling (atomic ConcurrentDictionary)
   - Integration test for verification

Assign to local agent to implement using Implement Handoff

---

## Step 4 — Review Changes with Copilot
 
1. **Click on Source Control** (Ctrl+Shift+G or left sidebar)
2. **Hover on "Changes"** section
3. **Click on "Code Review Uncommitted Changes"** to review the generated code

![Code Review Uncommitted Changes](assets/uncommittedcodereview.png)
---
## Step 5 - Commit message with Copilot

1. **Click on Source Control** (Ctrl+Shift+G or left sidebar)
2. **Hover on "Changes"** section
3. **Click on "Generate Commit Message"** to generate a commit message

![Generate Commit Message](assets/copilotcommitmsg.png)
---
## Step 6 - Github Pull Request (**Preferably done after exercise 07 to see the code quality issues and fix them before creating PR**)

1. **Click on Github Pull Request** 
2. **Click on "Create Pull Request"** to create a pull request for the changes made by the local agent
3. **Generate with Copilot** to generate a pull request Title and description automatically with copilot
4. **Click on "Create Pull Request"** to create the pull request
![Create Pull Request](assets/copilotpr.png)


## Verify

- [ ] Issue assigned with GitHub Copilot local agent reference
- [ ] Purchase verification implemented
- [ ] Rating aggregation handles concurrency
- [ ] PR created for the local-agent changes

---

## Key Takeaway

> Local agent with business-logic focus generates code emphasizing domain correctness (verification, aggregation) over generic scaffolding.

---

**Next**: [Exercise 07 — Code Quality](exercise-07-code-quality.md)
