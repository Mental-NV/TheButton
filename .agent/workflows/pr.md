---
description: Automates the branch creation, commit, and PR submission process
---

// turbo-all
1. Create a new branch: `git checkout -b <branch_name>` (e.g. `pr/M{n}-...` or `feature/...`)
2. Stage and commit changes: `git add -A; git commit -m "<commit_message>"` (e.g. `M{n}: <area> - <action>` or `chore: ...`)
3. Create `.pr-body.md` using the standard template
4. Push the branch: `git push -u origin HEAD`
5. Create a Pull Request: `gh pr create --title "<commit_message>" --body-file .pr-body.md`
6. Remove .pr-body.md