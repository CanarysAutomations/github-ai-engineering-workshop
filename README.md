# GitHub Copilot Workshop — Wishlist + Reviews Feature

> **Audience**: Intermediate .NET developers with basic GitHub and microservices knowledge <br>
> **Total Duration**: ~3 Hours <br>
> **Pre-requisites**: VS Code with GitHub Copilot Chat extension, .NET 8 SDK, Git CLI, GitHub Copilot access, Spec Kit CLI, GitHub MCP Server

---

## What You Will Build

In this workshop, you'll use **Spec-Driven Development** with GitHub Copilot to design, specify, and implement two complementary eShop features: **Wishlist** (save products for later) and **Product Reviews** (rate and review purchases). Rather than writing code directly, you'll guide Copilot through structured specification → planning → task breakdown → agent assignment → validation.

By the end, you'll have:
- **Constitution** — eShop's microservices principles (Minimal APIs, in-memory storage, JWT auth, testing standards)
- **Specification** — detailed requirements with user goals and 13 acceptance criteria (Wishlist & Reviews)
- **Technical Plan** — API contracts, data models, service boundaries, and integration design
- **Implementation & Validation** — both features coded by agents, CI pipelines validated, security scanning enabled, and merged to main



---

## Prerequisites

- **[VS Code](https://code.visualstudio.com)**
- **[GitHub Copilot Chat](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot-chat)**
- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
- **[Node.js 18+](https://nodejs.org)**
- **npm 8+** (Bundled with Node.js)
- **[Git CLI](https://git-scm.com)**
- **[UV](https://astral.sh/uv)**
- **[Spec Kit](https://github.com/github/spec-kit)**
- **[GitHub Extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-pull-request-github)**
- **GitHub MCP Server** Enabled
- **Playwright MCP Server** Enabled
- **GitHub Advanced Security** Enabled 
- **GitHub Code Quality** Enabled

    

---




### Exercises

| # | Exercise | Copilot Feature | Duration |
|---|----------|----------------|----------|
| 00 | [Prerequisites](workshop/exercise-00-prerequisites.md) | Configuration Setup | 8 min |
| 01 | [Constitution: Define eShop Microservices Principles](workshop/exercise-01-constitution.md) | Issues , Spec Kit Constitution | 8 min |
| 02 | [Specify: Define Wishlist + Reviews Requirements](workshop/exercise-02-specify.md) | Spec Kit Specify, GitHub MCP | 10 min |
| 03 | [Plan: Design the Technical Architecture](workshop/exercise-03-plan.md) | Spec Kit Plan | 8 min |
| 04 | [Tasks: Break into Implementation Work Items](workshop/exercise-04-tasks.md) | Spec Kit Tasks & Checklist | 8 min |
| 05 | [Wishlist Feature with Coding agent](workshop/exercise-05-wishlist-coding-agent.md) | Copilot Coding Agent | 8 min |
| 06 | [Product Review Feature with Local Agent](workshop/exercise-06-reviews-local-agent.md) | Copilot Local Agent | 10 min |
| 07 | [Code Quality](workshop/exercise-07-code-quality.md) | Copilot Code Quality | 10 min |
| 08 | [Coverage Report Artifact](workshop/exercise-08-coverage-report-artifact.md) | GitHub Actions, CI Pipeline Review | 6 min |
| 09 | [GHAS Code Scanning](workshop/exercise-09-ghas-code-scanning.md) | GitHub Advanced Security, Code Scanning | 6 min |
| 10 | [GHAS Dependency Scanning](workshop/exercise-10-ghas-dependency-scanning.md) | GitHub Advanced Security, Dependency Scanning | 8 min |
| 11 | [GHAS Secret Scanning](workshop/exercise-11-ghas-secret-scanning.md) | GitHub Advanced Security, Secret Scanning | 8 min |
| 12 | [UI Automation Testing](workshop/exercise-12-ui-automation-testing.md) | Copilot UI Automation | 8 min |
| 13 | [Infrastructure as Code: Generate Terraform](workshop/exercise-13-infrastructure-as-ai.md) | Copilot Chat, Terraform | 8 min |




---

## Key GitHub Copilot Features Covered

| Feature | Description |
|---------|-------------|
| **GitHub Issues** | Create parent issue for full feature traceability |
| **GitHub MCP** | `#issue_read` for reading issue context; `sub_issue_write` for creating sub-issues from tasks |
| **Spec Kit Constitution** | Define architectural principles once; all future features validate against them |
| **Spec Kit Specify** | Transform requirements into structured specifications with acceptance criteria |
| **Spec Kit Plan** | Generate technical designs (API contracts, data models, integration) from specifications |
| **Spec Kit Tasks** | Break plans into ordered, dependency-aware implementation tasks with test matrices |
| **GitHub Advanced Security** | Enable code scanning, dependency scanning, and secret scanning; then fix findings with Copilot |
| **Agent Customization** | Create workspace-level `.github/agents/` for custom personas  |
| **Agent Skill** | Create a specific skill to perform the testing with custom agent |

---

## Getting Started

1. **Clone or open this repository** in VS Code 
2. **Start with Exercise 00**: Open [Exercise 00 — Prerequisites](workshop/exercise-00-prerequisites.md)


---

## Key Learnings

By completing this workshop, you will understand:

1. **Shift-Left Specification**: Why detailed specification *before* coding prevents rework
2. **Constitution-Driven Design**: How to encode team principles once and validate all future work against them
3. **GitHub MCP Integration**: How to use GitHub's MCP server for automated issue/sub-issue workflows
4. **Custom Agents**: How to create workspace-level agents with YAML for specialized domains (e.g., complex business logic)
5. **Agent Strategy**: When to use default agent (CRUD) vs. custom agent (complex domain logic)
6. **Governance Gates**: Why consistency validation *before* implementation is a high-ROI investment
7. **AI-Assisted Partnership**: How to maintain human approval gates while leveraging AI code generation

---





