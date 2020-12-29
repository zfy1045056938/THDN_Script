using UnityEngine;
using System.Collections;
using Mirror;
using System.Linq;
using System.Text.RegularExpressions;

    [AddComponentMenu("Network/Authenticators/BasicAuthenticator")]
    public class BasicAuthenticator : NetworkAuthenticator
    {
        [Header("Custom Properties")]

        // set these in the inspector
        public string username;
        public string password;

        public DatabaseTHDN database;
        public NetworkManagerTHDN manager;

        public int accountMaxLength = 16;
        public class AuthRequestMessage : MessageBase
        {
            // use whatever credentials make sense for your game
            // for example, you might want to pass the accessToken if using oauth
            public string account;
            public string password;
            public string version;
        }

        public class AuthResponseMessage : MessageBase
        {
            public byte code;
            public string message;
        }
        
        

        public override void OnStartServer()
        {
            // register a handler for the authentication request we expect from client
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnServerLogin,false);
        }

        public override void OnStartClient()
        {
            // register a handler for the authentication response we expect from server
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }

        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }

        void OnServerLogin(NetworkConnection conn, AuthRequestMessage message)
        {
              Debug.Log("Login Module for server");
        // correct version?
        if (message.version == Application.version)
        {   
            Debug.Log("Version"+Application.version);
            // allowed account name?
            if (IsAllowedAccountName(message.account))
            {
                Debug.Log("try login");
                // validate account info
                if (database.TryLogin(message.account, message.password))
                {
                    Debug.Log("log success");
                    // not in lobby and not in world yet?
                    if (!AccountLoggedIn(message.account))
                    {
                        // add to logged in accounts
                        manager.lobby[conn] = message.account;

                        // login successful
                        Debug.Log("login successful: " + message.account);

                        // notify client about successful login. otherwise it
                        // won't accept any further messages.
                        conn.Send(new LoginSuccessMsg());

                        // authenticate on server
                        OnServerAuthenticated.Invoke(conn);
                    }
                    else
                    {
                        //print("account already logged in: " + message.account); <- don't show on live server
                        manager.ServerSendError(conn, "already logged in", true);

                        // note: we should disconnect the client here, but we can't as
                        // long as unity has no "SendAllAndThenDisconnect" function,
                        // because then the error message would never be sent.
                        //conn.Disconnect();
                    }
                }
                else
                {
                    //print("invalid account or password for: " + message.account); <- don't show on live server
                    manager.ServerSendError(conn, "invalid account", true);
                }
            }
            else
            {
                //print("account name not allowed: " + message.account); <- don't show on live server
                manager.ServerSendError(conn, "account name not allowed", true);
            }
        }
        else
        {
            //print("version mismatch: " + message.account + " expected:" + Application.version + " received: " + message.version); <- don't show on live server
            manager.ServerSendError(conn, "outdated version", true);
        }
        }

        private bool AccountLoggedIn(string account)
        {
            return manager.lobby.ContainsValue(account) ||
                   Players.onlinePlayers.Values.Any(p => p.account == account);
        }

        private bool IsAllowedAccountName(string account)
        {
             return account.Length <= accountMaxLength &&
                          Regex.IsMatch(account, @"^[a-zA-Z0-9_]+$");
        }

        public override void OnClientAuthenticate(NetworkConnection conn)
        {
           
            AuthRequestMessage message = new AuthRequestMessage {account = "admin", password = "123", version = "0.1"};
            conn.Send(message);
            print("login message was sent");

            // set state
            manager.state = NetworkState.HandShake;
        }

        public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
        {
            Debug.LogFormat("Authentication Request: {0} {1}", msg.account, msg.password);

            // check the credentials by calling your web server, database table, playfab api, or any method appropriate.
            if (msg.account == username && msg.password == password)
            {
                // create and send msg to client so it knows to proceed
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 100,
                    message = "Success"
                };

                conn.Send(authResponseMessage);

                // Invoke the event to complete a successful authentication
                OnServerAuthenticated.Invoke(conn);
            }
            else
            {
                // create and send msg to client so it knows to disconnect
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 200,
                    message = "Invalid Credentials"
                };

                conn.Send(authResponseMessage);

                // must set NetworkConnection isAuthenticated = false
                conn.isAuthenticated = false;

                // disconnect the client after 1 second so that response message gets delivered
                StartCoroutine(DelayedDisconnect(conn, 1));
            }
        }

        public IEnumerator DelayedDisconnect(NetworkConnection conn, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            conn.Disconnect();
        }

        public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg)
        {
            if (msg.code == 100)
            {
                Debug.LogFormat("Authentication Response: {0}", msg.message);

                // Invoke the event to complete a successful authentication
                OnClientAuthenticated.Invoke(conn);
            }
            else
            {
                Debug.LogErrorFormat("Authentication Response: {0}", msg.message);

                // Set this on the client for local reference
                conn.isAuthenticated = false;

                // disconnect the client
                conn.Disconnect();
            }
        }
    }

