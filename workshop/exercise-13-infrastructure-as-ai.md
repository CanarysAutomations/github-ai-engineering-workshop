# Exercise 13 — Infrastructure as Code: Generate Terraform Scripts with Copilot

> **Duration**: 8 minutes<br>
> **GitHub/Copilot Feature**: Copilot Chat, Terraform Generation<br>
> **Goal**: Use one prompt to generate complete Terraform scripts for deploying the eShop microservices app on Azure.

---

## Background

With features implemented and tested, use Copilot Chat to generate production-ready Terraform configuration for the complete eShop microservices app. One prompt, full IaC.

---

## Step 1 — Open Copilot Chat

1. Open **Copilot Chat** in VS Code (`Ctrl+Shift+I`)
2. Keep default agent selected

---

## Step 2 — Paste Single Prompt to Generate Terraform

Copy and paste this prompt into Copilot Chat:

```
Generate Terraform for MyEcomm eShop on Azure:
- 5x .NET 8 Container Instances (Catalog, Cart, Order, Identity APIs)
- 1x YARP Gateway Container Instance
- 1x Static Web App for React frontend
- Virtual Network, Managed Identity, Application Insights
- main.tf, variables.tf, outputs.tf, terraform.tfvars
- Environment variables for inter-service URLs
```

---

## Step 3 — Review Generated Terraform Files

Agent generates:
- **`infra/main.tf`** — Main infrastructure orchestration
- **`infra/variables.tf`** — Input variables (location, environment, image tags)
- **`infra/outputs.tf`** — Output values (gateway URL, service endpoints)
- **`infra/terraform.tfvars`** — Dev environment values

---

## Step 4 — Deploy (Optional)

If you want to deploy:

```bash
cd infra
terraform init
terraform plan
terraform apply
```

---

## Verify

- [ ] Terraform files generated in `/infra` directory
- [ ] No syntax errors in main.tf
- [ ] All 5 backend services defined as Container Instances
- [ ] Gateway and frontend included
- [ ] Variables and outputs present

---

## Key Takeaway

> AI-generated Terraform is infrastructure-as-code ready for version control and CI/CD deployment pipelines.

---

**Congratulations!** Full spec-to-production workflow complete: Constitution → Specification → Planning → Implementation → Code Quality → Testing → Infrastructure Deployment.

