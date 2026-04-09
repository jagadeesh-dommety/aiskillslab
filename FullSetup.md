Intially the entire setup will be running on the codespaces primarily like a  vs code
1. Create a app that users can login
    Admin - have a sso login using aad entra auth
    Customers/interviewers - have  a work os login
    Candidate - login with Github
    Seperate logins and there will be seperate 

    How to save user data" — you need to decide this before building admin. Postgres with a tenant-scoped schema is the right answer. Every record tagged with tenant_id. Admin sees all, customer admin sees own tenant only.

    Admin page needs a customer onboarding flow — how does a new customer get set up? WorkOS Admin Portal handles their SSO config. But you need to: create their tenant record, set limits, assign repos. This is the admin "create customer" workflow. It must exist before you can onboard anyone.

    Admin page needs a customer onboarding flow — how does a new customer get set up? WorkOS Admin Portal handles their SSO config. But you need to: create their tenant record, set limits, assign repos. This is the admin "create customer" workflow. It must exist before you can onboard anyone.

Policies need a schema. What does "exclusive repo" mean in the DB? Which roles can change which policies? Define the data model before building the UI.

portal.hirewise.ai/admin/tenantid/config - this is the link and sso login


The session link needs a signed JWT not just a session ID. The URL /session/tenantid/startsession should embed a cryptographically signed token with: session_id, repo_id, candidate_email, expiry (e.g. 48 hours). Without signing, anyone with the URL pattern can attempt to create sessions.

MCP for handbook: the interviewer needs a chat interface where they can ask "what are the planted bugs?" or "what probe question for this response?" — this is a chatbot powered by your MCP server, not just a static PDF. Plan the UI for this early.

2. Admin page for configuration
        Have access to the customer admin 
        configurations - how to save the user data
        Interviews count and limits
        Configure policies like exclusive repos
        Rights over the candidate and interview

portal.hirewise.ai/interviewer/tenantid/interviewsetup 

3. Interviewer Login using workos
        Interviewer can login
        Select the question using configuration like level role senority
        Product gives options with description
        Interviwer will be provided with the session link
        Interviwer will be provided with the handbook of problem and mcp

portal.hirewise.ai/session/tenantid/startsession 

4. User receives a session link from interviewer 
        Get a consent form using github account - no billing to user - only access codespaces - AI insights
        In the same place can ask for user preference language if needed
        Logs in with the github account
        it redirects to github app - ask for codespaces permission
        product stores the token for the codespace creation - and product create a code space which takes the 2 mins to load

        2 minutes to load is too long for candidate UX. With prebuilds configured per repo, cold start drops to 30–60 seconds. If you don't configure prebuilds, candidates wait and wonder if something broke. Prebuild setup is not optional — it is a UX requirement.

        GitHub OAuth state parameter must include the session token. When GitHub redirects back with ?code=xxx, you also need &state=SESSION_TOKEN to know which session this auth belongs to. Without it you cannot link the GitHub token to the right candidate session.

        "Product stores the token" — clarify: stored encrypted in your backend DB, tagged to session_id, deleted after Codespace creation. Not in browser. Not in a cookie. Not in a JWT returned to frontend. Server-side only.

         Every telemetry event must be sent to your backend within seconds of occurring, not buffered for end-of-session flush. This is a critical engineering constraint, not an optimisation.

5. Product has the user token and provide temp access to repo
        based on the language (optionally) loads the devcontainer
        Devcontainer installs - tools, github cli copilot, excalidraw, extension to track navigation in codespace
        Logs and frequent saving data - and store in backend to keep session intact
        Continous tracking and ai insights to interviewer
        Candidate works with interviewer as usual flow - discussing, problem solving - here github cli and copilot provide additional help
        For interviewer handbook and mcp to drive interview

6. Interview completes - 
        user access to repo is removed
        codespaces session removed
        Keep the logs and telemetry and provide insights to interviewer
        Interviewer access to repo and mcp is removed in 1 hour
        based on the admin config, the interview session storage will be permenantly deleted



