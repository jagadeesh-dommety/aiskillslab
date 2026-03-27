# HireWise Repo Design — HelloMicrosoft Click Counter
### Interview Problem Specification & Evaluation Guide

---

## Problem Statement

Microsoft is launching a new product called **HelloMicrosoft**. Users log in with their Microsoft Account (MSA), land on a page, and click a "Hello" button.

**Rules:**
- Every click increments the user's click count
- If the resulting count is a **multiple of 1000**, the user receives a reward point
- The system returns a congratulations message at milestone counts (e.g. 2,322 → "You've clicked 2,322 times!", 122,000 → milestone reward)
- The system is **high RPS** — thousands of clicks per second across real users
- The system runs on **5 VMs** — not a single instance

**Core requirement:** The click count increment must be **atomic**. No lost updates. No duplicate rewards.

---

## Why This Problem Is Perfect

It looks deceptively simple. A junior candidate sees a counter. A senior candidate sees a distributed systems problem with five failure modes hiding in plain sight.

The surface area scales cleanly with seniority. The AI will confidently generate the wrong solution at every level — which is exactly what makes this a sharp interview tool. The candidate's job is not to write code. It is to **direct, question, and catch**.

---

## The Naive Starter Repo

The repo the candidate receives has this already implemented — working, clean, passing basic tests:

```
GET current_count FROM database WHERE user_id = X
new_count = current_count + 1
UPDATE database SET count = new_count WHERE user_id = X
IF new_count MOD 1000 == 0:
    grant_reward(user_id)
RETURN new_count
```

This is the classic **read-modify-write** pattern. It works on a single instance with low traffic. It breaks at high RPS across 5 VMs. The repo has:

- A clean service layer with this logic
- A database repository layer (simple get/update)
- Unit tests that all pass (single-threaded, so they pass fine)
- A README that describes the feature but does not mention concurrency
- One TODO comment: `// TODO: consider performance at scale`

That TODO is the only hint. Whether the candidate notices it — and what they do with it — is a signal.

---

## The Three Planted Issues

### Issue 1 — Critical Bug: Race Condition on the Counter

**The problem:**

The read-modify-write is not atomic. At 5 VMs with high RPS:

```
VM1: reads count = 999
VM2: reads count = 999
VM1: writes count = 1000  → triggers reward  ✓
VM2: writes count = 1000  → triggers reward again  ✗
```

Both VMs read 999 before either writes. Both believe they hit the milestone. The reward fires twice — or ten times at higher concurrency.

Worse: two concurrent increments can result in a net movement of +1 instead of +2. Clicks are silently lost.

This is the **lost update problem**. The unit tests pass because they are single-threaded. The bug only surfaces under concurrent load across multiple instances.

**What the fix looks like conceptually:**

The increment and read must happen as a single atomic operation at the database level. The application cannot read, compute, then write — the database must own the increment and return the new value.

- SQL: `UPDATE clicks SET count = count + 1 WHERE user_id = X RETURNING count`
- This is one round trip. The database handles isolation. The returned value is guaranteed to be the value this specific request caused.

**Why AI gets this wrong:**

If the candidate prompts: *"make the counter thread-safe"* — the AI will suggest an application-level lock. A mutex. A `lock` statement. This solves the problem on a single instance and completely fails across 5 VMs. The lock exists only in memory on one VM. The other four have no idea it exists.

A candidate who accepts that output has revealed they don't understand the boundary between in-process and distributed concurrency. A candidate who reads it, recognises the scope error, and redirects the AI with *"the lock needs to be at the database level, not application level — we have 5 instances"* is showing exactly the judgment you're hiring for.

---

### Issue 2 — Subtle Smell: Reward Check Is Not Atomic With the Increment

**The problem:**

Even after fixing the atomic increment, the reward logic is still broken in two ways.

**Way 1 — Crash between increment and reward:**
```
atomic_increment → new_count = 1000  ✓
--- service crashes here ---
grant_reward()  ✗  never fires
```

The count moved. The reward never fired. The user hit the milestone and got nothing. With high RPS across 5 VMs, this window is small but real — and at scale, "small but real" means it happens every day.

**Way 2 — Double reward on concurrent boundary hit:**

Two users are both clicking rapidly. User A increments to 999, User B increments to 1001. But due to timing, both read back a value that passes the mod check. Or — the same user's rapid clicks hit the boundary from two VMs simultaneously.

**What the fix looks like conceptually:**

