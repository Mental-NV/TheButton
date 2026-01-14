---
description: Automates the branch creation, commit, and PR submission process
---
// turbo-all
1. Create a new branch: `git checkout -b <branch_name>`
2. Stage and commit changes: `git add -A; git commit -m "<commit_message>"`
3. Create `.pr-body.md` using the standard template
4. Push the branch: `git push -u origin HEAD`
5. Create a Pull Request: `gh pr create --title "<commit_message>" --body-file .pr-body.md`
