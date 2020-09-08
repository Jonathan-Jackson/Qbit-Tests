# Qbit-Tests
Basic tests to QBittorrent (C#) API to use as a basis for implementation and testing commands. (V4.5)

# Things to Note (for the API)
1. You need to use the Referer header when authenticating.
2. Failed logins will result in being IP banned. It's worth disabling this while testing, or you'll be wondering why it's not authenticating.
3. Versions of QBittorrent have changed the API dramatically.
4. Use the Web UI as a reference, if you read the AJAX calls send via chromes console you can convert the request into CURL and use something like https://curl.olsh.me/ to get a good basis for what needs doing.