The reward grant must be tied to **ownership of the specific increment that caused it** — not just observation of the resulting value.

The atomic increment returns the new count. Only the request that received exactly `count % 1000 == 0` as its return value should fire the reward. No other request fires it. This requires the check to happen on the value returned by the atomic operation, not read separately.

Additionally, `grant_reward()` should be **idempotent** — if it is called twice for the same user at the same milestone count, the second call is a no-op. The reward table should have a unique constraint on `(user_id, milestone_count)`.

A candidate who spots this is thinking about **exactly-once delivery** and **idempotent side effects** — a senior-to-staff level signal.

---

### Issue 3 — Underspecified Requirement: What Does "Mod 1000" Actually Mean?

**The ambiguity:**

The spec says "if the click is mod of 1000, the user gets a reward." This statement contains at least four unresolved questions:

1. **Per-user count or global count?** Is it the 1000th click ever made on the platform, or the 1000th click by this specific user?
2. **Who gets the reward at a global milestone?** If User A is at click 999 and User B's click pushes the global count to 1000 — who gets the reward? Both? The one whose click was the 1000th?
3. **Repeating milestones?** If the same user hits 1000, 2000, 3000 — do they get a reward every time, or only the first time?
4. **Is the reward idempotent?** Can grant_reward() be safely called twice for the same user at the same milestone without financial impact?

**What each seniority level does with this:**

- **Junior** — assumes per-user, builds without asking
- **Mid** — asks one clarifying question before building
- **Senior** — enumerates all four edge cases, gets answers, then builds
- **Staff** — points out that questions 2 and 4 have financial and legal implications (reward = real value), refuses to proceed without product sign-off, and suggests the spec needs a formal decision log not just a verbal answer

---

## The Multi-Level Technical Depth Map

This is where the problem truly earns its place in the bank. Every layer of solution reveals a new layer of problem.

---

### Level 1 — Database Atomic Increment

**The fix:** Use a single atomic SQL operation.

```sql
UPDATE user_clicks
SET count = count + 1
WHERE user_id = ?
RETURNING count
```

**What this solves:** Lost updates, duplicate rewards from the read-modify-write.

**What this does not solve:** Performance at high RPS. Every click is a database write. At 10,000 RPS across 5 VMs, the database becomes the bottleneck. Write contention on the same row per user causes lock queuing. At a platform level with millions of users all clicking simultaneously, the database write path saturates.

**The probe question:** "This fixes the race condition. What happens at 50,000 RPS?"

---

### Level 2 — Introduce Redis

**The insight:** Redis `INCR` is atomic by design. It is single-threaded internally, which means it serialises all increment operations without locks. It can handle hundreds of thousands of operations per second. It lives in memory — sub-millisecond response.

**The architecture shift:**
```
Click arrives → Redis INCR user:{id}:clicks → returns new_count
IF new_count MOD 1000 == 0 → queue reward job
RETURN new_count
```

The database is no longer in the hot path. Redis handles all real-time increment and read operations. The database is the persistent record.

**What this solves:** Write throughput. Latency. Database saturation.

**What this introduces:** New failure modes. Redis is in-memory. What happens when it fails?

---

### Level 3 — Redis Failure Handling

This is where senior candidates separate from staff candidates.

**Failure scenario 1 — Redis is temporarily unavailable:**

The service cannot increment the counter. Options:
- **Fail the request** — user sees an error. Safe for data integrity but bad UX. Acceptable for a reward system where correctness matters more than availability.
- **Fall back to database** — increment directly in DB. Maintains availability. Creates a dual-write consistency problem when Redis comes back.
- **Queue the click** — accept the click, put it in a durable queue (e.g. Azure Service Bus), process when Redis recovers. Maintains availability, guarantees eventual consistency, but the real-time count shown to the user is stale.

**The right answer depends on the product requirement** — which the spec doesn't state. A staff candidate flags this and asks: "Is it more important that the click always succeeds, or that the count is always accurate in real time?"

**Failure scenario 2 — Redis loses data (crash without persistence):**

If Redis is configured without persistence (RDB snapshots or AOF), a crash wipes all in-memory counts. The database still has the last persisted count — but how stale is it?

**The fix:** Redis persistence strategy.
- **RDB (snapshot)** — periodic save. Fast, compact. Can lose up to N minutes of data on crash.
- **AOF (append-only file)** — logs every write. Near-zero data loss. Slower, larger.
- **Redis Sentinel / Cluster** — high availability with automatic failover. No single point of failure.

