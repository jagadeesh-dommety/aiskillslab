# HireWise — Complete Product & Strategy Document
### From Interview Problem to B2B SaaS Platform

---

## 1. The Core Problem — Industry Contradiction

The industry is caught in a contradiction. Engineering leaders tell developers to stop writing boilerplate by hand and let AI tools do the heavy lifting — then put candidates in a room with a whiteboard and ask them to reverse a linked list from memory.

**What interviews test vs what the job rewards:**

| What Interviews Test | What the Job Rewards |
|---|---|
| Syntax from memory | Directing AI effectively |
| No tooling, no docs | Reading unfamiliar code fast |
| Isolated problem solving | System-level judgment |
| Speed under pressure | Catching what AI misses |
| Algorithm recall | Architectural consistency |

### Why it hasn't changed yet

- **Structural inertia** — Interview pipelines at big companies take years to shift. Calibration rubrics are baked in and interviewers are often individually powerless.
- **Survivorship bias** — The people who designed today's interviews passed yesterday's interviews.
- **AI shift is too recent** — The Copilot/AI-native workflow is only 2–3 years old at scale. Interview redesign lags reality by 5+ years.
- **Fear of gaming** — Companies worry candidates will prompt their way through with no understanding. This is solvable.

---

## 2. The 10 Core Interview Categories

### Execution Layer — Doing Work

**1. AI-Assisted Feature Build**
- Add feature in real repo using tools like GitHub Copilot
- Signal: codebase navigation + pattern adherence

**2. AI-Assisted DSA (Applied, not memory)**
- Solve problem with AI allowed
- Must explain: complexity, tradeoffs, why AI solution works
- Removes memorisation, keeps fundamentals

**3. AI-Assisted Debugging**
- Broken system (race condition, async bug, silent failure)
- Signal: verification over generation

**4. Code Comprehension & Refactor**
- Given messy code — improve structure without breaking behaviour
- Signal: reading > writing

### Judgment Layer — Thinking Work

**5. PR Review (AI-generated)**
- Find: logical bug, over-engineering, missing test
- Signal: critical thinking

**6. System Design (Classic)**
- API, scaling, tradeoffs
- Signal: fundamentals still matter

**7. AI-Augmented System Design**
- Candidate uses AI to explore options
- Must compare designs and reject bad AI suggestions
- New and powerful differentiator

**8. Cloud / Infra Decision Round**
- Cost vs performance vs reliability
- AI can suggest infra (AWS/Azure/GCP) — candidate validates
- Real-world enterprise relevance

### Operational Layer — Real Engineering

**9. Incident Simulation**
- Production issue unfolding
- Prioritisation + communication
- Signal: calm under pressure

**10. Ambiguity & Product Thinking**
- Underspecified requirement
- Candidate must ask questions and challenge assumptions
- Signal: thinking before building

---

## 3. Interview Structure — The Framework

### Core Concept
Repo-based, AI-assisted interview sessions where candidates work inside an existing codebase. Evaluation is based on **how** they get there, not just whether they produced working code.

### The Non-Negotiable Floor
> **If the candidate cannot explain every line of code in their final submission — regardless of whether AI wrote it — it is an automatic no-hire.**

### Session Timeline — 60-Minute Round

| Phase | Time | What Happens |
|---|---|---|
| Explore repo | 0–10 min | Uninterrupted. Interviewer logs which files opened and in what order. |
| Task brief | 10–12 min | Requirement introduced — deliberately under-specified |
| Build with AI | 12–45 min | Candidate works freely with AI tools |
| Review + test | 45–55 min | Candidate reviews, tests, validates output |
| Explain it | 55–60 min | Verbal walkthrough — owns every line |

---

## 4. Seniority Calibration

| Dimension | Junior | Mid | Senior | Staff |
|---|---|---|---|---|
| Codebase Intelligence | Reads before prompting | Understands patterns | Enforces patterns | Identifies systemic issues |
| AI Direction | Contextual prompts | Iterative refinement | Architectural direction | Uses AI to evaluate tradeoffs |
| Output Skepticism | Runs tests | Catches obvious bugs | Catches planted bug | Catches bug + smell |
| Engineering Judgment | Clarifies ambiguity | Thinks about edge cases | Thinks about callers | Challenges the brief |

