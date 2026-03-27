# Hiring for the Job That Actually Exists
### A New Technical Interview Standard for the AI-Native Era

> *"The interview tests for what the job used to be. We need to test for what the job actually is."*

---

## The Idea — 400 Words

The industry is caught in a contradiction. Engineering leaders tell developers to stop writing boilerplate by hand and let AI tools do the heavy lifting — then put candidates in a room with a whiteboard and ask them to reverse a linked list from memory. These two things cannot both be true measures of a good engineer in 2026.

The gap is not about whether DSA matters — it does. A developer who cannot reason about time complexity will write bad AI-assisted code at scale. The problem is that a 45-minute whiteboard session, stripped of all tooling, documentation, and context, no longer tests the right kind of thinking. It tests a simulation of engineering, not engineering itself.

What the job actually rewards is different: knowing how to direct AI effectively, reading an unfamiliar codebase quickly, catching what the model got wrong, and making architectural decisions under real constraints. The skills are almost orthogonal to what traditional interviews measure.

The proposal is a repo-based, AI-assisted interview framework structured around five rounds — each designed to surface a specific dimension of modern engineering judgment. Candidates work inside an existing codebase, use AI tools openly, and are evaluated not on whether they produced working code, but on *how* they got there. Did they read before they prompted? Did they catch the race condition the AI introduced? Did they challenge the underspecified requirement before building to it?

Seniority is expressed through complexity, not category. A junior candidate and a staff engineer both add a feature — but one does it in a clean, well-patterned repo with a clear spec, while the other navigates a system with a hidden performance bottleneck and a requirement that should probably be challenged before building. The task is the same; the signal is completely different.

The evaluation floor is simple and non-negotiable: **the candidate owns every line of code in their final submission, regardless of who wrote it.** If they cannot explain it, it does not count. This single rule ensures that AI assistance never becomes AI substitution — which is exactly the balance the industry needs to maintain as tooling continues to evolve.

This is not a radical idea. It is what the job already looks like. The interview just hasn't caught up yet.

---

## The Core Mismatch

| What Interviews Test | What the Job Rewards |
|---|---|
| Syntax from memory | Directing AI effectively |
| No tooling, no docs | Reading unfamiliar code fast |
| Isolated problem solving | System-level judgment |
| Speed under pressure | Catching what AI misses |
| Algorithm recall | Architectural consistency |

---

## The Five-Round Framework

### 01 · AI-Assisted Feature Build
Candidate is given an existing repo and asked to add a feature using Copilot or Claude CLI. Evaluation focuses on whether they explored the codebase before prompting, whether their prompts were contextual or generic, and whether they enforced existing architectural patterns in the new code.

### 02 · Debugging Round
A broken service with deliberately planted bugs — a race condition, a missing async pattern, a silent API failure. No hints. The candidate must find and fix the issue. Closer to real on-call work than any LeetCode problem.

### 03 · PR Review
An AI-generated pull request with deliberate flaws: a logic error, a concurrency issue, an over-engineered abstraction, a missing test. Does the candidate catch what the AI got wrong? Do they communicate it clearly and constructively?

### 04 · System Design with Constraints
Architecture discussion with real constraints baked in — a performance limit, a legacy dependency, an ambiguous requirement. Tests judgment over perfection. Senior and staff candidates are expected to challenge the brief, not just answer it.

### 05 · Incident Simulation
A production scenario is unfolding. How does the candidate prioritise, communicate, and resolve under real conditions? Tests the skills that matter most and appear least in traditional loops.

---

## Seniority Calibration

| Dimension | Junior | Mid | Senior | Staff |
|---|---|---|---|---|
| Codebase Intelligence | Reads before prompting | Understands patterns | Enforces patterns | Identifies systemic issues |
| AI Direction | Contextual prompts | Iterative refinement | Architectural direction | Uses AI to evaluate tradeoffs |
| Output Skepticism | Runs tests | Catches obvious bugs | Catches planted bug | Catches bug + smell |
| Engineering Judgment | Clarifies ambiguity | Thinks about edge cases | Thinks about callers | Challenges the brief |

---

## Evaluation Standards & SOPs

**The non-negotiable floor:**
> A candidate who cannot explain every line of code in their final submission — regardless of whether AI wrote it — is a no-hire.

**What to plant in every repo:**
- One critical bug (race condition, wrong async pattern, silent failure)
- One subtle smell (naming inconsistency, missing edge case test, over-abstraction)
- One underspecified requirement

**Scoring is per-dimension, not averaged.** A candidate who catches the concurrency issue but misses the logic error is a different profile — and a different hire — than the reverse.

**Review the prompt history.** After the session, the prompts the candidate gave to AI are a separate scoring artifact. The quality of direction is as important as the quality of output.

---

## Who Is Already Moving This Way

| Company | What They're Doing |
|---|---|
| **Meta** | AI-assisted coding round in pilot since October 2025 — 60 min with AI in CoderPad, three scenario types, planted edge cases |
| **Coinbase** | All coding rounds are AI-assisted |
| **Stripe** | Repo-based integration rounds, debugging challenges, real API work |
| **Airbnb** | Debugging challenges over whiteboard problems |
| **Microsoft** | Team-by-team experiments, no company-wide standard yet |

---

## The Bottom Line

The engineers who will build your best products in the next five years are the ones who know how to think *with* AI — not despite it. The hiring process should reflect that.

A 2005 filter cannot reliably identify a 2026 engineer. The framework above is implementable incrementally — one round at a time — within existing hiring infrastructure. It does not require dismantling what works. It requires updating what doesn't.

---

*Framework developed from first-principles analysis of AI-native engineering workflows and evaluation gaps in current industry-standard interview pipelines.*