**Failure scenario 3 — Redis and DB are out of sync:**

Redis says user has 1,450 clicks. DB says 1,200. This gap exists normally (Redis is ahead). But after a Redis crash and restart, Redis reads from DB (1,200) and loses 250 clicks.

**The fix:** Periodic sync from Redis to DB — a background job that writes Redis counts to the database on a schedule (every 30 seconds, every minute). The DB is always at most N seconds behind Redis. On Redis restart, the worst-case loss is one sync interval.

The candidate who designs this sync job correctly also needs to think about:
- What if the sync job crashes mid-way?
- What if two sync jobs run simultaneously?
- Should the sync be a full overwrite or a max-value merge?

---

### Level 4 — When to Update the Database

**The sync strategy decisions:**

**Option A — Write-through (every click hits both Redis and DB)**
- Every increment writes to Redis and synchronously writes to DB
- Zero data loss
- DB is back in the hot path — defeats the purpose of Redis

**Option B — Write-behind with scheduled sync (recommended)**
- Redis handles all real-time increments
- Background job syncs Redis counts to DB every N seconds
- DB is eventually consistent, Redis is source of truth
- On Redis failure, max data loss = one sync interval

**Option C — Event-driven sync (most robust)**
- Every Redis increment publishes to a durable message queue
- A consumer reads the queue and batches writes to DB
- DB is eventually consistent, queue guarantees no data loss
- Most complex, most resilient

**The interview probe:** "You've chosen write-behind. The sync job has been running for a week. Redis crashes at 3am. Walk me through exactly what happens, what data is lost, and how the system recovers."

A strong candidate will walk through: sync job last ran at 2:58am, Redis held 2 minutes of increments, those are lost, system restarts from DB values, users lose at most 2 minutes of click history. Then they'll say: "We should alert on this gap and consider if business rules require us to tighten the sync interval or move to event-driven."

---

### Level 5 — Logging, Metrics, and QoS

**The question:** "The product is live. How do you know it's working correctly?"

This level separates engineers who build from engineers who operate.

**What should be logged:**

| Event | Log Level | Why |
|---|---|---|
| Reward granted | INFO + Audit | Financial event — needs audit trail |
| Reward grant failed | ERROR | User hit milestone but got nothing |
| Redis unavailable | WARN → ERROR | Escalate if sustained |
| Fallback to DB triggered | WARN | Unexpected degraded path |
| Sync job completed | INFO | With: rows synced, duration, lag |
| Sync job failed | ERROR | With: last successful sync time |
| Count mismatch detected | ERROR | Redis vs DB divergence beyond threshold |

**What metrics should be emitted:**

| Metric | Type | Alert Threshold |
|---|---|---|
| `clicks.per_second` | Counter | — (informational) |
| `click.latency_ms` | Histogram | p99 > 100ms |
| `redis.increment.success_rate` | Gauge | < 99.9% → alert |
| `redis.fallback.rate` | Counter | Any non-zero → alert |
| `rewards.granted_per_minute` | Counter | Sudden spike → investigate |
| `rewards.grant_failure_rate` | Gauge | Any non-zero → page on-call |
| `sync.lag_seconds` | Gauge | > 120s → alert |
| `sync.rows_processed` | Counter | — (informational) |
| `count.divergence_pct` | Gauge | > 1% → alert |

**QoS definition — what does "working" mean?**

The candidate should be able to define SLOs for this system:

- **Availability:** 99.9% of click requests succeed
- **Correctness:** 0 duplicate rewards granted
- **Latency:** p99 click response < 100ms
- **Data loss:** Max click count loss on Redis failure < 60 seconds of data
- **Reward delivery:** 99.99% of milestone clicks result in reward grant within 5 seconds

Each SLO drives a design decision. "Max loss < 60 seconds" means sync interval must be < 60 seconds. "0 duplicate rewards" means idempotency is not optional.

**The incident probe:** "It's 2pm on a Tuesday. The rewards.granted_per_minute metric spikes to 50x normal. Walk me through your investigation."

Strong answer: check if a milestone boundary coincidence is legitimate (many users hitting 1000 simultaneously — plausible at scale), check for duplicate reward grants per user per milestone (idempotency failure), check for Redis count reset (sync lag caused Redis to restart at a lower value, users are re-crossing milestones), correlate with deployment timeline (did a release just go out?).

---

### Level 6 — System Design Extension