### Seniority via Complexity, Not Category
A junior and a staff engineer both add a feature — but:
- **Junior repo:** clean, well-patterned, clear spec
- **Staff repo:** hidden performance bottleneck, underspecified requirement that should be challenged before building

---

## 5. Evaluation Standards & SOPs

### The Four Scoring Dimensions

**1. Codebase Intelligence** — Can they read a system before they touch it?

| Level | Observable Behaviour |
|---|---|
| Below bar | Starts prompting AI immediately without exploring the repo |
| At bar | Spends time understanding structure, then prompts with context |
| Above bar | Identifies patterns, naming conventions, existing abstractions — enforces them |
| Exceptional | Notices something suspicious before the task begins |

*SOP: Interviewer timestamps when candidate first touches AI vs first reads existing code.*

**2. AI Direction Quality** — Engineer using AI, or typist delegating to AI?

| Level | Observable Behaviour |
|---|---|
| Below bar | Generic prompts — "write a function that does X" |
| At bar | Contextual prompts — "in this codebase, X pattern is used, add Y consistently" |
| Above bar | Iterative direction — challenges AI output, refines, asks for alternatives |
| Exceptional | Uses AI to think, not just generate |

*SOP: Prompt history reviewed as a separate scoring artifact after the session.*

**3. Output Skepticism & Quality Gate**

| Level | Observable Behaviour |
|---|---|
| Below bar | Accepts AI output, runs it, submits if it compiles |
| At bar | Reads AI output, runs tests, catches obvious issues |
| Above bar | Tests edge cases AI didn't cover, questions correctness not just compilation |
| Exceptional | Catches the planted bug without being prompted |

**Quality gates to plant by language/domain:**

```
C# / .NET     → async void, missing ConfigureAwait,
                ConcurrentDictionary misuse, lock on wrong object

Java          → HashMap in concurrent context,
                checked exceptions swallowed, Optional misuse

Python        → GIL assumptions in threading,
                mutable default arguments, bare except clauses

Distributed   → Missing idempotency, no retry budget,
                silent failure on 4xx, no timeout
```

**4. Engineering Judgment** — Do they think beyond the ticket?

| Level | Observable Behaviour |
|---|---|
| Below bar | Builds exactly what was asked |
| At bar | Considers edge cases, asks one clarifying question |
| Above bar | Thinks about who uses this, how could it fail, what happens under load |
| Exceptional | Challenges the requirement before building |

### PR Review Round Standards

**What to plant in every PR:**

| Category | Example |
|---|---|
| Logic bug | Off-by-one in pagination, wrong operator |
| Concurrency issue | Shared state without synchronisation |
| AI fingerprint | Hallucinated method that doesn't exist in codebase |
| Style inconsistency | Naming that breaks existing conventions |
| Missing test | Happy path only, no failure/edge case |
| Security smell | Logging sensitive data, no input validation |
| Over-engineering | Abstraction that adds complexity with no benefit |

### Interviewer SOPs

**Before panel goes live:**
- Every interviewer attempts the same repo task themselves
- Panel agrees: "catching the planted concurrency bug = above bar for mid, at bar for senior"
- Shadow interviews for first 3 rounds

**During interview:**
- Do not help unless candidate is completely stuck for more than 5 minutes
- "Walk me through what you're thinking" is a probe, not a hint
- All notes are timestamped and behavioural — not "seemed confident"

**After interview:**
- Each dimension scored independently before discussion
- Prompt history reviewed as separate artifact
- Disagreements above 2 points require a third interviewer

---

## 6. The Co-Interviewer Product Role

### What the Co-Interviewer Provides

**Environment Layer (what the candidate works inside):**
- One repo from the bank, pre-selected by role and seniority
- AI model (Copilot or Claude CLI) — same as real job, no restrictions
- Azure/AWS CLI available for cloud decision rounds
- Terminal, test runner, linter — no internet search

**Observation Layer (what it watches and records):**
- Every prompt sent to AI captured with timestamp
- Navigation path — which files opened, in what order
- Time markers — first file open, first prompt, first test run
- Verbal probe triggers for interviewer

