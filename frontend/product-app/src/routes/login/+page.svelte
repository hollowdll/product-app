<script lang="ts">
	import { PRODUCT_API_URL } from "../../utils/const";
    import type { LoginForm } from "../../types/login";
    import { goto } from "$app/navigation";

    let loginForm: LoginForm = {
        username: "",
        password: "",
    }
    let errMessage = "";

    // Submits login form to the backend
    let submitLogin = () => {
        fetch(`${PRODUCT_API_URL}/account/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginForm)
        })
        .then(response => {
            if (response.ok) {
                response
                    .json()
                    .then(data => {
                        // Use local storage for simple use. Store access token more securely in production
                        localStorage.setItem("jwtAccessToken", data.token);
                        goto("/products");
                    })
            } else {
                response
                    .text()
                    .then(msg => {
                        errMessage = msg;
                        return console.error("Failed to submit login form: " + msg);
                    });
            }
        })
        .catch(err => console.error(err));
    }
</script>

<svelte:head>
	<title>Login</title>
	<meta name="description" content="Login with username and password." />
</svelte:head>

<section>
	<h1>Login</h1>

    {#if errMessage.length > 0}
        <p>{errMessage}</p>
    {/if}
    <div class="login-form">
        <div>
            <input
                id="username"
                placeholder="Username"
                value={loginForm.username}
                on:input={e => loginForm.username = e.currentTarget.value}
            >
        </div>
        <div>
            <input
                id="password"
                type="password"
                placeholder="Password"
                value={loginForm.password}
                on:input={e => loginForm.password = e.currentTarget.value}
            >
        </div>
        <button id="submit" on:click={submitLogin}>Login</button>
    </div>

    <p>No user? <a href="/register">Register</a></p>
</section>