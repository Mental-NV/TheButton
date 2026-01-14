---
name: pr
description: Automates the branch creation, commit, and PR submission process
---

# PR Skill

This skill automates the process of submitting a Pull Request.

## Steps
1. Create a new branch: `git checkout -b <branch_name>` (Use `pr/M{n}-...` for roadmap or `feature/...` / `fix/...` for generic)
2. Stage and commit changes: `git add -A; git commit -m "<commit_message>"` (Use `M{n}: <area> - <action>` for roadmap or `type: description` for generic)
3. Create `.pr-body.md` using the template in `./resources/pr-body-template.md`
4. Push the branch to origin: `git push -u origin HEAD`
5. Create the PR using GitHub CLI: `gh pr create --title "<commit_message>" --body-file .pr-body.md`
