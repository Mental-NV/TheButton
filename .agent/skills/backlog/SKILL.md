---
name: backlog
description: Automates the process of updating task and epic statuses in roadmap.md
---

# Backlog Skill

This skill automates the maintenance of `/docs/roadmap.md` by transitioning item statuses based on the work lifecycle.

## Status Mapping
- **InProgress**: Set when work starts on a task or epic. Format: `**[InProgress]**`
- **InReview**: Set when a PR is opened. Format: `**[InReview]**`
- **Done**: Set when a PR is merged or the task is finished. Format: `**[Done]**`
- **New**: The default state for unstarted tasks. Format: `**[New]**`

## Rules
1. **Consistency**: Ensure the brackets and bolding are preserved exactly: `**[Status]**`.
2. **Task-to-Epic Propagation**:
   - If any child task in an epic becomes `InProgress`, the parent epic must also become `InProgress`.
   - If all child tasks in an epic become `Done`, the parent epic must become `Done`.
3. **Regex Search**: Use precise matching to avoid changing similar text. Patterns usually follow the format: `[Task Link](path/to/task.md) **[Status]**`.
4. **Scope Constraint**: This skill and its status propagation rules MUST ONLY be enforced if the current task is explicitly tracked in [Roadmap](/docs/roadmap.md).

## Procedure
1. Locate the entry for the task/epic in `/docs/roadmap.md`.
2. Replace the current status string (e.g., `**[New]**`) with the target status.
3. If the item is a task, check its sibling tasks under the same Epic.
4. Update the parent Epic status if the propagation rules are met.
