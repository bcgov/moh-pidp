# No Git Resets Rule

- **CRITICAL**: Never run `git restore .`, `git reset`, or any commands that modify or reset the working directory tree in bulk.
- If you need to view the history or diff of a file, you may pull back a single file or view it via standard diff commands, but you MUST NOT reset the local working directory.
- Avoid undoing uncommitted changes that the user or another agent might have recently made unless explicitly instructed to do so on a specific file.
