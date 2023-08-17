<script lang="ts">
	import { PRODUCT_API_URL } from "../../utils/const";
    import type { RegisterForm } from "../../types/register";
    import { goto } from "$app/navigation";

    let registerForm: RegisterForm = {
        username: "",
        password: "",
        passwordConfirm: ""
    }
    let errMessage = "";
    let errMessage2 = "";

    // Submits register form to the backend
    let submitRegister = () => {
        fetch(`${PRODUCT_API_URL}/account/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(registerForm)
        })
        .then(response => {
            if (response.ok) {
                goto("/register/success");
            } else {
                response
                    .text()
                    .then(msg => {
                        errMessage = msg
                        console.error("Failed to submit register form: " + msg)
                    });
            }
        })
        .catch(err => {
            errMessage2 = "Failed to register";
            console.error(err)
        });
    }
</script>

<svelte:head>
	<title>Register</title>
	<meta name="description" content="Create a new user account." />
</svelte:head>

<section>
	<h1>Create a new user account</h1>

    {#if errMessage2.length > 0}
        <p>{errMessage2}</p>
    {/if}
    {#if errMessage.length > 0}
        <p>{errMessage}</p>
    {/if}
    <div class="register-form">
        <div>
            <input
                id="username"
                placeholder="Username"
                value={registerForm.username}
                on:input={e => registerForm.username = e.currentTarget.value}
            >
        </div>
        <div>
            <input
                id="password"
                type="password"
                placeholder="Password"
                value={registerForm.password}
                on:input={e => registerForm.password = e.currentTarget.value}
            >
        </div>
        <div>
            <input
                id="confirm-password"
                type="password"
                placeholder="Confirm password"
                value={registerForm.passwordConfirm}
                on:input={e => registerForm.passwordConfirm = e.currentTarget.value}
            >
        </div>
        <button id="submit" on:click={submitRegister}>Register</button>
    </div>

    <p>Already a user? <a href="/login">Login</a></p>
</section>