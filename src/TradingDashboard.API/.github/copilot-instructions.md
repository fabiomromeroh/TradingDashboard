## Response and token-efficiency rules

- Be concise. Explain only the relevant change and any important trade-offs.
- Before writing code, inspect the existing solution structure and identify reusable classes, interfaces, validators, handlers, extensions, and tests.
- Do not generate complete files unless explicitly requested.
- Prefer a focused patch or only the changed method/class.
- Do not repeat code that already exists in the repository.
- Do not create new abstractions when an existing abstraction can be reused.
- Do not add libraries, frameworks, projects, or design patterns unless they are required and justified.
- When requirements are ambiguous, ask one concise clarification question instead of guessing.
- For multi-step work, implement only the first requested step unless asked to continue.
- Do not add comments that merely restate the code.
- Do not generate documentation, tests, migrations, or frontend changes unless they are relevant to the requested task.
- At the end, provide a short list of changed files and any commands that must be run.

## Required implementation workflow

For every coding request:

1. Inspect the relevant project files and existing patterns.
2. State the smallest implementation approach in 2–4 bullets.
3. Identify the files that need changing.
4. Implement only the requested behavior.
5. Reuse existing abstractions and conventions.
6. Add or update focused tests if behavior changed.
7. Check for compile errors, validation issues, and missing migrations.
8. Report changed files, tests run, and any follow-up action briefly.