**Evaluation Layer (what it scores — not hire/no hire):**
- Prompt quality (generic vs contextual)
- Output skepticism (read, test, challenge vs accept and submit)
- Judgment signals (noticed planted bug? flagged ambiguity?)
- Communication clarity

### What the Co-Interviewer Does NOT Do
- Make hire / no-hire decision
- Hint at the planted bug
- Judge coding style aesthetics
- Intervene when candidate is stuck
- Compare to other candidates live

### What It Does Do
- Score each dimension independently (1–4)
- Log timestamps and prompt history
- Ask "walk me through this" at key moments
- Note what candidate notices unprompted
- Feed structured behavioural data to hiring panel

---

## 7. Who Is Already Moving This Way

| Company | What They're Doing |
|---|---|
| **Meta** | AI-assisted coding round in pilot since October 2025 — 60 min with AI in CoderPad, three scenario types, planted edge cases that AI will miss |
| **Coinbase** | All coding rounds are AI-assisted |
| **Stripe** | Repo-based integration rounds, debugging challenges, real API work — "Integration Round" tests reading docs and handling errors, not inventing |
| **Airbnb** | Debugging challenges over whiteboard problems |
| **Microsoft** | Team-by-team experiments, no company-wide standard yet |

**Industry stat:** 81% of interviewers at Big Tech have suspected candidates of using AI to cheat. Meta's response is to legalise it under controlled conditions — brilliant strategic move.

---

## 8. The Product — HireWise

### What It Is
A B2B SaaS platform that replaces traditional coding rounds with repo-based, AI-assisted interview sessions. Sold to enterprise hiring teams, staffing partners, and RPO providers.

### Business Partner Tiers

**Starter — SMB / Scaleup**
- Per-assessment pricing
- Curated public repo bank
- 5 interview categories
- Standard score report
- ATS webhook export

**Enterprise (Most Popular)**
- Annual seat licence
- Private repo ingestion
- Custom rubric builder
- Calibration dashboard
- SSO + compliance (SOC2)

**Partner — Staffing / RPO**
- Volume + revenue share
- White-label option
- Multi-client dashboard
- Candidate portal
- API-first integration

### How It Flows for a Partner
1. **Onboard repos** — Connect GitHub org or pick from curated bank. Repos tagged by domain and seniority.
2. **Configure round** — Select 1 of 10 categories. Set seniority level. Platform auto-selects repo and plants issues.
3. **Candidate session** — Candidate works in sandboxed environment. AI tools available. Prompt log and navigation path captured live.
4. **Score report** — 5-dimension behavioural report delivered. Hiring panel calibrates. No black-box hire/no-hire.

### Who Buys This

**Enterprise (high ACV, long sales cycle)**
- Microsoft, Google, Meta
- Large banks and fintechs
- Enterprise SaaS companies
- Big 4 tech consultancies

**Mid-market (fast close, high volume)**
- Series B–D startups
- Dev-first product companies
- Engineering agencies

**Channel / Resell (volume multiplier)**
- Staffing firms (white-label)
- RPO providers
- HRIS platforms (embedded)
- Job boards

### Defensible Moats

1. **Repo bank network effect** — Every partner who contributes enriches the benchmark. More repos = better signal calibration = harder to replicate.
2. **Behavioural benchmark data** — Prompt history and navigation patterns across thousands of sessions becomes a proprietary dataset no competitor can buy.
3. **Rubric calibration stickiness** — Once a company has trained interviewers and aligned on scoring, switching costs are high. The rubric becomes internal IP.
4. **ATS integration depth** — Deep Greenhouse/Lever/Workday hooks. Score reports flow into hiring pipelines. Becomes load-bearing infrastructure.

### Risks to Address Early

| Risk | Level | Mitigation |
|---|---|---|
| Bias and fairness liability | High | Third-party audit, transparent rubrics, EEOC/EU AI Act compliance from day 1 |
| Repo IP and confidentiality | High | Watertight data isolation, no cross-contamination between client sandboxes |
| Incumbent inertia (HackerRank, Codility) | Medium | Wedge at team level — frustrated hiring managers, not procurement |
| Candidate gaming | Low | AI is allowed, so gaming surface shrinks; direction and judgment are hard to fake over 60 min |

---

## 9. The Repo Bank — The Core Moat

