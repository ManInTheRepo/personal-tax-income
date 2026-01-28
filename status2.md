# Personal Tax Income Calculator – Project Roadmap

## Phase 1 – Core Project Delivery
Purpose: Deliver a functional, cleanly implemented personal tax income calculator with modern .NET and Vue stack.

1. **Backend**
   - Implement .NET 8 minimal API with clean separation of concerns.
   - Apply xUnit tests for core calculation logic.
   - Ensure configuration and environment variables are handled securely.

2. **Frontend**
   - Build a lightweight Vue 3 UI for input/output display.
   - Apply minimal but clean styling for clarity.
   - Ensure basic input validation on the client side.

3. **Continuous Integration**
   - Set up GitHub Actions to build and run unit tests on push.
   - Display build status badge in README.

4. **Documentation**
   - Provide a clear `README.md` with:
     - Project overview
     - Stack used
     - How to run locally
     - Screenshot of working app
   - Apply MIT License.

---

## Phase 2 – AI-Era Interview-Ready Enhancements
Purpose: Demonstrate code quality, adaptability, and AI-assisted workflow.

1. **Code Quality Integration**
   - Add SonarQube scan to CI pipeline (GitHub Actions).
   - Track and resolve issues until no blocker or critical issues remain.

2. **Testing Depth**
   - Extend xUnit tests to cover edge cases and failure paths.
   - Add integration tests against a local SQLite/MSSQL instance.
   - Generate a coverage report and reference it in the README.

3. **Refactoring Challenge**
   - Simulate a mid-project requirement change.
   - Document the reasoning, refactor affected code, and confirm all tests pass.

4. **AI Tool Usage Log**
   - Document (in README) which parts of the code were AI-assisted.
   - Note manual tweaks, bug fixes, and decision-making steps.

5. **Interview Simulation Prep**
   - Draft a “project walk-through” script for technical interviews.
   - Justify architectural, design, and tooling choices clearly.
