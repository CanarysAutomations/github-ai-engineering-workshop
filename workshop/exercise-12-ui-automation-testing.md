# Exercise 12 — Run UI Automation with UIAutomationTester

> **Duration**: 10 minutes
> **Copilot Feature**: Custom Agent Selection, Skill-Driven Testing
> **Goal**: Select the UIAutomationTester agent, run the UI automation flow from skill.md, and capture the test results.

---

## Background

This exercise closes the workshop with a prompt-driven UI automation pass. Instead of writing tests manually, you hand the running app to a custom agent that is scoped for browser validation and follows the shared skill file.

---
## Step 1 - Create Custom Agent for UI Automation Testing

In Copilot Chat, Go to **Chat Settings** → **Agents** → **Generate Drop-down** → **Workspace** → **github** → type **UIAutomationTester** enter. This creates a new agent persona that is scoped for UI automation testing.

![Create Custom Agent](assets/customagent.png)

Select the agent and click on edit to open the agent persona YAML file. Paste the following content into the file and save it.

```
---
name: UIAutomationTester
description: "Run Playwright UI automation tests for the eShop app. Use when: run UI tests, smoke test, checkout flow test, e2e test, test the app, automate browser, validate UI, quickcheckout skill."
argument-hint: "URL to test (default: http://localhost:5173), browsers (default: chromium), scenario name from skill."
tools: [read, edit, 'playwright/*']
---
 
## Inputs — ask the user if not provided
- **URL**: target app URL (default `http://localhost:5173`)
- **Browsers**: comma-separated list (default `chromium`)
- **Scenario**: skill name to run (default `quickcheckout`)
 
## Execution — follow the quickcheckout skill steps exactly
 
 
## Output — append each run as a row to `testartifacts.md`
Format: `| Scenario | Browser | Status | Notes | Timestamp |`
- **Status**: `PASS` if order confirmation shown; `FAIL` otherwise.
- **Notes**: error message or confirmation order id.
- Never store passwords in results.
```

## Step 2 — Select the Custom Agent and Start the Test Flow

In Copilot Chat, switch to the `UIAutomationTester`, then paste this prompt:

```text
read the #skill.md and do ui automation testing
```

> **Tip**: Keep the app running before you start so the agent can evaluate the live UI.

---

## Step 2 — Review the Result Summary

After the agent finishes, it provides a concise test report and any evidence notes.


---

## Verify

- [ ] UIAutomationTester was selected in Copilot Chat
- [ ] The agent followed `.github/skills/ui-automation-testing/SKILL.md`
- [ ] Pass/fail results were returned for the UI flow
- [ ] Any defects include clear reproduction notes

---

## Key Takeaway

> A specialized agent plus a shared skill file turns UI automation into a repeatable, prompt-driven validation step.

---

**Next**: [Workshop Complete](../README.md)