### Why Product-Owned Repos Is the Only Viable Strategy

**Option A — Public OSS repos:** Not viable. Candidates can prep against them. No control over quality. No planted issues. Zero IP ownership.

**Option B — Enterprise partner repos:** Later, not first. IP negotiation overhead. Partner has control, not you. Hard to launch with.

**Option C — Product-owned repos (chosen):** Full IP ownership. Bugs placed precisely. Versioned per seniority. Candidate can't Google answers. Compounds into a real moat.

### The Fundamental Problem With Public Repos
The moment you use any publicly known codebase, you've created a preparation surface. Candidates will find the repo, read it, know where the interesting parts are. The signal collapses. The interview becomes a test of whether they found the right GitHub repo before the session — not whether they can navigate an unfamiliar codebase.

### Anatomy of One Well-Crafted Repo

**Structure:**
- Realistic service — not toy code
- 3–5 files, clear entry point
- Existing tests (some passing, some not)
- README slightly outdated (intentional)
- One TODO comment, unresolved (intentional)

**Planted Issues — always exactly 3:**
- 1× critical bug (e.g. race condition, wrong async pattern)
- 1× subtle smell (e.g. wrong abstraction, inconsistent naming)
- 1× underspecified requirement (e.g. ambiguous spec)

**Domain Tags:**
REST API · background jobs · auth service · data pipeline · cache layer · event bus · infra/IaC · frontend component

**Seniority Tags:**
junior · mid · senior · staff

### Build Roadmap — Path to 100

**Phase 1 — MVP (10 repos)**
- 2 per seniority level
- Cover 3 most common domains: REST API, background job, auth
- Hand-crafted by senior engineers on the product team
- Goal: prove the signal works

**Phase 2 — Scale (40 repos)**
- Add infra, data pipeline, frontend, cloud decision scenarios
- Hire 1–2 dedicated repo engineers
- First enterprise partner pilots contribute sanitised patterns
- Goal: domain coverage

**Phase 3 — Moat (100+ repos)**
- Language variants: .NET, Java, Python, Go
- Industry verticals: fintech, healthtech, SaaS
- Rotation schedule — repos retired and replaced to prevent leakage
- Goal: benchmark depth no competitor can replicate

### Why This Is the Moat — Compounding Over Time

**1. Candidate-proof by design (valuable from day 1)**
No one can Google your repos. No YouTube walkthrough. No LeetCode discussion thread. The signal stays clean indefinitely.

**2. Planted issue catalogue as IP (compounds after year 2)**
Every bug, smell, and ambiguity is documented with expected catch rates by seniority. Over thousands of sessions this becomes calibration data nobody else will have — e.g. "race conditions are caught by 90% of staff engineers and 30% of mid-levels."

**3. Partner contribution flywheel (strategic asset by year 3–4)**
Enterprise partners contribute sanitised patterns. They get custom domain scenarios; you own the asset entirely. A fintech partner's payment service pattern becomes a financial domain repo nobody else has.

**4. Rotation keeps signal fresh (operational discipline from year 1)**
Repos are versioned and retired on a schedule. Even repeat candidates never see the same repo twice. This operational discipline is itself hard to copy.

### The Question to Answer Next
Who on the team builds these repos, and what is the quality bar and review process for a repo to enter the bank?

---

## 10. The One-Page Pitch

> The industry is caught in a contradiction. Engineering leaders tell developers to stop writing boilerplate by hand — then put candidates in a room with a whiteboard and ask them to reverse a linked list from memory. These two things cannot both be true measures of a good engineer in 2026.

The engineers who will build your best products in the next five years are the ones who know how to think *with* AI — not despite it. A 2005 filter cannot reliably identify a 2026 engineer.

**HireWise** is an AI-native interview platform that tests what the job actually requires: directing AI effectively, reading unfamiliar codebases, catching what the model gets wrong, and making architectural decisions under real constraints.

The evaluation floor is simple and non-negotiable: the candidate owns every line of code in their final submission, regardless of who wrote it. If they cannot explain it, it does not count.

---

*Document compiled from full product and strategy discussion. Framework developed from first-principles analysis of AI-native engineering workflows and evaluation gaps in current industry-standard interview pipelines.*
