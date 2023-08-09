<script>
	
</script>

<svelte:head>
	<title>Register</title>
	<meta name="description" content="Create a new user account" />
</svelte:head>

<section>
	<h1>Create a new user account</h1>

    <div>
        <div>
            <input id="username" placeholder="Username" />
        </div>
        <div>
            <input id="password" placeholder="Password" />
        </div>
        <div>
            <input id="confirm-password" placeholder="Confirm password" />
        </div>
        <button id="submit">Register</button>
    </div>

    <p>Already a user? Login <a href="/login">here</a></p>
</section>