**The prompt for senior/staff candidates:** "HelloMicrosoft is scaling from 5 VMs to 50. The click rate is now 500,000 RPS globally. What changes?"

**Key design questions this surfaces:**

**Sharding the Redis counter:**
A single Redis instance handles ~100k-200k ops/second. At 500k RPS, one instance is not enough. Options:
- Redis Cluster — automatic sharding across nodes, `INCR` still atomic per key
- Shard by user_id hash — each VM routes to the correct Redis shard

**The reward boundary problem at scale:**
With sharded Redis, a user's clicks may be spread across shards if not consistently routed. Consistent hashing by user_id solves this — all clicks for a given user always hit the same shard.

**Global vs per-user milestones revisited:**
At 500k RPS, a global counter is a single write bottleneck no matter what. The system design forces a conversation about whether global milestones are even achievable at this scale, or whether per-user is the only viable model.

**CQRS separation:**
The write path (increment) and read path (display count) have different requirements. The write path must be fast and atomic. The read path can tolerate slight staleness. Separating them — writes go to Redis, reads served from a cache with TTL — reduces load on the increment path.

**The Azure-specific conversation:**
- Azure Cache for Redis — managed, handles failover, persistence, cluster mode
- Azure Service Bus — durable queue for reward event processing
- Azure Monitor + Application Insights — metrics and alerting
- Azure SQL with read replicas — DB tier for persistent storage

A candidate who can map the abstract architecture to concrete Azure services is demonstrating cloud judgment, not just distributed systems theory.

---

## What AI Does At Each Level

| Level | What Candidate Prompts | What AI Returns | What a Good Candidate Does |
|---|---|---|---|
| 1 | "make counter thread-safe" | Application-level mutex/lock | Rejects — explains it only works in-process, re-prompts for DB-level atomic op |
| 2 | "use Redis for the counter" | Redis INCR implementation | Accepts the core, then asks AI about failure modes — probes further |
| 3 | "handle Redis failure" | Try/catch with fallback to DB | Reads it critically — asks: what about data consistency when Redis recovers? |
| 4 | "sync Redis to DB" | Scheduled job with full overwrite | Asks: what if two sync jobs run simultaneously? Is this max or overwrite? |
| 5 | "add logging" | Basic request/response logging | Pushes for structured audit logging, reward-specific events, metric names |
| 6 | "scale to 50 VMs" | Horizontal scaling boilerplate | Asks about Redis cluster sharding, consistent routing, milestone boundary edge cases |

---

## Seniority Decision Guide

**Junior — passes if:**
- Identifies the read-modify-write issue with a prompt or light hint
- Moves the fix to DB level (even if imperfect)
- Runs tests, asks one clarifying question about the spec

**Mid — passes if:**
- Independently identifies the race condition
- Correctly implements DB-level atomic increment
- Notices the reward check gap (Issue 2)
- Asks about the per-user vs global ambiguity

**Senior — passes if:**
- All of mid, plus:
- Proactively raises Redis as the right solution for high RPS
- Designs the sync strategy without prompting
- Defines basic logging and at least 3 meaningful metrics
- Catches at least 2 of 3 planted issues independently

**Staff — passes if:**
- All of senior, plus:
- Identifies the spec ambiguity as a financial/product risk before building
- Designs the full Redis failure recovery path including crash + restart scenario
- Defines SLOs and connects each one to a design decision
- Extends to the 50-VM sharding conversation unprompted
- Reviews AI output critically at every stage and redirects with precision

---

## Repo Metadata

| Field | Value |
|---|---|
| **Domain** | Backend service / distributed systems |
| **Seniority range** | Junior → Staff |
| **Primary language** | C# / .NET (also works in Java, Python, Go) |
| **Planted critical bug** | Race condition — read-modify-write counter |
| **Planted smell** | Non-atomic reward check after increment |
| **Planted ambiguity** | Per-user vs global milestone definition |
| **AI trap** | In-process lock suggested for distributed problem |
| **Extension ceiling** | Redis clustering, event-driven sync, CQRS, Azure architecture |
| **Interview categories covered** | Feature build, debugging, system design, cloud infra, incident simulation, ambiguity/product thinking |

---

## The One-Sentence Summary

> A counter that looks like a counter but is actually a distributed systems exam — with a financial side effect, an AI that confidently generates the wrong answer, and a spec ambiguity that only staff-level candidates flag before they start building.

---

*Part of the HireWise proprietary repo bank. Not to be shared with candidates. Rotate after 200 sessions.*
