# Personal Tax Income — Status & Plan

**Repo:** https://github.com/ManInTheRepo/personal-tax-income  
**Latest baseline commit:** 4c490e0 (initial code: brackets/service/tests/API)

## ✅ Done
- Repo scaffold (.NET 8 solution; Domain, API, Tests)
- Core tax classes present (TaxBracket, calculator service)
- Minimal API project present
- README added

## ⏭️ Next 24–48h (resume-ready minimum)
- [ ] Ensure `/calculate` returns AU FY24–25 tax for sample inputs
- [ ] Add/verify **boundary tests** for 0, 18,200, 45,000, 135,000, 190,000, 200,000
- [ ] Add **GitHub Actions CI** (build + test on push)
- [ ] Confirm **README run steps** match the actual endpoint (POST/GET & payload)

### CI template (drop in `.github/workflows/ci.yml`)
- Build + test with .NET 8 (ubuntu-latest)

## 🎯 Nice-to-have (polish)
- [ ] Vue 3 minimal UI (single page: income input → results card)
- [ ] Screenshot(s) in README
- [ ] Simple tax bracket table in README
- [ ] Tag `v1.0.0` after UI + CI are green

## 📦 Backlog (later)
- [ ] Medicare levy toggle
- [ ] Multi-year schedules (AU), simple selector
- [ ] Tests-only US schedule for engine parity (kept out of API/UI)

## Working Mode
- Keep this file as the source of truth.  
- Open **Issues** for each checkbox; small PRs only; we’ll review on diffs.

## Second phase
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
