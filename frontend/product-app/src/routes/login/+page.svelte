<script>
	
</script>

<svelte:head>
	<title>Login</title>
	<meta name="description" content="Login with username and password" />
</svelte:head>

<section>
	<h1>Login</h1>

    <div>
        <div>
            <input id="username" placeholder="Username" />
        </div>
        <div>
            <input id="password" placeholder="Password" />
        </div>
        <button id="submit">Login</button>
    </div>

    <p>No user? Register <a href="/register">here</a></p>
</section>