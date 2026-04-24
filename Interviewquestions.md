Category 1 — System Design Problems (Like Hello Microsoft)
Real-world services with intentional bugs and design flaws. Rich for senior/staff interviews.

Problem	Core Issue to Find
Global Click Counter	Race condition, hot partition, Redis vs DB
URL Shortener	Hash collision, redirect latency, cache invalidation
Rate Limiter	Token bucket vs sliding window, distributed state
Leaderboard Service	Sorted set design, stale reads, cache vs DB
Notification Service	Fan-out design, at-least-once vs exactly-once delivery
Session Manager	Token expiry, concurrent login, race on session write
Job Queue / Task Scheduler	Duplicate execution, poison message, retry storm
File Upload Service	Chunked upload, idempotency, partial failure recovery
Search Autocomplete	Trie vs prefix index, latency vs freshness tradeoff
Distributed Config Service	Cache staleness, hot reload without downtime
Voting / Poll System	Double vote prevention, real-time count accuracy
Inventory / Stock Counter	Oversell problem, optimistic lock, reservation pattern

Category 2 — DSA Implementation Problems
Candidate implements a known data structure. Bugs are planted in the implementation. Tests verify correctness and performance.

Problem	What Is Broken
Design HashMap	Hash collision handling broken, load factor ignored
LRU Cache	Eviction order wrong under concurrent access
LFU Cache	Frequency tie-breaking incorrect
Min Stack	getMin() returns wrong value after pop sequence
Circular Buffer / Ring Queue	Off-by-one on wrap-around, overwrite not handled
Trie (Prefix Tree)	Delete operation corrupts shared prefixes
Graph — Shortest Path	Dijkstra with negative edge weights silently wrong
Binary Search Tree	Deletion of node with two children breaks tree
Bloom Filter	False negative possible due to wrong hash count
Thread-Safe Queue	Producer-consumer with lost signals under load
Consistent Hashing	Node removal redistributes more than necessary
Skip List	Level promotion probability wrong, search degrades

Category 3 — Concurrency and Threading Problems
Focused purely on parallel execution bugs. Very strong for mid to senior level.

Problem	Core Bug
Bank Account Transfer	Deadlock when two accounts transfer simultaneously
Singleton Pattern	Double-checked locking broken without volatile
Connection Pool	Pool exhaustion under burst, no timeout handling
Producer-Consumer Pipeline	Buffer overflow, missed notify, spurious wakeup
Parallel File Processor	Shared mutable state across worker threads
Cache Stampede	Thundering herd on cache miss, no lock-or-compute
Background Job Runner	Jobs run twice on restart, no idempotency guard

Category 4 — API and Code Quality Problems
Closer to day-to-day engineering. Good for junior and mid-level interviews.

Problem	Issues Planted
REST API for Todo App	N+1 query, no pagination, no input validation
Auth Middleware	JWT not validated, expiry not checked, userId trusted from body
CSV Report Generator	Memory exhaustion on large files, no streaming
Retry Client	Exponential backoff missing, no jitter, retries non-idempotent calls
Config Loader	Secrets in code, no environment override, no reload
Webhook Dispatcher	No signature verification, no timeout, synchronous fan-out

For oauth - state or any additional data been sent.  first it call the oauth and oauth returns with /callback which goes to any of the multi instance system, how to handle it 
